import { Component, OnInit } from '@angular/core';
import { Song, User } from '@app/_models';
import { PlaylistRequest } from '@app/_requests';
import { SongResponse } from '@app/_responses';
import { AuthenticationService, SongService } from '@app/_services';

@Component({
  selector: 'app-similar-songs',
  templateUrl: './similar-songs.component.html',
  styleUrls: ['./similar-songs.component.css']
})
export class SimilarSongsComponent implements OnInit {

  user: User;
  songList:SongResponse[];
  example: string;

  constructor(public songService: SongService,
    public authenticationService: AuthenticationService) 
    { 
      this.user = this.authenticationService.userValue;
    }

  ngOnInit(): void {}

  
  onRecommend(songName: HTMLInputElement, artistName: HTMLInputElement){
    let request = new PlaylistRequest(songName.value, artistName.value);

    this.songService.recommendPlaylist(request).subscribe({
      next: (data)=> {
        if(!data[0]) 
          alert("DB doesnt contain similar songs");
        else 
          this.songList = data;
      },
      error: (e) => alert("This song doesnt exist") });
    }

}
