
import { Component, ChangeDetectorRef } from '@angular/core';
import { Navbar } from '../../../../layout/components/navbar/navbar';
import { RoleService } from '../../../roles/role.service';
import { RoleDetailDTO } from '../../../auth/models/role-detail.dto';
import { ActivatedRoute } from '@angular/router';
import { SessionService } from '../../../../core/services/session.service';
import { filter, take, switchMap } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  imports: [Navbar],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  items = Array.from({ length: 50 });

  roles: RoleDetailDTO[] = [];
  section = 'home';
  loading = false;

  constructor(
    private route: ActivatedRoute,
    private roleService: RoleService,
    private sessionService: SessionService,
    private cdr: ChangeDetectorRef        // ← add this
  ) {
    this.route.paramMap.subscribe(params => {
      this.section = params.get('section') ?? 'home';
      this.roles = [];

      switch (this.section) {
        case 'roles':
          this.loadRoles();
          break;
        case 'home':
          break;
      }
    });
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
}