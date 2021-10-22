using System;

namespace BlueDream
{
    public class FundingLedger
    {
        public Funding Funding { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionOn { get; set; }

        public DepositOrWithdrawal DepositOrWithdrawal { get; set; }

        public Advance ForAdvance { get; set; }

        public decimal FundingBalanceBeforeTransaction { get; set; }

        public AnonymousDonation FromDonation { get; set; }

        public PatronagePayment FromPatronage { get; set; }

        public RevenueLedger FromRevenue { get; set; }
    }
}