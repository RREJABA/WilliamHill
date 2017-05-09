namespace RiskAssessment.Web.Models
{
    public class CustomerViewModel
    {
        public string Customer { get; set; }
        public string Event { get; set; }
        public string Participant { get; set; }
        public decimal Stake { get; set; }
        public decimal Win { get; set; }
        public decimal AverageStake { get; set; }
        public decimal NumberOfBetsMade { get; set; }
        public decimal NumberOfTimesWon { get; set; }
        public bool IsUnusualRateWinner { get; set; }
        public bool IsCurrentStakeTenTimesHigherThanAverage { get; set; }
        public bool IsCurrentStakeThirtyTimesHigherThanAverage { get; set; }
        public bool IsCurrentWinningOnRiskAmount { get; set; }
    }
}