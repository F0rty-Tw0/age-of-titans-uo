using System;
using System.Reflection;
using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

// Self-checking invariant: IsEmpty must be true iff every property is left at its default value.
// Setting ANY single public property to a non-default value must flip IsEmpty to false. This
// makes a future field addition to a row struct fail loudly here instead of silently leaving a
// stale IsEmpty that omits the new field (the bug this test was written to catch).
public class EffectRowInvariantTests
{
    [Fact]
    public void WeaponEffectRow_DefaultIsEmpty_EveryPropertyBreaksIt() =>
        AssertEveryPropertyBreaksEmpty<WeaponEffectRow>(row => row.IsEmpty);

    [Fact]
    public void ArmorEffectRow_DefaultIsEmpty_EveryPropertyBreaksIt() =>
        AssertEveryPropertyBreaksEmpty<ArmorEffectRow>(row => row.IsEmpty);

    [Fact]
    public void AccessoryEffectRow_DefaultIsEmpty_EveryPropertyBreaksIt() =>
        AssertEveryPropertyBreaksEmpty<AccessoryEffectRow>(row => row.IsEmpty);

    private static void AssertEveryPropertyBreaksEmpty<T>(Func<T, bool> isEmpty) where T : struct
    {
        Assert.True(isEmpty(default));

        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.SetMethod == null)
            {
                continue; // computed properties (IsEmpty itself) carry no backing field to set
            }

            object boxedRow = default(T);

            // `init`-only setters carry an IsExternalInit modreq that PropertyInfo.SetValue's
            // default binder refuses to call ("Property set method not found"); MethodInfo.Invoke
            // has no such restriction, so invoke the compiler-generated setter directly.
            property.SetMethod.Invoke(boxedRow, new[] { NonDefaultValue(property.PropertyType) });

            Assert.False(isEmpty((T)boxedRow),
                $"{typeof(T).Name}.{property.Name} left IsEmpty true when set to a non-default value");
        }
    }

    private static object NonDefaultValue(Type type)
    {
        if (type == typeof(bool))
        {
            return true;
        }

        if (type == typeof(int))
        {
            return 1;
        }

        if (type == typeof(short))
        {
            return (short)1;
        }

        if (type == typeof(ClauseType))
        {
            return ClauseType.CritEveryN;
        }

        throw new NotSupportedException($"Unhandled row property type {type}");
    }
}
