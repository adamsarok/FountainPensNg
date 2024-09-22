import { ResolveFn } from '@angular/router';
import { InkForListDTO } from '../../dtos/InkForListDTO';
import { InkService } from '../services/ink.service';
import { inject } from '@angular/core';
import { catchError, of } from 'rxjs';

export const inksResolver: ResolveFn<InkForListDTO[]> = () => {
  const inkService = inject(InkService);
  return inkService.getInks().pipe(
    catchError(error => {
      console.error('Error in resolver', error);
      return of([]);
    })
  );
};
