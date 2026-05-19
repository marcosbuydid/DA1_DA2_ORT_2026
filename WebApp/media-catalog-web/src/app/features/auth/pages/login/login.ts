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
import { CoreService } from '../../../../core/services/core.service';
import { SessionService } from '../../../../core/services/session.service';
import { LoginUserDTO } from '../../models/login-user.dto';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  form: FormGroup;
  submitted = false;

  private readonly authPath = 'sessions'; // API endpoint

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private coreService: CoreService,
    private session: SessionService
  ) {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  ngOnInit(): void { }

  //shortcut to access form controls
  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.form.invalid) return;

    const loginUser: LoginUserDTO = {
      email: this.form.value.email,
      password: this.form.value.password,
    };

    //call API via CoreService
    this.coreService.post<LoginUserDTO, { value: string }>(this.authPath, loginUser).subscribe({
      next: (result) => this.onAuthenticated(result),
      error: () => {
        //errors handled globally by interceptor and toast service
      },
    });
  }

  private onAuthenticated(result: { value: string }) {
    this.session.setToken(result);
    this.router.navigate(['/home']);
  }
}
