import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { AuthService } from '../../../../core/services/auth.service';
import { LoginUserDTO } from '../../models/login-user.dto';
import { CoreService } from '../../../../core/services/core.service';
import { SessionService } from '../../../../core/services/session.service';
import { SessionDTO } from '../../models/session.dto';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  form: FormGroup;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private coreService: CoreService,
    private sessionService: SessionService
  ) {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  ngOnInit(): void { }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    const loginUser: LoginUserDTO = {
      email: this.form.value.email,
      password: this.form.value.password,
    };

    this.authService.login(loginUser).subscribe({
      next: () => this.onAuthenticated(),
      error: () => {
        //errors handled globally
      },
    });
  }

  private onAuthenticated(): void {
    this.coreService
      .get<{ result: SessionDTO }>('sessions')
      .subscribe({
        next: (res) => {
          this.sessionService.setSession(res.result);
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          console.error('Session load failed', err);
        }
      });
  }
}
