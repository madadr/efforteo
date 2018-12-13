import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable, of, throwError} from 'rxjs';
import {Router} from '@angular/router';
import {catchError} from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    if (err.status === 401 || err.status === 403) {
      this.router.navigateByUrl(`/sign-in`);
      return of(err.message);
    }
    return throwError(err);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      const authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`
        }
      });
      return next.handle(authReq).pipe(
          catchError(x => this.handleAuthError(x))
      );
    }

    return next.handle(req);
  }
}
