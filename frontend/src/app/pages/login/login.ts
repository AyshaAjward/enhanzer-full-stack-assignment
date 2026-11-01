import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  email: string = '';
  password: string = '';

  showPassword: boolean = false;
  errorMessage: string = '';

  submitted: boolean = false;
  isLoading: boolean = false;

  onLogin(): void {
    this.submitted = true;
    this.errorMessage = '';

    if (!this.email.trim() || !this.password) {
      return;
    }

    // API integration will be added here.
    console.log('Email:', this.email);
    console.log('Login form submitted');
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }
}
