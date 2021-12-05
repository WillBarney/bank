using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank
{
    class Customer
    {
        public int customerID;
        public string firstName;
        public string lastName;
        public string ssn;
        public string address;
        public List<Account> accounts = new List<Account>();
        private string passcode;
 
        public Customer(int customerID,string firstName,string lastName,string ssn,string address,string passcode)
        {
            this.customerID = customerID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.ssn = ssn;
            this.address = address;
            this.passcode = passcode;
        }

        public string PrintAllAccounts()
        {
            string accountsSummary = "";
            foreach(var account in accounts)
            {
                accountsSummary += $"\taccount#: {account.accountNumber}\taccount type: {account.accountType}\tapy: {account.apy}\tbalance: {account.balance:C}";
            }
            return accountsSummary;
        }

        public void ResetPassword()
        {
            
        }
        
        public override string ToString()
        {
            return $"customerID: {customerID}\nfirst name: {firstName}\nlast name: {lastName}\nssn: ***-**-{ssn.Substring(ssn.Length-4)}\naddress: {address}\nnumber of accounts: {accounts.Count()}\n";
        }
    }
}