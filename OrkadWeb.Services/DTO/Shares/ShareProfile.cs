using AutoMapper;
using OrkadWeb.Models;
using System.Linq;

namespace OrkadWeb.Services.DTO.Shares
{
    public class ShareProfile : Profile
    {
        public ShareProfile()
        {
            CreateMap<UserShare, UserShareDetail>()
                .ForMember(usd => usd.Id, o => o.MapFrom(us => us.User.Id))
                .ForMember(usd => usd.Name, o => o.MapFrom(us => us.User.Username))
                .ForMember(usd => usd.Expenses, o => o.MapFrom(us => us.Expenses))
                .ForMember(usd => usd.Refunds, o => o.MapFrom(us => us.EmittedRefunds.Union(us.ReceivedRefunds)));

            CreateMap<ShareCreation, Share>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

            CreateMap<Share, ShareDetail>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Users, o => o.MapFrom(s => s.UserShares))
                .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.Owner.Id));

            CreateMap<Share, ShareItem>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.AttendeeCount, o => o.MapFrom(s => s.UserShares.Count()));
        }

    }
}
