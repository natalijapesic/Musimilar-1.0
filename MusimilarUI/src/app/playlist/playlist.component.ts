import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Song, User } from '@app/_models';
import { AddPlaylistRequest, PlaylistRequest } from '@app/_requests';
import { SongResponse } from '@app/_responses';
import { AuthenticationService, SongService, UserService } from '@app/_services';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent implements OnInit {

  user: User;
  @Input() songList:SongResponse[];
  example: Song;

  constructor(public songService: SongService,
              public authenticationService: AuthenticationService,
              public userService: UserService, 
              public router: Router) 
              { 
                this.authenticationService.user.subscribe(x => this.user = x);
              }

  ngOnInit(): void {}


  onSave(playlistName: HTMLInputElement){
    let request = new AddPlaylistRequest(this.user.id, playlistName.value, this.example, this.songList)
    this.userService.addPlaylist(request).subscribe({
      next:(v) => this.router.navigate(["/user-profile"]),
      error:(e) => alert("This playlist is already saved")
    })
  }

}
