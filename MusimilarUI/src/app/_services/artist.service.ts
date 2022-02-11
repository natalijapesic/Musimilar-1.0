import { HttpClient, HttpErrorResponse} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Artist } from '@app/_models';
import { environment } from '@environments/environment';
import { catchError, map, Observable, throwError} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArtistService {

  constructor(private http: HttpClient) { }

  getSimilarArtists(artistRequest: string):Observable<any>{
    return this.http.get<any>(`${environment.apiUrl}/artist/similar/${artistRequest}`);
  }

  handleError(error: HttpErrorResponse) {
    return throwError(error);
  }
  addMany(artists: JSON){
    console.log(artists);
  }
}
