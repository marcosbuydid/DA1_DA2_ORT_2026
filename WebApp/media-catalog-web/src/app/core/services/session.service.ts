import { Injectable } from '@angular/core';
import { TokenDTO } from '../../features/auth/models/token.dto';

@Injectable({
    providedIn: 'root'
})
export class SessionService {
    private readonly tokenKey = 'token';

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
}