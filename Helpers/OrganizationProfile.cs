
using AutoMapper;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.DTOs;
using MusimilarApi.Models.Requests;
using MusimilarApi.Models.Responses;

namespace MusimilarApi.Helpers
{
    public class OrganizationProfile: Profile
    {
        public OrganizationProfile(){

            CreateMap<AudioFeaturesRequest, AudioFeaturesDTO>();

            CreateMap<SongRequest, SongDTO>();
            CreateMap<SongDTO, SongResponse>();
            
            CreateMap<SongInfo, SongInfoDTO>();
            CreateMap<SongInfoDTO, SongInfo>();

            CreateMap<SongDTO, SongInfoDTO>();

            CreateMap<Song, SongDTO>();
            CreateMap<AudioFeatures, AudioFeaturesDTO>();

            CreateMap<SongDTO, Song>();
            CreateMap<AudioFeaturesDTO, AudioFeatures>();


            CreateMap<RegisterRequest, UserDTO>();
            CreateMap<UserDTO, UserResponse>();

            CreateMap<PlaylistRequest, PlaylistDTO>();
            
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();
            

        }

    }
}