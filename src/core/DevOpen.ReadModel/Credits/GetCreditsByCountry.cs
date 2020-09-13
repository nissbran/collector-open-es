using System.Collections.Generic;
using DevOpen.Domain.Model;
using DevOpen.Framework.Application.ReadModels;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.ReadModel.Credits
{
    public class GetCreditsByCountry : Query<List<CreditViewModel>>
    {
        public Country Country { get; }

        public GetCreditsByCountry(Country country)
        {
            Country = country;
        }
    }
}