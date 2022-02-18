import { Song } from "@app/_models";


export class AddPlaylistRequest {
    
    constructor(public userId: string,
                public name: string,
                public example: string,
                public songs: Song[]){}

}