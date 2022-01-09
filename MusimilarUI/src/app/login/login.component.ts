import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginRequest } from '@app/_requests';

import { AuthenticationService } from '@app/_services';

@Component({ 
    templateUrl: 'login.component.html',
    selector: 'app-login',
    styleUrls: ['login.component.css']
})
export class LoginComponent implements OnInit {

    emptyString: boolean = false;
    wrongPassword: boolean = false;
    wrongEmail: boolean = false;
    errorMessage: string = '';

    constructor(public authenticationService: AuthenticationService, 
                public router: Router) { }


    ngOnInit() {
        let passwordInput = document.getElementById("password")
        console.log(passwordInput);
    }

    setErrorLabel(){
      if (this.wrongPassword) {
          this.errorMessage = "Wrong password";
        }
        else if (this.wrongEmail) {
          this.errorMessage = "Wrong email";
        }
        else if (this.emptyString) {
          this.errorMessage = "All fields must be filled";
        }
    }

    onSignIn(email: HTMLInputElement,  password: HTMLInputElement){
      console.log(email.value);

      if (!email || !password) {
          this.emptyString = true;
          this.setErrorLabel();
      } else
      {
        this.emptyString = false;
        this.setErrorLabel();
  
        let request = new LoginRequest(password.value, email.value);

        this.authenticationService.login(request).subscribe(response =>{
          console.log(response);
        })
        this.router.navigate(["/"]);

      }      
    }

}