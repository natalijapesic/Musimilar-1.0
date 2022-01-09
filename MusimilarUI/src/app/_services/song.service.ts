import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})

export class SongService {

  constructor(private http: HttpClient) { }

  addMany(songs: JSON){
    return this.http.post(`${environment.apiUrl}/song/many`, songs);
  }
}
