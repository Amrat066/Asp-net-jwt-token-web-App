import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, tap } from 'rxjs';
import { AuthResponse } from '../Model/auth-response';
import { LoginRequest } from '../Model/login-request';
import { RegisterRequest } from '../Model/register-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authApiUrl = 'https://localhost:7086/api/Auth';
  private tokenKey = 'auth_token';
  private expiresKey = 'auth_expires';
  private emailKey = 'auth_email';
  private authState$ = new BehaviorSubject<boolean>(this.hasValidSession());

  constructor(private http: HttpClient, private router: Router) {}

  login(payload: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.authApiUrl}/login`, payload).pipe(
      tap((res) => this.setSession(res))
    );
  }

  register(payload: RegisterRequest) {
    return this.http.post<AuthResponse>(`${this.authApiUrl}/register`, payload).pipe(
      tap((res) => this.setSession(res))
    );
  }

  logout(redirect: boolean = true) {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.expiresKey);
    localStorage.removeItem(this.emailKey);
    this.authState$.next(false);
    if (redirect) {
      this.router.navigate(['/login']);
    }
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getEmail(): string | null {
    return localStorage.getItem(this.emailKey);
  }

  isLoggedIn(): boolean {
    return this.hasValidSession();
  }

  authChanges() {
    return this.authState$.asObservable();
  }

  private setSession(res: AuthResponse) {
    localStorage.setItem(this.tokenKey, res.token);
    localStorage.setItem(this.expiresKey, res.expiresAt);
    localStorage.setItem(this.emailKey, res.email);
    this.authState$.next(true);
  }

  private hasValidSession(): boolean {
    const expiresAt = localStorage.getItem(this.expiresKey);
    if (!expiresAt) {
      return false;
    }

    const expiryDate = new Date(expiresAt);
    return expiryDate.getTime() > Date.now();
  }
}
