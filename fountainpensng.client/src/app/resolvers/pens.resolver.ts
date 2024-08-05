import { ResolveFn } from '@angular/router';
import { FountainPen } from '../../dtos/FountainPen';
import { inject } from '@angular/core';
import { catchError, of } from 'rxjs';
import { PenService } from '../services/pen.service';

export const pensResolver: ResolveFn<FountainPen[]> = (route, state) => {
  const penService = inject(PenService);
  return penService.getPens().pipe(
    catchError(error => {
      console.error('Error in resolver', error);
      return of([]);
    })
  );
};
