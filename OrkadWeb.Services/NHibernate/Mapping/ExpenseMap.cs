using FluentNHibernate.Mapping;
using OrkadWeb.Models;

namespace OrkadWeb.Services.NHibernate.Mapping
{
    public class ExpenseMap : ClassMap<Expense>
    {
        public ExpenseMap()
        {
            Table("expense");
            Id(x => x.Id, "id");
            Map(x => x.Amount, "amount");
            Map(x => x.Name, "name");
            Map(x => x.Date, "date");
            References(x => x.UserShare, "user_share_id");
        }
    }
}
