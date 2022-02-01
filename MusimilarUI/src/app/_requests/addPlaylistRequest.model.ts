import { Song } from "@app/_models";


export class AddPlaylistRequest {
    
    constructor(public userId: string,
                public name: string,
                public example: Song,
                public songs: Song[]){}

}