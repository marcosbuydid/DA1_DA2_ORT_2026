import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserDetailDTO } from '../auth/models/user-detail.dto';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.baseUrl}users`;

    getUsers(): Observable<UserDetailDTO[]> {
        return this.http
            .get<{ result: UserDetailDTO[] }>(this.apiUrl)
            .pipe(
                map(response => response.result)
            );
    }

    deleteUser(email: string): Observable<any> {
        return this.http.delete(`${this.apiUrl}/by-email/${email}`);
    }
}