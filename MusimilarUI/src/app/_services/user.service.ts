import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environments/environment';
import { Playlist, User } from '@app/_models';
import { AddPlaylistRequest, DeletePlaylistRequest, GetPlaylistFeed, RegisterRequest } from '@app/_requests';

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
        return this.http.get<Playlist[]>(`${environment.apiUrl}/user/playlist/feed?${url}`);
    }

    createURL(request: GetPlaylistFeed):string{

        let url:string ='?';
        for(let genre in request.subscriptions){

            url.concat(`Subscriptions=${genre}&`);
        }
        url.slice(request.subscriptions.length-1, 1)
        console.log(url);
        return url;
    }
}