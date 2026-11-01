import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginResponse {
  message: string;
  userCode: string;
  locations: {
    Location_Code: string;
    Location_Name: string;
  }[];
}

export interface Location {
  location_Code: string;
  location_Name: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5286/api';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.apiUrl}/Auth/login`,
      { email, password },
      { withCredentials: true },
    );
  }

  getLocations(): Observable<Location[]> {
    return this.http.get<Location[]>(`${this.apiUrl}/Locations`, { withCredentials: true });
  }

  checkSession(): Observable<{ authenticated: boolean }> {
    return this.http.get<{ authenticated: boolean }>(`${this.apiUrl}/Auth/session`, {
      withCredentials: true,
    });
  }

  logout(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.apiUrl}/Auth/logout`,
      {},
      { withCredentials: true },
    );
  }
}
