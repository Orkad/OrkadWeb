using AutoMapper;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Shares.ReadModels;
using System.Linq;

namespace OrkadWeb.Services.DTO.Shares
{
    public class ShareProfile : Profile
    {
        public ShareProfile()
        {
            CreateMap<Share, ShareItem>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.Owner.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.UserCount, o => o.MapFrom(s => s.UserShares.Count()));
        }

    }
}
