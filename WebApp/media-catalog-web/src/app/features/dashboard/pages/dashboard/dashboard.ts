
import { Component, ChangeDetectorRef } from '@angular/core';
import { Navbar } from '../../../../layout/components/navbar/navbar';
import { RoleService } from '../../../roles/role.service';
import { RoleDetailDTO } from '../../../auth/models/role-detail.dto';
import { ActivatedRoute } from '@angular/router';
import { SessionService } from '../../../../core/services/session.service';
import { filter, take, switchMap, Observable, of, map } from 'rxjs';
import { UserService } from '../../../users/user.service';
import { UserDetailDTO } from '../../../auth/models/user-detail.dto';
import { CommonModule, DatePipe } from '@angular/common';
import { MovieDetailDTO } from '../../../auth/models/movie-detail.dto';
import { MovieService } from '../../../movies/movie.service';

@Component({
  selector: 'app-dashboard',
  imports: [Navbar, DatePipe, CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  items = Array.from({ length: 50 });

  roles: RoleDetailDTO[] = [];
  users: UserDetailDTO[] = [];
  movies: MovieDetailDTO[] = [];
  section = 'home';
  loading = false;
  isAdmin$: Observable<boolean> = of(false);
  loggedUserEmail: string | null = null;
  deleteUserAlertVisible = false;

  constructor(
    private route: ActivatedRoute,
    private roleService: RoleService,
    private sessionService: SessionService,
    private userService: UserService,
    private movieService: MovieService,
    private cdr: ChangeDetectorRef
  ) {
    this.isAdmin$ = this.sessionService.session$.pipe(
      map(session => session?.loggedUserRoleName === 'Administrator')
    );
    this.sessionService.session$.pipe(
      filter(s => s !== null),
      take(1)
    ).subscribe(session => {
      this.loggedUserEmail = session!.loggedUser.email;
    });
    this.route.paramMap.subscribe(params => {
      this.section = params.get('section') ?? 'home';
      this.roles = [];
      this.users = [];
      this.movies = [];

      switch (this.section) {
        case 'roles':
          this.loadRoles();
          break;
        case 'home':
          break;
        case 'users':
          this.loadUsers();
          break;
        case 'movies':
          this.loadMovies();
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

  private loadUsers(): void {
    this.loading = true;
    this.sessionService.session$
      .pipe(
        filter(session => session !== null),
        take(1),
        switchMap(() => this.userService.getUsers())
      )
      .subscribe({
        next: response => {
          this.users = response;
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

  private loadMovies(): void {
    this.loading = true;
    this.sessionService.session$
      .pipe(
        filter(session => session !== null),
        take(1),
        switchMap(() => this.movieService.getMovies())
      )
      .subscribe({
        next: response => {
          this.movies = response;
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

  deleteUser(email: string): void {
    if (email === this.loggedUserEmail) {
      this.deleteUserAlertVisible = true;
      this.cdr.detectChanges();
      return;
    }

    if (!confirm('Are you sure you want to delete this user?')) return;

    this.userService.deleteUser(email).subscribe({
      next: () => {
        this.users = this.users.filter(u => u.email !== email);
        this.cdr.detectChanges();
      },
      error: err => console.error(err)
    });
  }

  closedeleteUserAlert(): void {
    this.deleteUserAlertVisible = false;
  }

  deleteMovie(title: string): void {
    if (!confirm('Are you sure you want to delete this movie?')) return;
    this.movieService.deleteMovie(title).subscribe({
      next: () => {
        this.movies = this.movies.filter(m => m.title !== title);
        this.cdr.detectChanges();
      },
      error: err => console.error(err)
    });
  }
}