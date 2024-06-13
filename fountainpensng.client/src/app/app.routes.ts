import { Routes } from '@angular/router';
import { InkListComponent } from './ink-list/ink-list.component';
import { PenListComponent } from './pen-list/pen-list.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'pen-list', component: PenListComponent },
  { path: 'ink-list', component: InkListComponent }
];

