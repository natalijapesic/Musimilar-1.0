import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterRequest } from '@app/_requests';
import { UserService } from '@app/_services';

@Component({
  selector: 'app-register',
  templateUrl: 'register.component.html',
  styleUrls: ['register.component.css']
})
export class RegisterComponent implements OnInit {

  isNameGood: boolean = true;
  doesNameExist: boolean = true;
  isEmailGood:boolean = true;
  emailInput: string = '';
  nameInput: string = '';
  passwordInput: string = '';
  errorMessage: string = '';


  constructor(public userService: UserService, 
              private errorLabel: ElementRef,
              public router: Router) 
              {
                this.errorLabel.nativeElement;
                
              }

  ngOnInit(){}

  checkEmail(email: string){
    console.log(email);
    if (email.indexOf("@") === -1 || (email.indexOf(".com") === -1 && email.indexOf(".rs") === -1))
      this.isEmailGood = false;
    else
      this.isEmailGood = true;
    this.setErrorLabel();
    this.emailInput = email;
  }

  setErrorLabel(){
    if(!this.isEmailGood){  
      this.errorMessage = "Email must contain @ and .com or .rs";
    }
    else if(!this.isNameGood){
      this.errorMessage = "Name may conatain only letters, numbers, _ and .";
    }    
  }

  checkName(name: string){
    console.log(name);
    if (this.validateName(name))
      this.isNameGood = false;
    else
      this.isNameGood = true;
    this.setErrorLabel();
    this.nameInput = name;
}

  checkPassword(password: string){
    this.passwordInput = password;
  }
  
  validateName(name: string){
    let flag = true;
    for (let i = 0; i < name.length; i++) {
      let code = name.charCodeAt(i);
      if (code < 48 && code != 46) {
        flag = false;
      }
      else if (code > 57 && code < 65) {
        flag = false;
      }
      else if (code > 90 && code < 97 && code != 95) {
        flag = false;
      }
      else if (code > 122) {
        flag = false;
      }
    }
    return flag;
  }

  onSignUp(){
    if(!this.emailInput || !this.passwordInput || !this.nameInput)
      alert("All fields must be filled");
    
    let request =  new RegisterRequest(this.nameInput, this.passwordInput, this.emailInput);

    this.userService.register(request).subscribe(response =>{

      console.log(response);
      this.router.navigate(["/login"]);
        
    },
    (error: HttpErrorResponse) => {
      alert("Check your email or user already exists.");
    })
  }

}
