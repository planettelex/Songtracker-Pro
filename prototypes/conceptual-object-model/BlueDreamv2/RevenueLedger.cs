using System;

namespace BlueDream
{
    public class RevenueLedger
    {
        public Revenue Revenue { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionOn { get; set; }

        public DepositOrWithdrawal DepositOrWithdrawal { get; set; }

        public SubsidaryEarnings FromSubsidaryEarnings { get; set; }

        public FundingLedger ForFunding { get; set; }

        public Expense ForExpense { get; set; }
    }
}