import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [FormsModule],
  template: `
    <main class="login-page">
      <form class="login-card" (ngSubmit)="login()">
        <div class="brand">
          <span class="brand-icon">M</span>
          <span>METROFLOW</span>
        </div>

        <h1>Admin Login</h1>
        <p>Sign in to manage your metro network.</p>

        <label>Email Address</label>
        <input
          type="email"
          name="email"
          [(ngModel)]="email"
          placeholder="admin@example.com"
          required
        />

        <label>Password</label>
        <input
          type="password"
          name="password"
          [(ngModel)]="password"
          placeholder="Enter your password"
          required
        />

        @if (errorMessage()) {
          <div class="error">{{ errorMessage() }}</div>
        }

        <button type="submit" [disabled]="loading()">
          {{ loading() ? 'Signing in...' : 'Sign In' }}
        </button>
      </form>
    </main>
  `,
  styles: [`
    .login-page {
      min-height: 100vh;
      display: grid;
      place-items: center;
      background: #f1f5f9;
      padding: 24px;
    }

    .login-card {
      width: 100%;
      max-width: 420px;
      padding: 40px;
      background: white;
      border: 1px solid #e2e8f0;
      border-radius: 16px;
      box-shadow: 0 15px 40px rgba(15, 23, 42, .07);
    }

    .brand {
      display: flex;
      align-items: center;
      gap: 10px;
      color: #0f172a;
      font-weight: 800;
      letter-spacing: 1px;
      margin-bottom: 32px;
    }

    .brand-icon {
      background: #2563eb;
      color: white;
      padding: 10px 14px;
      border-radius: 10px;
    }

    h1 {
      margin: 0 0 8px;
      color: #0f172a;
    }

    p {
      color: #64748b;
      margin-bottom: 28px;
    }

    label {
      display: block;
      font-size: 14px;
      font-weight: 600;
      margin: 18px 0 8px;
    }

    input {
      width: 100%;
      box-sizing: border-box;
      padding: 13px;
      border: 1px solid #cbd5e1;
      border-radius: 8px;
      outline: none;
    }

    input:focus {
      border-color: #2563eb;
    }

    button {
      width: 100%;
      margin-top: 24px;
      padding: 14px;
      border: none;
      border-radius: 8px;
      background: #2563eb;
      color: white;
      font-weight: 700;
      cursor: pointer;
    }

    button:disabled {
      opacity: .6;
      cursor: not-allowed;
    }

    .error {
      margin-top: 16px;
      color: #dc2626;
      font-size: 14px;
    }
  `]
})
export class AdminLogin {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';

  loading = signal(false);
  errorMessage = signal('');

  login(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.authService.login({
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        if (!this.authService.isAdmin()) {
          this.authService.logout();
          this.errorMessage.set('You do not have Admin permissions.');
          this.loading.set(false);
          return;
        }

        this.router.navigate(['/admin']);
      },
      error: () => {
        this.errorMessage.set('Invalid email or password.');
        this.loading.set(false);
      }
    });
  }
}