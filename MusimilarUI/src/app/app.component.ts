import { Component } from '@angular/core';

import { AuthenticationService } from './_services';
import { User, Role } from './_models';
import { ArtistService } from './_services/artist.service';
import { Router } from '@angular/router';

@Component({ selector: 'app', templateUrl: 'app.component.html' })
export class AppComponent {
    user: User;

    constructor(private authenticationService: AuthenticationService,
                private router: Router
                ) {
        this.authenticationService.user.subscribe(x => this.user = x);
    }

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
}