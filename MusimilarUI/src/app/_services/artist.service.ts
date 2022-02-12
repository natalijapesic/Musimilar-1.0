import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { map, Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArtistService {

  constructor(private http: HttpClient) { }

  getSimilarArtists(artistRequest: string):Observable<any>{
    return this.http.get<any>(`${environment.apiUrl}/artist/similar/${artistRequest}`);
  }

  addMany(artists: JSON){
    console.log(artists);
  }
}
