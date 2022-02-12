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
        //return this.http.get(`${environment.apiUrl}/user/playlist/feed`, {params:request.subscriptions});
    }
}