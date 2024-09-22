import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  
  return next(req).pipe(
    catchError((e: HttpErrorResponse) => {
      if (e) {
        switch (e.status) {
          case 400:
            if (e.error.errors) {
              const modelStateErrors = [];
              for (const key in e.error.errors) {
                if (e.error.errors[key]) {
                  modelStateErrors.push(e.error.errors[key]);
                }
              }
              throw modelStateErrors.flat();
            } else {
              throw(e.error, e.status.toString());
            }
        }
      }
      throw e;
    })
  )
};
