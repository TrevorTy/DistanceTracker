import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map  } from 'rxjs/operators';
import { User } from '../_models/user';
import { Observable } from 'rxjs';
import { environment} from '../../environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root' // The root module is app.module.ts
  // A Component is by default injectable and a service needs a @injactable decorator
})
export class AuthService {

private loginPath = environment.apiUrl + 'identity/login';
private registerPath = environment.apiUrl + 'identity/register';
jwtHelper = new JwtHelperService();
decodedToken: any;

constructor(private http: HttpClient) { }
  login(data): Observable<any> {
    return this.http.post(this.loginPath, data);
  }

  register(data): Observable<any> {
    return this.http.post(this.registerPath, data);
  }

  saveToken(token) {
    localStorage.setItem('token', token);
    console.log(token);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
