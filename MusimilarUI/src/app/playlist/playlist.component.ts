import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Song, User } from '@app/_models';
import { AddPlaylistRequest, DeletePlaylistRequest, PlaylistRequest } from '@app/_requests';
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
  @Input() example: string;

  constructor(public songService: SongService,
              public authenticationService: AuthenticationService,
              public userService: UserService, 
              public router: Router) 
              { 
                this.user = this.authenticationService.userValue;
              }

  ngOnInit(): void {}


  onSave(playlistName: HTMLInputElement){
    let request = new AddPlaylistRequest(this.user.id, playlistName.value, this.example, this.songList)

    this.userService.addPlaylist(request).subscribe({
      next:(v) => {
        this.user.playlists.push(v)
          this.router.navigate(["/user-profile"]);
      },
      error:(e) => alert("This playlist is already saved")
    })
  }
}
