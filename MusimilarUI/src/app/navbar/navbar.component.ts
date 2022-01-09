import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Role, User } from '@app/_models';
import { AuthenticationService } from '@app/_services';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  
  user: User;
  
  constructor(private authenticationService: AuthenticationService,
              private router: Router) 
            {
              this.authenticationService.user.subscribe(x => this.user = x);
            }

  ngOnInit(): void {}

  get isAdmin() {
    return this.user && this.user.role === Role.Admin;
  }

  get isLogin(){
      return localStorage.getItem('user') === null ? false : true;     
  }

  onSearch(artist: HTMLInputElement){
      if(artist.value)
          this.router.navigate(["/artist/similar/", artist.value]);
  }

  logout() {
      this.authenticationService.logout();
  }

  handleClick() {
    this.router.navigate(["/"]);
  }
}
