import { Component, inject } from '@angular/core';
import { SessionService } from '../../../core/services/session.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  private authService = inject(AuthService);
  private router = inject(Router);
  private sessionService = inject(SessionService);

  session$ = this.sessionService.session$;

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}
