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
  public artistList = new BehaviorSubject<Artist[]>([]); //pocetna vrednost je prazan niz

  constructor(private artistService: ArtistService,
              private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.getSimilarArtist();

  }

  getSimilarArtist(){
    this.activeRoute.params.subscribe(routeParams => {
      this.searchValue = routeParams['artist_name'];
      
      if(this.searchValue)
        this.artistService.getSimilarArtists(this.searchValue).subscribe(response => {
          if(!response[0])
            this.finished = true;
          else{

            this.finished = false;
            this.artistList.next([...this.artistList.getValue(), ...response]);
            //konkatenira liste, postojecu sa reponse
          }
        })
    });
  }



}
