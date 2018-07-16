using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SolartisTravelApiLogicAppWrapper.Controllers
{
    public class LUISModels
    {
        public class TopScoringIntent
        {
            public string intent { get; set; }
            public double score { get; set; }
        }

        public class Intent
        {
            public string intent { get; set; }
            public double score { get; set; }
        }

        public class Entity
        {
            public string entity { get; set; }
            public string type { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public double score { get; set; }
        }

        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public List<Intent> intents { get; set; }
        public List<Entity> entities { get; set; }
        public string luisPrediciton { get; set; }
        public string desiredIntent { get; set; }
        public bool isDesiredIntent { get; set; }

    }


    public class SolartisModels
    {
        public class TravelerList
        {
            public string TravelCost { get; set; }
            public string TravelerDOB { get; set; }
            public string TravelerBasePremium { get; set; }
            public string TravelerGrossPremium { get; set; }
        }

        public class PremiumInformations
        {
            public string TotalBasePremium { get; set; }
            public string TripCancellationCoverage { get; set; }
            public List<TravelerList> TravelerList { get; set; }
            public string PlanCode { get; set; }
            public string PlanName { get; set; }
            public string TotalGrossPremium { get; set; }
            public string TotalGrossPremium_IM { get; set; }
            public string ServiceFee { get; set; }
            public string CommissionAmt { get; set; }
            public string TotalOptionalCoverage { get; set; }
        }

        public class CoverageInformations
        {
            public string CoverageName { get; set; }
            public string CoverageLimit { get; set; }
            public string IsOptionalCoverage { get; set; }
        }

        public string RequestStatus { get; set; }
        public List<object> RuleInformationList { get; set; }
        public PremiumInformations PremiumInformation { get; set; }
        public List<CoverageInformations> CoverageInformation { get; set; }

    }

    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Route("SolartisTravelParser")]
        [HttpPost]
        public string SolartisTravelParser([FromQuery]string user, [FromBody] SolartisModels incModel)
        {
            try
            {

                return string.IsNullOrWhiteSpace(incModel?.PremiumInformation?.TotalGrossPremium) ?
                    $"{user}, Oh Dear...I was not able to get you a rate. Generated {DateTime.UtcNow}" :
                    $"{user}, Get a rate as low as {incModel?.PremiumInformation?.TotalGrossPremium} including Trip Cancellation! Quote generated {DateTime.UtcNow}. #solartishack";
            }
            catch (Exception e)
            {
                return $"Was not able to get you that rate...Error: {e.Message}";
            }
        }


        [Route("LUISExtractOrigin")]
        [HttpPost]
        public string LUISExtractOrigin([FromBody] LUISModels incModel)
        {
            try
            {
                return incModel.entities.FirstOrDefault(x => x.type == "Location:Origin")?.entity?.ToUpper() ?? string.Empty;
            }
            catch (Exception e)
            {
                return $"Was not able to extract Origin...Error: {e.Message}";
            }
        }

        [Route("LUISExtractDestination")]
        [HttpPost]
        public string LUISExtractDestination([FromBody] LUISModels incModel)
        {
            try
            {
                return incModel.entities.FirstOrDefault(x => x.type == "Location:Destination")?.entity.ToUpper() ?? string.Empty;
            }
            catch (Exception e)
            {
                return $"Was not able to extract Destination...Error: {e.Message}";
            }
        }


    }
}
