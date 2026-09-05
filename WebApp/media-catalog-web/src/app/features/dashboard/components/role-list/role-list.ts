import { Component, ChangeDetectorRef } from '@angular/core';
import { RoleService } from '../../../roles/role.service';
import { RoleDetailDTO } from '../../../auth/models/role-detail.dto';
import { SessionService } from '../../../../core/services/session.service';
import { filter, take, switchMap, Observable, of, map } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-role-list',
  imports: [CommonModule],
  templateUrl: './role-list.html',
  styleUrl: './role-list.css',
})
export class RoleList {
  items = Array.from({ length: 50 });
  roles: RoleDetailDTO[] = [];
  loading = false;
  isAdmin$: Observable<boolean> = of(false);

  constructor(
    private roleService: RoleService,
    private sessionService: SessionService,
    private cdr: ChangeDetectorRef
  ) {
    this.isAdmin$ = this.sessionService.session$.pipe(
      map(session => session?.loggedUserRoleName === 'Administrator')
    );
    this.roles = [];
    this.loadRoles();
  }

  private loadRoles(): void {
    this.loading = true;
    this.sessionService.session$
      .pipe(
        filter(session => session !== null),
        take(1),
        switchMap(() => this.roleService.getRoles())
      )
      .subscribe({
        next: response => {
          this.roles = response;
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: error => {
          console.error(error);
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
  }

  deleteRole(name: string): void {
    if (!confirm('Are you sure you want to delete this role?')) return;
    this.roleService.deleteRole(name).subscribe({
      next: () => {
        this.roles = this.roles.filter(r => r.name !== name);
        this.cdr.detectChanges();
      },
      error: err => console.error(err)
    });
  }
}
