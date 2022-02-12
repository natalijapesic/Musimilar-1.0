import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '@app/_models';
import { DeletePlaylistRequest } from '@app/_requests';
import { AuthenticationService, UserService } from '@app/_services';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  user: User;
  
  constructor(public authenticationService: AuthenticationService,
              public userService: UserService, 
              public router: Router) {

              this.user = this.authenticationService.userValue;
   }

  ngOnInit(): void {
  }

  onDelete(playlistName: string){
    let request = new DeletePlaylistRequest(this.user.id, playlistName)
    this.userService.deletePlaylist(request).subscribe({
      next:(v) => {
        this.user.playlists.forEach((value, index) => {
          if(value.name.toLowerCase() == playlistName.toLowerCase()) 
            this.user.playlists.splice(index, 1);
        })
      },
      error:(e) => alert("Error")
    })
  }

}

