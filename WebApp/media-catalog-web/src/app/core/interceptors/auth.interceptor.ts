import { Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
    HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastService } from '../../shared/services/toast.service';
import { SessionService } from '../../core/services/session.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(
        private router: Router,
        private toastService: ToastService,
        private session: SessionService
    ) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.session.getToken()?.value;

        let authReq = req;
        if (token) {
            authReq = req.clone({
                setHeaders: { Authorization: `Bearer ${token}` }
            });
        }

        return next.handle(authReq).pipe(
            catchError((error: HttpErrorResponse) => {
                this.handleError(error);
                return throwError(() => error);
            })
        );
    }

    private handleError(error: HttpErrorResponse) {
        let message = 'An unknown error occurred';
        //determine message from backend response
        if (error.error) {
            if (typeof error.error === 'string') {
                message = error.error;
            } else if (error.error.message) {
                message = error.error.message;
            } else if (error.message) {
                message = error.message;
            }
        }

        switch (error.status) {
            case 401:
                this.session.removeToken();
                this.router.navigate(['auth/login'], { replaceUrl: true });
                break;
            case 400:
                this.toastService.show(message, 'warning');
                break;
            case 403:
                this.toastService.show(message, 'warning');
                break;
            case 404:
                this.toastService.show(message, 'warning');
                break;
            case 409:
                this.toastService.show(message, 'warning');
                break;
            case 422:
                this.toastService.show(message, 'warning');
                break;
            case 500:
                this.toastService.show(message, 'error');
                break;
            case 0:
                this.toastService.show('Cannot connect to the server', 'error');
                break;
        }
    }
}