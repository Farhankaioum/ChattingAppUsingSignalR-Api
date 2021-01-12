using AutoMapper;
using ChattingApp.API.Dtos;
using ChattingApp.Foundation.Entities;

namespace ChattingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();

            CreateMap<MessageToReturnDto, Message>()
                .ReverseMap();
            CreateMap<Message, MessageForCreationDto>();
        }
    }
}
