import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginRequest } from '../../Model/login-request';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  errorMessage = '';
  isSubmitting = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {}

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  onSubmit() {
    this.errorMessage = '';
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const payload = this.loginForm.value as LoginRequest;
    this.authService.login(payload).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/itemMaster']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err?.error ?? 'Login failed. Please try again.';
      }
    });
  }
}
