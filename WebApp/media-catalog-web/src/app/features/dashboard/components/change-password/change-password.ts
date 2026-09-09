import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../users/user.service';
import { SessionService } from '../../../../core/services/session.service';
import { filter, take, switchMap } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './change-password.html',
})
export class ChangePassword {
  changePasswordForm: FormGroup;
  submitted = false;
  errorMessage = '';
  submitSuccess = false;
  loggedUserEmail: string | null = null;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private sessionService: SessionService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.changePasswordForm = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      retypedNewPassword: ['', Validators.required],
    }, { validators: this.passwordsMatchValidator });

    this.sessionService.session$.pipe(
      filter(s => s !== null),
      take(1)
    ).subscribe(session => {
      this.loggedUserEmail = session!.loggedUser.email;
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.changePasswordForm.controls;
  }

  private passwordsMatchValidator(form: AbstractControl) {
    const newPassword = form.get('newPassword')?.value;
    const retypedNewPassword = form.get('retypedNewPassword')?.value;
    return newPassword === retypedNewPassword ? null : { passwordsMismatch: true };
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.changePasswordForm.invalid) return;

    this.sessionService.session$.pipe(
      filter(s => s !== null),
      take(1),
      switchMap(() => this.userService.changePassword(
        this.loggedUserEmail!,
        this.changePasswordForm.value
      ))
    ).subscribe({
      next: () => {
        this.submitSuccess = true;
        this.submitted = false;
        this.changePasswordForm.reset();
        this.cdr.detectChanges();
      },
      error: err => {
        this.errorMessage = err?.error?.message ?? 'An error occurred. Please try again.';
        console.error(this.errorMessage);
        this.cdr.detectChanges();
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/dashboard/home']);
  }
}
