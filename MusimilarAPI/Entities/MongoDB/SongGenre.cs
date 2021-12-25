namespace MusimilarApi.Entities.MongoDB
{
    public static class SongGenre
    {
        public const string Edm = "Edm";
        public const string Pop = "Pop";
        public const string Rock = "Rock";
        public const string Rap = "Rap";
        public const string Rb = "Rb";
        public const string Latin = "Latin";


        public static bool Validate(string genre){

            if( 
                genre == Edm    || 
                genre == Pop    || 
                genre == Rock   || 
                genre == Rap    || 
                genre == Rb     ||  genre == Latin
            )

                return true;
            else
                return false;

        }
    }
}









