import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

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

    constructor(public authenticationService: AuthenticationService, 
                public router: Router) { }


    ngOnInit() {
        let passwordInput = document.getElementById("password")
        console.log(passwordInput);
    }

    setErrorLabel(errorLabel){

        let errorMessage = "";

        if (this.wrongPassword) {
            errorMessage = "Wrong password";
          }
          else if (this.wrongEmail) {
            errorMessage = "Wrong email";
          }
          else if (this.emptyString) {
            errorMessage = "All fields must be filled";
          }
          errorLabel.innerHTML = errorMessage;
    }

    onSignIn(){
        let errorLabel = document.getElementById("error");
        const email: string = (document.getElementById("e-mail") as HTMLInputElement).value;
        const password = (document.getElementById("password") as HTMLInputElement).value;

        if (!email || !password) {
            this.emptyString = true;
            this.setErrorLabel(errorLabel);
        } else {
            this.emptyString = false;
            this.setErrorLabel(errorLabel);
      
            let request = {
              password: password,
              email: email
            }

            this.authenticationService.login(request).subscribe(response =>
              {
                console.log(response);
              })

        }      
    }

}