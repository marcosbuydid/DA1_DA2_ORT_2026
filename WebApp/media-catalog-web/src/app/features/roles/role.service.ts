import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { RoleDetailDTO } from '../auth/models/role-detail.dto';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.baseUrl}roles`;

  getRoles(): Observable<RoleDetailDTO[]> {

    return this.http
      .get<{ result: RoleDetailDTO[] }>(this.apiUrl)
      .pipe(
        map(response => response.result)
      );
  }
}