import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../../users/user.service';
import { RoleService } from '../../../roles/role.service';
import { RoleDetailDTO } from '../../../auth/models/role-detail.dto';
import { SessionService } from '../../../../core/services/session.service';
import { filter, take, switchMap } from 'rxjs';

@Component({
  selector: 'app-add-user',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-user.html',
})
export class AddUser {
  addUserForm: FormGroup;
  submitted = false;
  errorMessage = '';
  submitSuccess = false;
  roles: RoleDetailDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private roleService: RoleService,
    private sessionService: SessionService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.addUserForm = this.fb.group({
      name:     ['', Validators.required],
      lastName: ['', Validators.required],
      email:    ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      roleId:   [0,  [Validators.required, Validators.min(1)]],
    });

    this.loadRoles();
  }

  get f(): { [key: string]: AbstractControl } {
    return this.addUserForm.controls;
  }

  private loadRoles(): void {
    this.sessionService.session$.pipe(
      filter(s => s !== null),
      take(1),
      switchMap(() => this.roleService.getRoles())
    ).subscribe({
      next: response => {
        this.roles = response;
        this.cdr.detectChanges();
      },
      error: err => console.error(err)
    });
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.addUserForm.invalid) return;

    this.sessionService.session$.pipe(
      filter(s => s !== null),
      take(1),
      switchMap(() => this.userService.createUser(this.addUserForm.value))
    ).subscribe({
      next: () => {
        this.submitSuccess = true;
        this.submitted = false;
        this.addUserForm.reset({ roleId: 0 });
        this.cdr.detectChanges();
      },
      error: err => {
        this.errorMessage = err?.error?.message ?? 'An error occurred. Please try again.';
        console.log(this.errorMessage);
        this.cdr.detectChanges();
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/dashboard/home']);
  }
}
