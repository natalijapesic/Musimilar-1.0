import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Artist } from '@app/_models';
import { ArtistService } from '@app/_services/artist.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-artist-list',
  templateUrl: './artist-list.component.html',
  styleUrls: ['./artist-list.component.css']
})
export class ArtistListComponent implements OnInit {

  private searchValue: string = '';
  public finished: boolean = false;
  public artistList = new BehaviorSubject<Artist[]>([]);

  constructor(private artistService: ArtistService,
              private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.getSimilarArtist();

  }

  getSimilarArtist(){
    this.artistList=new BehaviorSubject<any[]>([]);
    this.activeRoute.params.subscribe(routeParams => {
      this.searchValue = routeParams['artist_name'];
      
      if(this.searchValue)
        this.artistService.getSimilarArtists(this.searchValue).subscribe(
          response => {
            if(response[0] == null)
              alert("Similar artists dont exist. Try someone else.")
            this.artistList.next(response);
          },
          (error: HttpErrorResponse) =>{
            alert("Artist doesnt exist.")
          }
        )
    });
  }



}
