import { ResolveFn } from '@angular/router';
import { InkForListDTO } from '../../dtos/InkForListDTO';
import { InkService } from '../services/ink.service';
import { Injectable, inject } from '@angular/core';
import { catchError, of } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
export const inksResolver: ResolveFn<InkForListDTO[]> = (route, state) => {
  const inkService = inject(InkService);
  return inkService.getInks().pipe(
    catchError(error => {
      console.error('Error in resolver', error);
      return of([]);
    })
  );
};
