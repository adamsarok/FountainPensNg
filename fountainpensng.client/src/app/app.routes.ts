import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { InkComponent } from './components/ink/ink.component';
import { inksResolver } from './resolvers/inks.resolver';
import { PenComponent } from './components/pen/pen.component';
import { InkListComponent } from './components/ink-list/ink-list.component';
import { PenListComponent } from './components/pen-list/pen-list.component';
import { InkedupListComponent } from './components/inkedup-list/inkedup-list.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'inkedup-list', component: InkedupListComponent },
  { path: 'pen-list', component: PenListComponent },
  { path: 'ink-list', component: InkListComponent, resolve: {inks: inksResolver} },
  { path: 'pen', component: PenComponent, resolve: {inks: inksResolver} },
  { path: 'pen/:id', component: PenComponent, resolve: {inks: inksResolver } },
  { path: 'ink', component: InkComponent },
  { path: 'ink/:id', component: InkComponent }
];

