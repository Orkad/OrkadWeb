using AutoMapper;
using OrkadWeb.Models;

namespace OrkadWeb.Services.DTO.Refunds
{
    public class RefundProfile : Profile
    {
        public RefundProfile()
        {
            CreateMap<Refund, RefundItem>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.EmitterId, o => o.MapFrom(s => s.Emitter.User.Id))
                .ForMember(d => d.EmitterName, o => o.MapFrom(s => s.Emitter.User.Username))
                .ForMember(d => d.ReceiverId, o => o.MapFrom(s => s.Receiver.User.Id))
                .ForMember(d => d.ReceiverName, o => o.MapFrom(s => s.Receiver.User.Username))
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date));
        }
    }
}
