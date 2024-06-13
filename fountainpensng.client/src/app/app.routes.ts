import { Routes } from '@angular/router';
import { InkListComponent } from './ink-list/ink-list.component';
import { PenListComponent } from './pen-list/pen-list.component';
import { HomeComponent } from './home/home.component';
import { PenComponent } from './pen/pen.component';
import { InkComponent } from './ink/ink.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'pen-list', component: PenListComponent },
  { path: 'ink-list', component: InkListComponent },
  { path: 'pen', component: PenComponent },
  { path: 'ink', component: InkComponent }
];

