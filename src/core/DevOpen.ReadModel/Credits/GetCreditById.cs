using DevOpen.Domain.Model.Credits;
using DevOpen.Framework.Application.ReadModels;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.ReadModel.Credits
{
    public class GetCreditById : Query<CreditViewModel>
    {
        public CreditId CreditId { get; }

        public GetCreditById(CreditId creditId)
        {
            CreditId = creditId;
        }
    }
}