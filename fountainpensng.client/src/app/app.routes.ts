import { Routes } from '@angular/router';
import { InkListComponent } from './ink-list/ink-list.component';
import { PenListComponent } from './pen-list/pen-list.component';
import { HomeComponent } from './home/home.component';
import { PenComponent } from './pen/pen.component';
import { InkComponent } from './ink/ink.component';
import { inksResolver } from './resolvers/inks.resolver';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'pen-list', component: PenListComponent },
  { path: 'ink-list', component: InkListComponent, resolve: {inks: inksResolver} },
  { path: 'pen', component: PenComponent, resolve: {inks: inksResolver} },
  { path: 'pen/:id', component: PenComponent, resolve: {inks: inksResolver } },
  { path: 'ink', component: InkComponent }
];

