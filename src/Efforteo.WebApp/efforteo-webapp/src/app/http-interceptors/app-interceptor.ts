import {Injectable} from '@angular/core';
import {HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';

@Injectable()
export class AppInterceptor implements HttpInterceptor {

  constructor() {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const contentType = req.clone({
      setHeaders:
        {'Content-Type': 'application/json'}
    });

    return next.handle(contentType);
  }
}
