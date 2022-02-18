import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environments/environment';
import { Playlist, User } from '@app/_models';
import { AddPlaylistRequest, AddSubscriptionsRequest, DeletePlaylistRequest, GetPlaylistFeed, RegisterRequest } from '@app/_requests';

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(`${environment.apiUrl}/user`);
    }

    getById(id: number) {
        return this.http.get<User>(`${environment.apiUrl}/user/${id}`);
    }

    register(request: RegisterRequest){
        return this.http.post(`${environment.apiUrl}/user/registration`, request);
    }

    addPlaylist(request: AddPlaylistRequest){
        return this.http.put<Playlist>(`${environment.apiUrl}/user/add/playlist`, request);
    }

    deletePlaylist(request: DeletePlaylistRequest){
        return this.http.put<Playlist>(`${environment.apiUrl}/user/delete/playlist`, request);
    }

    getPlaylistFeed(request: GetPlaylistFeed){
        let url:string = this.createURL(request);
        return this.http.get<Playlist[]>(`${environment.apiUrl}/user/playlist/feed${url}`);
    }

    addSubscriptions(request: AddSubscriptionsRequest){
        return this.http.put(`${environment.apiUrl}/user/subscriptions`, {request});
    }

    createURL(request: GetPlaylistFeed):string{

        let url:string ='?';
        request.subscriptions.forEach(genre =>{
            url+=`Subscriptions=${genre}&`;
        })

        console.log("Natalija")
        url.substring(0, url.length-1)
        console.log(url);
        return url;
    }
}