import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/ngx-bootstrap-datepicker';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/user';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>; // With a partial class you can make required types optional

  constructor(private fb: FormBuilder, private authService: AuthService, private toastr: ToastrService) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-blue',
      isAnimated: true
    };
    this.createRegisterForm();
  }
      // minLength or maxLength(or other reserved keywords) should be in lower case in html
      // confirmPassword can have camelcasing
  createRegisterForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', Validators.required],
      password:  ['',
         [Validators.required, Validators.minLength(4), Validators.maxLength(8)]
        ],
      confirmPassword: ['', Validators.required],
    //   gender: ['male'],
    // //  dateOfBirth: [null, Validators.required],
    //   country: ['', Validators.required],
    //   city: ['', Validators.required]
     }, {validator: this.passwordMatchValidator});
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : {mismatch: true};
  }

    register() {
      this.authService.register(this.registerForm.value).subscribe( data => {
        this.toastr.success('Registering user...');
      });
    }

  cancel() {
    this.cancelRegister.emit(false);
  }

  onSubmit() {
    // console.warn(this.registerForm.value);
  }

}
