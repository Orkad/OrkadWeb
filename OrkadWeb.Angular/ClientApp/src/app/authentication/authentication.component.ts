import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from '@services/notification.service';
import { AuthenticationService } from './authentication.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit {
  loading: boolean = false;
  loginForm: FormGroup;
  error: string;

  constructor(private authenticationService: AuthenticationService, 
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private router: Router) { 
    this.loginForm = fb.group({
      username: [''],
      password: [''],
    });
  }

  ngOnInit(): void {
  }

  login(): void {
    this.loading = true;
    console.log(this.loginForm);
    this.authenticationService.login(this.loginForm.value['username'], this.loginForm.value['password'])
      .subscribe(data => {
        if (data.error){
          this.error =  data.error;
          this.loading = false;
        }
        localStorage.setItem("jwt", data.token);
        this.router.navigate(["/"]);
      });
  }
}