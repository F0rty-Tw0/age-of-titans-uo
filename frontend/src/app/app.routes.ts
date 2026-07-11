import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./features/home/home').then(m => m.Home), title: 'Age of Titans' },
  { path: 'items', loadComponent: () => import('./features/items/items').then(m => m.Items), title: 'Items — Age of Titans' },
  { path: 'monsters', loadComponent: () => import('./features/monsters/monsters').then(m => m.Monsters), title: 'Monsters — Age of Titans' },
  { path: 'skills', loadComponent: () => import('./features/skills/skills').then(m => m.Skills), title: 'Skills — Age of Titans' },
  { path: 'mechanics', loadComponent: () => import('./features/mechanics/mechanics').then(m => m.Mechanics), title: 'Mechanics — Age of Titans' },
  { path: 'gathering', loadComponent: () => import('./features/gathering/gathering').then(m => m.Gathering), title: 'Gathering — Age of Titans' },
  { path: 'professions', loadComponent: () => import('./features/professions/professions').then(m => m.Professions), title: 'Professions — Age of Titans' },
  { path: '**', redirectTo: '' },
];
