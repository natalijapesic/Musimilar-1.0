
using AutoMapper;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.Requests;
using MusimilarApi.Models.Responses;

namespace MusimilarApi.Helpers
{
    public class OrganizationProfile: Profile
    {
        public OrganizationProfile(){

            CreateMap<AudioFeaturesRequest, AudioFeatures>();
            CreateMap<SongRequest, Song>();
            CreateMap<Song, SongResponse>();
            CreateMap<SongInfo, SongInfoResponse>();
            CreateMap<Song, SongInfo>();

            CreateMap<RegisterRequest, User>();
            CreateMap<User, UserResponse>();

        }

    }
}