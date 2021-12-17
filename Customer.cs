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
        public string passcode;
        
        public Customer()
        {
            
        }

        public Customer(int customerID,string firstName,string lastName,string ssn,string address,string passcode)
        {
            this.customerID = customerID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.ssn = ssn;
            this.address = address;
            this.passcode = passcode;
        }

        public void PrintAllAccounts()
        {
            if(accounts.Count() < 1)
            {
                Console.WriteLine("this customer has no accounts");
            }
            else
            {
                string accountsSummary = "";
                foreach(var account in accounts)
                {
                    accountsSummary += $"\taccount#: {account.accountNumber}\taccount type: {account.accountType}\tapy: {account.apy}\tbalance: {account.balance:C}\n";
                }
                Console.WriteLine(accountsSummary);
            }
        }

        public void ResetPassword()
        {
            string password1;
            string password2;
            do{
                Console.Write("enter new password: ");
                password1 = Console.ReadLine();

                Console.Write("enter new password again: ");
                password2 = Console.ReadLine();
            }
            while(password1 != password2);

            this.passcode = password1;
        }
        
        public override string ToString()
        {
            return $"customerID: {customerID}\nfirst name: {firstName}\nlast name: {lastName}\nssn: ***-**-{ssn.Substring(ssn.Length-4)}\naddress: {address}\nnumber of accounts: {accounts.Count()}\n";
        }
    }
}
