import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { Role, User } from '@app/_models';
import { AuthenticationService, UserService } from '@app/_services';

@Component({ templateUrl: 'admin.component.html' })
export class AdminComponent implements OnInit {
    loading = false;
    users: User[] = [];    
    user: User;

    constructor(private authenticationService: AuthenticationService,
                private userService:UserService) 
    {
        this.authenticationService.user.subscribe(x => this.user = x);
    }

    get isAdmin() {
        return this.user && this.user.role === Role.Admin;
    }


    ngOnInit() {
        this.loading = true;
        this.userService.getAll().pipe(first()).subscribe(users => {
            this.loading = false;
            this.users = users;
        });
    }
}