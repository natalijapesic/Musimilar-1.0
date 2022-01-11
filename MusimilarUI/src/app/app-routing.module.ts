import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { AuthGuard } from './_helpers';
import { Role } from './_models';
import { RegisterComponent } from './register';
import { AdminComponent } from './admin';
import { ArtistListComponent } from './artist-list';
import { PlaylistComponent } from './playlist';

const routes: Routes = [
    {
        path: '',
        component: HomeComponent
    },
    {
        path: 'artist/similar/:artist_name',
        component: ArtistListComponent
    },
    {
        path: 'admin',
        component: AdminComponent,
        canActivate: [AuthGuard],
        data: { roles: [Role.Admin] }
    },
    {
        path: 'playlist',
        component: PlaylistComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: RegisterComponent
    },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }