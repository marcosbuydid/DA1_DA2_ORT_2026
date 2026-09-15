import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class CoreService {
    private readonly baseUrl = environment.baseUrl;

    constructor(protected http: HttpClient) { }

    public get<R>(path: string, params?: HttpParams): Observable<R> {
        return this.http.get<R>(this.baseUrl + path, { params });
    }

    public post<S, R>(path: string, body?: S, params?: HttpParams): Observable<R> {
        return this.http.post<R>(this.baseUrl + path, body, { params });
    }

    public put<S, R>(path: string, body: S): Observable<R> {
        return this.http.put<R>(this.baseUrl + path, body);
    }

    public delete<R>(path: string): Observable<R> {
        return this.http.delete<R>(this.baseUrl + path);
    }
}