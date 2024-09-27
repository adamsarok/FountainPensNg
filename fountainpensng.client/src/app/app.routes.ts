import { Routes } from '@angular/router';
//import { HomeComponent } from './home/home.component';
import { InkComponent } from './components/ink/ink.component';
import { inksResolver } from './resolvers/inks.resolver';
import { pensResolver } from './resolvers/pens.resolver';
import { PenComponent } from './components/pen/pen.component';
import { InkListComponent } from './components/ink-list/ink-list.component';
import { PenListComponent } from './components/pen-list/pen-list.component';
import { InkedupListComponent } from './components/inkedup-list/inkedup-list.component';
import { InkedupComponent } from './components/inkedup/inkedup.component';
import { ColorWheelComponent } from './components/color-wheel/color-wheel.component';

export const routes: Routes = [
  { path: '', component: PenListComponent },
  { path: 'inkedup-list', component: InkedupListComponent },
  { path: 'pen-list', component: PenListComponent },
  { path: 'ink-list', component: InkListComponent, resolve: {inks: inksResolver} },
  { path: 'pen', component: PenComponent, resolve: {inks: inksResolver} },
  { path: 'pen/:id', component: PenComponent, resolve: {inks: inksResolver } },
  { path: 'ink', component: InkComponent },
  { path: 'ink/:id', component: InkComponent },
  { path: 'inked-up', component: InkedupComponent, resolve: { inks: inksResolver, pens: pensResolver } },
  { path: 'inked-up/:id', component: InkedupComponent, resolve: { inks: inksResolver, pens: pensResolver } },
  { path: 'colors', component: ColorWheelComponent, resolve: { pens: pensResolver } },
];

