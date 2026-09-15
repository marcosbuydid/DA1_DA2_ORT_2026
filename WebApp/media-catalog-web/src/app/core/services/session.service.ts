import { Injectable } from '@angular/core';
import { TokenDTO } from '../../features/auth/models/token.dto';
import { BehaviorSubject, Observable } from 'rxjs';
import { SessionDTO } from '../../features/auth/models/session.dto';

@Injectable({
    providedIn: 'root'
})
export class SessionService {
    private readonly tokenKey = 'token';

    private readonly sessionSubject =
        new BehaviorSubject<SessionDTO | null>(null);

    session$: Observable<SessionDTO | null> =
        this.sessionSubject.asObservable();

    setToken(token: TokenDTO): void {
        try {
            localStorage.setItem(this.tokenKey, JSON.stringify(token));
        } catch (err) {
            console.error('Failed to save token to localStorage', err);
        }
    }

    getToken(): TokenDTO | null {
        const json = localStorage.getItem(this.tokenKey);
        if (!json) return null;

        try {
            return JSON.parse(json) as TokenDTO;
        } catch (err) {
            console.warn('Failed to parse token from localStorage, removing it.', err);
            localStorage.removeItem(this.tokenKey);
            return null;
        }
    }

    removeToken(): void {
        localStorage.removeItem(this.tokenKey);
    }

    hasToken(): boolean {
        const token = this.getToken();
        return token !== null && !!token.value;
    }

    setSession(session: SessionDTO | null): void {
        this.sessionSubject.next(session);
    }

    getCurrentSession(): SessionDTO | null {
        return this.sessionSubject.value;
    }

    clear(): void {
        this.removeToken();
        this.sessionSubject.next(null);
    }
}