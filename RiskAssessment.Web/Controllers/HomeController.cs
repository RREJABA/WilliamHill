using Microsoft.VisualBasic.FileIO;
using RiskAssessment.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RiskAssessment.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string settledFilePath = Server.MapPath("~/Content/Files/Settled.csv");
            string unsettledFilePath = Server.MapPath("~/Content/Files/Unsettled.csv");
            char seperator = ',';
            decimal AverageStake = 0;
            decimal NumberOfBetsMade = 0;
            decimal NumberOfTimesWon = 0;
            bool IsUnusualRateWinner = false;
            bool IsCurrentStakeTenTimesHigherThanAverage = false;
            bool IsCurrentStakeThirtyTimesHigherThanAverage = false;
            bool IsCurrentWinningOnRiskAmount = false;

            IEnumerable<CustomerViewModel> listSettledData = new List<CustomerViewModel>();
            IEnumerable<CustomerViewModel> listUnsettledData = new List<CustomerViewModel>();

            listSettledData= ParseCSV(settledFilePath, "" + seperator);
            listUnsettledData = ParseCSV(unsettledFilePath, "" + seperator);

            List<string> CustomerListSettled = listSettledData.Select(c => c.Customer).Distinct().ToList();

            foreach(string cust in CustomerListSettled)
            {
                AverageStake = 0;
                NumberOfBetsMade = 0;
                NumberOfTimesWon = 0;
                IsUnusualRateWinner = false;

                NumberOfBetsMade = listSettledData.Where(c => c.Customer == cust).Count();
                NumberOfTimesWon = listSettledData.Where(c => c.Customer == cust && c.Win > 0).Count();
                AverageStake = (listSettledData.Where(c => c.Customer == cust).Sum(c => c.Stake) / NumberOfBetsMade);
                IsUnusualRateWinner = ((NumberOfTimesWon / NumberOfBetsMade) * 100) > 60;

                foreach (CustomerViewModel custU in listUnsettledData.Where(c => c.Customer == cust))
                {
                    IsCurrentWinningOnRiskAmount = false;
                    IsCurrentStakeTenTimesHigherThanAverage = false;
                    IsCurrentStakeThirtyTimesHigherThanAverage = false;

                    IsCurrentWinningOnRiskAmount = custU.Win >= 1000;
                    IsCurrentStakeTenTimesHigherThanAverage = custU.Stake > (AverageStake * 10);
                    IsCurrentStakeThirtyTimesHigherThanAverage = custU.Stake > (AverageStake * 30);

                    custU.NumberOfBetsMade = NumberOfBetsMade;
                    custU.NumberOfTimesWon = NumberOfTimesWon;
                    custU.AverageStake = AverageStake;
                    custU.IsUnusualRateWinner = IsUnusualRateWinner;
                    custU.IsCurrentWinningOnRiskAmount = IsCurrentWinningOnRiskAmount;
                    custU.IsCurrentStakeTenTimesHigherThanAverage = IsCurrentStakeTenTimesHigherThanAverage;
                    custU.IsCurrentStakeThirtyTimesHigherThanAverage = IsCurrentStakeThirtyTimesHigherThanAverage;
                }
            }
            
            return View(listUnsettledData);
        }

        public IEnumerable<CustomerViewModel> ParseCSV(string fileSrc, string seperator)
        {   
            var list = new List<CustomerViewModel>();
            bool header = true;

            using (TextFieldParser parser = new TextFieldParser(fileSrc))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                //specify the delimiter 
                parser.SetDelimiters(seperator);
                while (!parser.EndOfData)
                {
                    //read Fileds 
                    string[] fields = parser.ReadFields();
                    //Processing row
                    if (header == true)
                    {  //escape first line
                        header = false;
                    }
                    else
                    {
                        CustomerViewModel customer = new CustomerViewModel();
                        customer.Customer = fields[0];
                        customer.Event = fields[1];
                        customer.Participant = fields[2];
                        customer.Stake = decimal.Parse(fields[3]);
                        customer.Win = decimal.Parse(fields[4]);
                        list.Add(customer);
                    }
                }
            }
            return list.AsEnumerable();
        }
    }
}