export interface LoginResponse {
  id: number;
  name: string;
  email: string;
  role: string;
  success: boolean;
  error: string;
  token: string;
}
