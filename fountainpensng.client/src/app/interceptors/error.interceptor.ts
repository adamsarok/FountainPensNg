import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { NavigationExtras } from '@angular/router';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  
  return next(req).pipe(
    catchError((e: HttpErrorResponse) => {
      if (e) {
        switch (e.status) {
          case 400: //simple bad request or validation error
            if (e.error.errors) {
              const modelStateErrors = []; //validation error
              for (const key in e.error.errors) {
                if (e.error.errors[key]) {
                  modelStateErrors.push(e.error.errors[key]);
                  //this.toastr.error(e.error.errors[key]);
                }
              }
              throw modelStateErrors.flat();
            } else {
              throw(e.error, e.status.toString());
            }
            break;
          // case 401:
          //   this.toastr.error('Unauthorised', e.status.toString());
          //   break;
          // case 404:
          //   this.router.navigateByUrl('/not-found');
          //   break;
          // case 500:
          //   const navigationExtras: NavigationExtras = {state: {error: e.error}};
          //   this.router.navigateByUrl('/server-error', navigationExtras);
          //   break;
          // default:
          //   this.toastr.error('Unexpected error');
          //   console.log(e);
        }
      }
      throw e;
    })
  )
};
