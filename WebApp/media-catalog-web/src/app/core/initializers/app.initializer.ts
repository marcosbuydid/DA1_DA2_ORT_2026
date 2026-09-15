import { provideAppInitializer, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { SessionService } from '../services/session.service';

export const appInitializer = provideAppInitializer(() => {
  const authService = inject(AuthService);
  const sessionService = inject(SessionService);

  if (!sessionService.hasToken()) {
    return;
  }

  return firstValueFrom(authService.loadCurrentSession())
    .then(() => undefined)
    .catch(() => {
      sessionService.clear();
    });
});