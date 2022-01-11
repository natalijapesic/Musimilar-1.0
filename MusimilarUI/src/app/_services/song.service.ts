import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PlaylistRequest } from '@app/_requests';
import { SongResponse } from '@app/_responses';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class SongService {

  constructor(private http: HttpClient) { }

  addMany(songs: JSON){
    return this.http.post(`${environment.apiUrl}/song/many`, songs);
  }

  recommendPlaylist(request: PlaylistRequest):Observable<SongResponse[]>{
    return this.http.post<SongResponse[]>(`${environment.apiUrl}/song/playlist`, request);
  }
}
