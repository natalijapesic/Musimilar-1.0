import { Component, OnInit } from '@angular/core';
import { Route } from '@angular/router';
import { RegisterRequest } from '@app/_requests';
import { UserService } from '@app/_services';
import { fromEvent, map } from 'rxjs';

@Component({
  selector: 'register',
  templateUrl: 'register.component.html',
  styleUrls: ['register.component.css']
})
export class RegisterComponent implements OnInit {

  isNameGood: boolean = true;
  doesNameExist: boolean = true;
  isEmailGood:boolean = true;


  constructor(public userService: UserService) { }

  ngOnInit(){
    let emailInput = document.getElementById("email-reg");
    let nameInput = document.getElementById("name-reg");

    this.checkEmail(emailInput);
    this.checkName(nameInput);
  }

  checkEmail(email){
    let errorLabel = document.getElementById("error-reg");
    fromEvent(email, "input").pipe(
      map(ev => ev['target'].value)
    ).subscribe(mail =>{
      console.log(mail);
      if (mail.indexOf("@") === -1 || (mail.indexOf(".com") === -1 && mail.indexOf(".rs") === -1))
        this.isEmailGood = false;
      else
        this.isEmailGood = true;
      this.setErrorLabel(errorLabel);
    });
  }

  setErrorLabel(errorLabel){
    let errorMessage = "";
    
    if(!this.isEmailGood){  
      errorMessage = "Email must contain @ and .com or .rs";
    }
    else if(!this.isNameGood){
      errorMessage = "Name may conatain only letters, numbers, _ and .";
    }

    (errorLabel as HTMLLabelElement).innerHTML = errorMessage;
  }

  checkName(name){
    let errorLabel = document.getElementById("error");
    fromEvent(name, "input").pipe(
      map(ev => ev['target'].value)
    ).subscribe(username =>{
      console.log(username);
      if (this.validateName(username))
        this.isNameGood = false;
      else
        this.isNameGood = true;
      this.setErrorLabel(errorLabel);
  })
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
    const email:string = (document.getElementById("email-reg") as HTMLInputElement).value;
    const name:string = (document.getElementById("name-reg") as HTMLInputElement).value;
    const password: string = (document.getElementById("password-reg") as HTMLInputElement).value;
    let errorLabel = document.getElementById("error-reg");

    if(!email || !password || !name)
      alert("All fields must be filled");
    
    let request =  new RegisterRequest(name, password, email);


    this.userService.register(request).subscribe(response =>{
      console.log(response);
    }
    )
  }

}
