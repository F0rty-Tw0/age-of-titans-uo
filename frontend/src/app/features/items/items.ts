import { Component, inject } from '@angular/core';

import { WikiData } from '../../core/wiki-data';

@Component({
  selector: 'aot-items',
  imports: [],
  templateUrl: './items.html',
  styleUrl: './items.scss',
})
export class Items {
  protected readonly data = inject(WikiData);
}
