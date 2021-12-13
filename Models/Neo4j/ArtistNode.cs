using System.Collections.Generic;

public class ArtistNode
{
    public string Name { get; set; }
    public List<string> Genres { get; set; }
    public string Image { get; set; }

    public ArtistNode(string name, List<string> genres, string image){

        this.Name = name;
        this.Genres = genres;
        this.Image = image;

    }
}