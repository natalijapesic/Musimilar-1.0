import { Component } from '@angular/core';

import { AuthenticationService } from './_services';
import { User, Role } from './_models';

@Component({ selector: 'app', templateUrl: 'app.component.html' })
export class AppComponent {
    user: User;

    constructor(private authenticationService: AuthenticationService) {
        this.authenticationService.user.subscribe(x => this.user = x);
    }

    get isAdmin() {
        return this.user && this.user.role === Role.Admin;
    }

    get isLogin(){
        return localStorage.getItem('user') === null ? false : true;     
    }

    logout() {
        this.authenticationService.logout();
    }
}