
using AutoMapper;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.Requests;
using MusimilarApi.Models.Responses;

namespace MusimilarApi.Helpers
{
    public class OrganizationProfile: Profile
    {
        public OrganizationProfile(){

            // Request -> DTO
            CreateMap<SongRequest, Song>();

            // DTO -> Song
            CreateMap<Song, SongResponse>();

            CreateMap<RegisterRequest, User>();

            CreateMap<User, UserResponse>();

        }

    }
}