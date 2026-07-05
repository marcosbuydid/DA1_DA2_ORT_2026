
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { CoreService } from './core.service';
import { SessionService } from './session.service';

import { LoginUserDTO } from '../../features/auth/models/login-user.dto';
import { SessionDTO } from '../../features/auth/models/session.dto';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    constructor(
        private coreService: CoreService,
        private sessionService: SessionService
    ) { }

    login(dto: LoginUserDTO): Observable<{ result: string }> {
        return this.coreService
            .post<LoginUserDTO, { result: string }>('sessions', dto)
            .pipe(
                tap(response => {
                    this.sessionService.setToken({
                        value: response.result
                    });
                })
            );
    }

    loadCurrentSession(): Observable<{ result: SessionDTO }> {
        return this.coreService
            .get<{ result: SessionDTO }>('sessions')
            .pipe(
                tap(response => {
                    this.sessionService.setSession(response.result);
                })
            );
    }

    logout(): void {
        this.sessionService.clear();
    }
}