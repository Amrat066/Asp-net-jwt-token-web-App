import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { RegisterRequest } from '../../Model/register-request';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  errorMessage = '';
  isSubmitting = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {}

  registerForm = this.fb.group({
    fullName: [''],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit() {
    this.errorMessage = '';
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const payload = this.registerForm.value as RegisterRequest;
    this.authService.register(payload).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/itemMaster']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err?.error ?? 'Registration failed. Please try again.';
      }
    });
  }
}
