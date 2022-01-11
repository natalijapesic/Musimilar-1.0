import { Component, OnInit } from '@angular/core';
import { Playlist, Song, User } from '@app/_models';
import { AddPlaylistRequest, PlaylistRequest } from '@app/_requests';
import { SongResponse } from '@app/_responses';
import { AuthenticationService, SongService, UserService } from '@app/_services';
import { Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent implements OnInit {

  user: User;
  songList:SongResponse[];
  example: Song;

  constructor(public songService: SongService,
              public authenticationService: AuthenticationService,
              public userService: UserService) 
              { 
                this.authenticationService.user.subscribe(x => this.user = x);
              }

  ngOnInit(): void {
  }

  onRecommend(songName: HTMLInputElement, artistName: HTMLInputElement){
    this.example = new Song(songName.value, artistName.value);
    let request = new PlaylistRequest(songName.value, artistName.value);

    this.songService.recommendPlaylist(request).subscribe(data =>{
      if(!data[0])
        alert("DB doesnt contain similar songs");
      else{
        this.songList = data;
      }
    });
  }

  onSave(playlistName: HTMLInputElement){
    let request = new AddPlaylistRequest(this.user.id, playlistName.value, this.example, this.songList)
    this.userService.addPlaylist(request).subscribe(response =>{
       console.log(response);
    })
        
  }

}