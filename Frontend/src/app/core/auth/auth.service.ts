import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  accessTokenExpiresAt: string;
  refreshToken: string;
  refreshTokenExpiresAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7151/api/auth';

  private readonly tokenKey = 'accessToken';
  private readonly refreshTokenKey = 'refreshToken';

  readonly isAuthenticated = signal(!!this.getAccessToken());
  readonly isAdmin = signal(this.checkAdminRole());

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      request
    ).pipe(
      tap(response => {
        localStorage.setItem(this.tokenKey, response.accessToken);
        localStorage.setItem(this.refreshTokenKey, response.refreshToken);

        this.isAuthenticated.set(true);
        this.isAdmin.set(this.checkAdminRole());
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.refreshTokenKey);

    this.isAuthenticated.set(false);
    this.isAdmin.set(false);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private checkAdminRole(): boolean {
    const token = this.getAccessToken();

    if (!token) return false;

    try {
      const payload = JSON.parse(
        atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))
      );

      const role =
        payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
        ?? payload.role
        ?? payload.roles;

      return Array.isArray(role)
        ? role.includes('Admin')
        : role === 'Admin';
    } catch {
      return false;
    }
  }
}