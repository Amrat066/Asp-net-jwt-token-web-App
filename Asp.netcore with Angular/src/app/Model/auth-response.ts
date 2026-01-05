export interface AuthResponse {
  token: string;
  expiresAt: string;
  email: string;
  roles: string[];
}
