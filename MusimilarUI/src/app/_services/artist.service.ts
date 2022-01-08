import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Artist } from '@app/_models';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ArtistService {

  constructor(private http: HttpClient) { }

  getSimilarArtists(artistRequest: string){
    return this.http.get<Artist[]>(`${environment.apiUrl}/artist/similar/${artistRequest}`)
  }
}
