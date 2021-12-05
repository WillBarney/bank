using System;

namespace Bank
{
    class Account
    {
        public int accountNumber;
        public string accountType;
        public decimal apy;
        public decimal balance;


        public Account()
        {
            accountType = "Checking";
            apy = DetermineAPY(accountType);
            balance = 0.00m;
        }
        public Account(int accountNumber,string accountType,decimal balance)
        {
            this.accountNumber = accountNumber;
            this.accountType = accountType;
            this.apy = DetermineAPY(accountType);
            this.balance = 0.00m;
        }

        private static decimal DetermineAPY(string accountType)
        {
            decimal apy;
            switch(accountType)
            {
                case "checking":
                    apy = 0.0001m;
                    break;
                case "savings":
                    apy = 0.0005m;
                    break;
                default:
                    apy = 0.0m;
                    break;
            }

            return apy;
        }

        public string QuickSummary()
        {
            return $"\taccount#: {accountNumber}\taccount type: {accountType}\tapy: {apy*100}%\tbalance: {balance:C}"; 
        }
        
        public override string ToString()
        {
            return $"\taccount#: {accountNumber}\n\taccount type: {accountType}\n\tapy: {apy*100}%\n\tbalance: {balance:C}\n";
        }
    }
}