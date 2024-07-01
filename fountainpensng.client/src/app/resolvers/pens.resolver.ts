// import { ResolveFn } from '@angular/router';

// export const pensResolver: ResolveFn<boolean> = (route, state) => {
//   return true;
// };

import { ResolveFn } from '@angular/router';
import { FountainPen } from '../../dtos/FountainPen';
import { Injectable, inject } from '@angular/core';
import { catchError, of } from 'rxjs';
import { PenService } from '../services/pen.service';

// @Injectable({
//   providedIn: 'root'
// })
export const pensResolver: ResolveFn<FountainPen[]> = (route, state) => {
  const penService = inject(PenService);
  return penService.getPens().pipe(
    catchError(error => {
      console.error('Error in resolver', error);
      return of([]);
    })
  );
};
