import { Component, OnInit, QueryList } from '@angular/core';
import { Router } from '@angular/router';
import { Playlist, User } from '@app/_models';
import { AddSubscriptionsRequest, GetPlaylistFeed } from '@app/_requests';
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
  public genres: string[] = ["rap", "pop", "rock", "rnb", "edm", "latin"];
  
  constructor(public authenticationService: AuthenticationService,
              public userService: UserService, 
              public router: Router) {

              this.user = this.authenticationService.userValue;
  }

  ngOnInit(): void {
    this.generateFeed();
  }

  checkValue(checkbox: HTMLInputElement){
    
    if(checkbox.checked)
      this.user.subscriptions.push(checkbox.value);
    else
    {
      this.user.subscriptions.forEach((value, index) => {
        if(value == checkbox.value) 
          this.user.subscriptions.splice(index, 1);
      })
    }

  }

  onSaveChanges(){
    let addSubscriptions = new AddSubscriptionsRequest(this.user.id, this.user.subscriptions);
    this.userService.addSubscriptions(addSubscriptions);
    this.generateFeed();

  }

  generateFeed(){
    this.feed = new BehaviorSubject<any[]>([]);
    let request = new GetPlaylistFeed(this.user.subscriptions);
    console.log(this.feed)
    if(this.user.subscriptions.length > 0)
      this.userService.getPlaylistFeed(request).subscribe({
        next:(data) => {
          if(data[0] == null)
            alert("There is still no new playlists");
          this.feed.next(data);
        },
        error:(e) => alert("Do you follow any of the genres?")
      })
  }

}
