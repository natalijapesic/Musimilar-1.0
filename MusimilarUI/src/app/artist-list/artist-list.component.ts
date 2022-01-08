import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ArtistService } from '@app/_services/artist.service';

@Component({
  selector: 'app-artist-list',
  templateUrl: './artist-list.component.html',
  styleUrls: ['./artist-list.component.css']
})
export class ArtistListComponent implements OnInit {

  private searchValue: string = '';

  constructor(private artistService: ArtistService,
              private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.getSimilarArtis();

  }

  getSimilarArtis(){

    this.activeRoute.params.subscribe(routeParams => {
      this.searchValue = routeParams['artist_name'];
      
      if(this.searchValue)
        this.artistService.getSimilarArtists(this.searchValue).subscribe(response => {
          console.log(response);
        })

    })
  }



}
