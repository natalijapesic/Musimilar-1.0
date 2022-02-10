namespace MusimilarApi.Entities.MongoDB
{
    public static class SongGenre
    {
        public const string Edm = "edm";
        public const string Pop = "pop";
        public const string Rock = "rock";
        public const string Rap = "rap";
        public const string Rb = "rb";
        public const string Latin = "latin";


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









