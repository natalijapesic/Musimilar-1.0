import { Role } from ".";
import { Playlist } from "./playlist.model";

export class User {

    constructor(public id: string,
                public name: string,
                public email: string,
                public role: Role,
                public subscriptions: string[],
                public playlists: Playlist[],
                public password?: string,
                public token?: string){}

}
