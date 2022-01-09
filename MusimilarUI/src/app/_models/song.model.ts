export class Song{

    constructor(public name: string,
                public artist: string,
                public audioFeatures: AudioFeautres){}
    
}

export class AudioFeautres{

    constructor(public tempo: number,
                public energy: number,
                public speechiness: number,
                public danceability: number,
                public durationMS: number,
                public valence: number){}

}