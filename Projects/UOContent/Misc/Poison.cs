using System;
using Server.Engines.BuffIcons;
using Server.Engines.Virtues;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;

namespace Server;

public class PoisonImpl : Poison
{
    private readonly int _count;
    private readonly TimeSpan _delay;
    private readonly TimeSpan _interval;
    private readonly int _maximum;
    private readonly int _messageInterval;
    private readonly int _minimum;
    private readonly double _scalar;

    public PoisonImpl(
        string name, int index, int level, int min, int max, double percent, double delay, double interval, int count,
        int messageInterval, PoisonFamily family = PoisonFamily.Standard
    ) : base(index)
    {
        Name = name;
        Level = level;
        Family = family;
        _minimum = min;
        _maximum = max;
        _scalar = percent * 0.01;
        _delay = TimeSpan.FromSeconds(delay);
        _interval = TimeSpan.FromSeconds(interval);
        _count = count;
        _messageInterval = messageInterval;
    }

    public override string Name { get; }

    public override int Level { get; }

    public override PoisonFamily Family { get; }

    public override Timer ConstructTimer(Mobile m) => new PoisonTimer(m, this);

    // Stackable-poison merged tick: the FIRST stack constructs this timer and donates its
    // delay/interval cadence; every tick then sums the per-stack damage of ALL active stacks
    // (each stack keeps its own T2A damage math, own last-damage quirk, and own expiry) into
    // one damage number, pruning expired stacks as it goes. The engine (Mobile.PoisonStacks)
    // owns the stack set; this timer owns the tick.
    public class PoisonTimer : Timer
    {
        private readonly Mobile _mobile;
        private readonly PoisonImpl _anchor; // cadence donor — the first stack's poison
        private int _tickIndex;
        private int _lastStackCount = 1;

        public PoisonTimer(Mobile m, PoisonImpl p) : base(p._delay, p._interval)
        {
            _mobile = m;
            _anchor = p;

            // Buff-bar icon spans the anchor stack; appended stacks re-arm it via the
            // count-change refresh in OnTick. Auto-expires on its own timer so an external
            // cure (which stops this timer without hitting the exits below) still clears it.
            var total = p._delay + TimeSpan.FromTicks(p._interval.Ticks * p._count);
            BuffHelper.AddCustomBuff(m, BuffIcon.Poison, "Poisoned", total);
        }

        // Legacy attribution knob (PlayerMobile/BaseCreature set this right after ApplyPoison).
        // Stacks now carry their own source; this remains the fallback for stacks appended
        // sourceless (direct `Poison = x` assignments).
        public Mobile From { get; set; }

        protected override void OnTick()
        {
            var stacks = _mobile.PoisonStacks;

            if (stacks.Count == 0)
            {
                BuffHelper.RemoveBuff(_mobile, BuffIcon.Poison);
                Stop();
                return;
            }

            _tickIndex++;

            // Era self-cure escapes, level-gated on the strongest active stack (the Poison
            // mirror). A successful cure clears every stack.
            var strongestLevel = _mobile.Poison?.Level ?? 0;

            if ((Core.AOS && strongestLevel < 4 &&
                 TransformationSpellHelper.UnderTransformation(_mobile, typeof(VampiricEmbraceSpell)) ||
                 strongestLevel < 3 && OrangePetals.UnderEffect(_mobile) ||
                 AnimalForm.UnderTransformation(_mobile, typeof(Unicorn))) && _mobile.CurePoison(_mobile))
            {
                if (Core.SA)
                {
                    // * You feel yourself resisting the effects of the poison *
                    _mobile.LocalOverheadMessage(MessageType.Emote, 0x3F, 1114441);
                }
                else
                {
                    _mobile.LocalOverheadMessage(
                        MessageType.Emote,
                        0x3F,
                        true,
                        "* You feel yourself resisting the effects of the poison *"
                    );
                }

                if (Core.SA)
                {
                    // * ~1_NAME~ seems resistant to the poison *
                    _mobile.NonlocalOverheadMessage(MessageType.Emote, 0x3F, 1114442, _mobile.Name);
                }
                else
                {
                    _mobile.LocalOverheadMessage(
                        MessageType.Emote,
                        0x3F,
                        true,
                        $"* {_mobile.Name} seems resistant to the poison *"
                    );
                }

                BuffHelper.RemoveBuff(_mobile, BuffIcon.Poison);
                Stop();
                return;
            }

            var total = 0;
            var remainingTicks = 0; // longest-lived stack, for the buff icon's countdown

            for (var i = stacks.Count - 1; i >= 0; i--)
            {
                var stack = stacks[i];

                if (stack.Poison is not PoisonImpl impl || stack.TicksElapsed++ >= impl._count)
                {
                    _mobile.OnPoisonStackExpired(stack); // merged tick shrinks as stacks expire
                    continue;
                }

                remainingTicks = Math.Max(remainingTicks, impl._count - stack.TicksElapsed);

                int damage;

                if (!Core.AOS && stack.LastDamage != 0 && Utility.RandomBool())
                {
                    damage = stack.LastDamage;
                }
                else
                {
                    damage = 1 + (int)(_mobile.Hits * impl._scalar);
                    damage = Math.Clamp(damage, impl._minimum, impl._maximum);

                    stack.LastDamage = damage;
                }

                var source = stack.From ?? From;

                // Darkglow: 10% damage boost when attacker is more than 1 tile away
                if (impl.Family == PoisonFamily.Darkglow && source != null && source.Map == _mobile.Map &&
                    !source.InRange(_mobile, 1))
                {
                    damage = (int)(damage * 1.1);
                    // Darkglow poison increases your damage!
                    source.SendLocalizedMessage(1072850);
                }

                // Parasitic: heals the attacker for this stack's damage when within 1 tile.
                // (Pre-merge this healed after the damage landed; inert reordering on a T2A
                // shard — Darkglow/Parasitic poisons are never registered pre-ML.)
                if (impl.Family == PoisonFamily.Parasitic && source != null && source.Map == _mobile.Map &&
                    source.InRange(_mobile, 1))
                {
                    source.Heal(damage);
                    // You have had ~1_HEALED_AMOUNT~ hit points healed.
                    source.SendLocalizedMessage(1060203, damage.ToString());
                }

                total += damage;
            }

            if (stacks.Count == 0)
            {
                _mobile.SendLocalizedMessage(502136); // The poison seems to have worn off.
                _mobile.Poison = null; // clears the mirror; stops this timer via the setter

                BuffHelper.RemoveBuff(_mobile, BuffIcon.Poison);
                Stop();
                return;
            }

            // SpellDrVsPoisonDot (rarity enabler clause): spell DR also reduces the merged tick.
            total = Engines.Rarity.RarityEffects.ReducePoisonTickDamage(_mobile, total);

            if (total > 0)
            {
                // One merged number. Attribution goes to the oldest stack's source (the timer's
                // anchor) — per-stack riders above already credited each source individually.
                var source = stacks[0].From ?? From;

                source?.DoHarmful(_mobile, true);

                (_mobile as IHonorTarget)?.ReceivedHonorContext?.OnTargetPoisoned();

                Misc.FloatingCombatText.SetPoisonContext(stacks.Count); // "-9 Poison x2"
                AOS.Damage(_mobile, source, total, 0, 0, 0, 100, 0);
                Misc.FloatingCombatText.ClearContext();

                // OSI: randomly revealed between first and third damage tick, guessing 60% chance
                if (Utility.RandomDouble() < 0.40)
                {
                    _mobile.RevealingAction();
                }

                // Merged buff-bar readout ("-{tick} poison x{n}"); re-sent only when the stack
                // count changes so the bar isn't spammed every tick.
                if (stacks.Count != _lastStackCount)
                {
                    _lastStackCount = stacks.Count;
                    BuffHelper.AddCustomBuff(
                        _mobile,
                        BuffIcon.Poison,
                        $"-{total} poison x{stacks.Count}",
                        TimeSpan.FromTicks(_anchor._interval.Ticks * (remainingTicks + 1))
                    );
                }
            }

            if (_tickIndex % _anchor._messageInterval == 0)
            {
                _mobile.OnPoisoned(stacks[0].From ?? From, _anchor, _anchor);
            }
        }
    }
}
