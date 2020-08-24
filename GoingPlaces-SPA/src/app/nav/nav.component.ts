import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { FormControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  // This will store the username and password
  model: any = {};
  loginForm: FormGroup;

  constructor(private authService: AuthService, private fb: FormBuilder, private router: Router ) {
    this.loginForm = this.fb.group({
      username : ['', [Validators.required]],
      password : ['', Validators.required]
    });
  }


  ngOnInit() {
  }

  login() {
    this.authService.login(this.loginForm.value).subscribe(data => {
      this.authService.saveToken(data['token']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    //this.authService.decodedToken = null;
    //this.authService.currentUser = null;
    // this.alertify.message('logged out');
    this.router.navigate(['/home']);
      }

}
