import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ComparerService {

  //constructor() { }

  compare(a: number | string | Date, b: number | string | Date, isAsc: boolean) {
    if (a instanceof Date && b instanceof Date) {
      return (a.getTime() < b.getTime() ? -1 : 1) * (isAsc ? 1 : -1);
    } else if ((typeof a === 'number' && typeof b === 'number') || (typeof a === 'string' && typeof b === 'string')) {
      return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
    } else {
      return 0;
    }
  }
}
