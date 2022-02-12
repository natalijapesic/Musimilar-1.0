import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Playlist, User } from '@app/_models';
import { GetPlaylistFeed } from '@app/_requests';
import { AuthenticationService, UserService } from '@app/_services';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-playlist-feed',
  templateUrl: './playlist-feed.component.html',
  styleUrls: ['./playlist-feed.component.css']
})
export class PlaylistFeedComponent implements OnInit {

  user: User;
  public feed = new BehaviorSubject<Playlist[]>([]);
  
  constructor(public authenticationService: AuthenticationService,
              public userService: UserService, 
              public router: Router) {

              this.user = this.authenticationService.userValue;
  }

  ngOnInit(): void {

    this.generateFeed();
  }

  generateFeed(){
    this.feed = new BehaviorSubject<any[]>([]);
    let request = new GetPlaylistFeed(this.user.subscriptions);
    this.userService.getPlaylistFeed(request).subscribe({
      next:(data) => {
        console.log(data);
        if(data[0] == null)
          alert("There is still no new playlists");
        this.feed.next(data);
        console.log(this.feed)
      },
      error:(e) => alert("Do you follow any of the genres?")
    })
  }

}
