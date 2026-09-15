import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RoleService } from '../../../roles/role.service';
import { SessionService } from '../../../../core/services/session.service';
import { filter, take, switchMap } from 'rxjs';

@Component({
  selector: 'app-add-role',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-role.html',
  styleUrl: './add-role.css',
})
export class AddRole {
  addRoleForm: FormGroup;
  submitted = false;
  submitSuccess = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private roleService: RoleService,
    private sessionService: SessionService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.addRoleForm = this.fb.group({
      name: ['', [Validators.required, Validators.pattern('^(Administrator|User)$')]],
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.addRoleForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.addRoleForm.invalid) return;
  console.log('Sending:', this.addRoleForm.value); 
    this.sessionService.session$.pipe(
      filter(s => s !== null),
      take(1),
      switchMap(() => this.roleService.createRole(this.addRoleForm.value))
    ).subscribe({
      next: () => {
        this.submitSuccess = true;
        this.submitted = false;
        this.addRoleForm.reset();
        this.cdr.detectChanges();
      },
      error: err => {
        this.errorMessage = err?.error?.message ?? 'An error occurred. Please try again.';
        console.log(this.errorMessage)
        this.cdr.detectChanges();
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/dashboard/home']);
  }
}
