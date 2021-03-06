
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
            CreateMap<SongInfoDTO, SongInfoResponse>();

            CreateMap<SongDTO, SongInfoDTO>();

            CreateMap<Song, SongDTO>();
            CreateMap<AudioFeatures, AudioFeaturesDTO>();

            CreateMap<SongDTO, Song>();
            CreateMap<AudioFeaturesDTO, AudioFeatures>();


            CreateMap<RegisterRequest, UserDTO>();
            CreateMap<UserDTO, UserResponse>();

            CreateMap<SongInfoRequest, SongInfoDTO>();

            CreateMap<PlaylistRequest, PlaylistDTO>();
            CreateMap<AddPlaylistRequest, PlaylistDTO>();
            CreateMap<PlaylistDTO, Playlist>();
            CreateMap<Playlist, PlaylistDTO>();
            CreateMap<PlaylistDTO, PlaylistResponse>();
            CreateMap<PlaylistFeedDTO, PlaylistFeedResponse>();
            
            CreateMap<UserDTO, User>().ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
            CreateMap<User, UserDTO>().ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));

            

        }

    }
}