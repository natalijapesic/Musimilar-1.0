import { Component, OnInit } from '@angular/core';
import { ArtistService } from '@app/_services/artist.service';
import { SongService } from '@app/_services/song.service';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css']
})
export class AddComponent implements OnInit {

  public entities:string[] = ["Artist", "Song"];
  private fileToLoad: any;

  constructor(private songService: SongService,
              private artistService: ArtistService) {}

  ngOnInit(): void {
  }

  onFileChange(event){
    this.fileToLoad = event.target.files[0];
    
  }



  onClick(selectEntity: HTMLSelectElement){
    console.log(selectEntity.value);
    
    const fileReader = new FileReader();
    fileReader.onload = (fileLoadedEvent) => {
      const textFromFileLoaded =(fileLoadedEvent.target.result as string);
      const json = JSON.parse(textFromFileLoaded);
      if(selectEntity.value === this.entities[0])
        this.artistService.addMany(json);
      else
        this.songService.addMany(json).subscribe(response =>{
          console.log(response);
        })
    };
    fileReader.readAsText(this.fileToLoad, "UTF-8");


  }

}
