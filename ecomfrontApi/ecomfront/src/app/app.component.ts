import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from './Services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'ecomfront';
  isLoggedIn = false;
  userEmail = '';
  private authSub?: Subscription;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.syncAuthState();
    this.authSub = this.authService.authChanges().subscribe(() => this.syncAuthState());
  }

  ngOnDestroy(): void {
    this.authSub?.unsubscribe();
  }

  logout() {
    this.authService.logout();
  }

  private syncAuthState() {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.userEmail = this.authService.getEmail() || '';
  }
}
