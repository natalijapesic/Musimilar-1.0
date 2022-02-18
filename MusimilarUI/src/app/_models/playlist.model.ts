import { Song } from ".";

export class Playlist{

    constructor(public name:string, 
                public songs: Song[],
                public example?: string){}
}
