using AutoMapper;
using OrkadWeb.Models;

namespace OrkadWeb.Services.DTO.Expenses
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Expense, ExpenseItem>()
                .ForMember(m => m.Id, o => o.MapFrom(e => e.Id))
                .ForMember(m => m.Name, o => o.MapFrom(e => e.Name))
                .ForMember(m => m.Amount, o => o.MapFrom(e => e.Amount))
                .ForMember(m => m.Date, o => o.MapFrom(e => e.Date))
                .ReverseMap()
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<ExpenseCreation, Expense>()
                .ForMember(m => m.Name, o => o.MapFrom(e => e.Name))
                .ForMember(m => m.Amount, o => o.MapFrom(e => e.Amount))
                .ForAllOtherMembers(o => o.Ignore());
        }
    }
}