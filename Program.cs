using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Program
    {
        //list to keep up with customers
        private static List<Customer> customers = new List<Customer>
        {
            new Customer(976080,"joe","smith","979456123","123 joe rd","june")
        };
        //list to keep up with employees
        private static List<Employee> employees = new List<Employee>
        {
            new Employee(123456,"will","barney","manager","april")
        };
        
        //random number generator
        private static Random rnd = new Random();

        //expressions
        private static Regex customerRegex = new Regex("^(customer)$");
        private static Regex adminRegex = new Regex("^(admin)$");
        private static Regex quitRegex = new Regex("^(quit)$");
        private static List<Regex> adminRegexes = new List<Regex>
        {
            new Regex("^(create)[ ](customer)$"), //index 0
            new Regex("^(create)[ ](account)$"), //index 1
            new Regex("^(create)[ ](deposit)$"), //index 2
            new Regex("^(create)[ ](withdraw)$"), //index 3
            new Regex("^(quit)$"), //index 4
            new Regex("^(show)[ ](customers)$"), //index 5
            new Regex("^(show)[ ](accounts)$"), //index 6
            new Regex("^(simulate)[ ][0-9]{6}[ ][0-9]{8}[ ][0-9]{1,2}$"), //index 7 - simulate word, then a 6 digit customerID, then a 8 digit accountID, then a number of years to calculate for
            new Regex("^[/](functions)$")//index 8
        };
        private static List<Regex> customerRegexes = new List<Regex>
        {
            new Regex("^(show)[ ](accounts)$"), //index 0
            new Regex("^(create)[ ](transfer)$"),
            new Regex("^(quit)$") //index 1
        };

        //loop bools
        private static bool toContinue = true;
        private static bool toContinueUser = true;
        private static bool toContinueAdmin = true;

        //login bools
        private static bool adminLoggedIn = false;
        private static bool customerLoggedIn = false;

        //current customer and admin logged-in variables
        private static Customer currentCustomerLoggedIn = new Customer();
        private static Employee currentEmployeeLoggedIn = new Employee();

        public static void Main(string[] args)
        {
            while(toContinue)
            {
                Console.Write("customer/admin> ");
                string user = Console.ReadLine();

                if(customerRegex.IsMatch(user))
                {
                    CustomerLogin();
                    CustomerMenu();
                }
                else if(adminRegex.IsMatch(user))
                {
                    AdminLogIn();
                    AdminMenu();
                }
                else if(quitRegex.IsMatch(user))
                {
                    toContinue = false;
                }
            }
        }

        //USER METHODS
        //CustomerMenu: 
        private static void CustomerMenu()
        {
            toContinueUser = true;
            while(toContinueUser)
            {
                Console.Write("customer> ");
                string userFunction = Console.ReadLine();

                if(customerRegexes[0].IsMatch(userFunction))
                {
                    currentCustomerLoggedIn.PrintAllAccounts();
                }
                else if(customerRegexes[1].IsMatch(userFunction))
                {
                    Transfer();
                }
                else if(customerRegexes[2].IsMatch(userFunction))
                {
                    toContinueUser = false;
                }
            }
        }

        //CustomerLogin:
        private static void CustomerLogin()
        {
            //initialize number of tries to 0
            int incorrectTries = 0;

            //ask for userid and password
            Console.Write("userid: ");
            int userID = int.Parse(Console.ReadLine());
            Console.Write("password: ");
            string password = Console.ReadLine();

            //find a customer with a userid that matches
            var customerBasedOffID = customers.Where(x => x.customerID == userID).ToList();
            //loop till a valid userid is given or till number of tries reaches 5
            while(customerBasedOffID.Count() < 1 && incorrectTries < 5)
            {
                Console.Write("user id does not exist. enter another user id: ");
                userID = int.Parse(Console.ReadLine());

                customerBasedOffID = customers.Where(x => x.customerID == userID).ToList();
                incorrectTries++;
            }
            //reset tries for password
            incorrectTries = 0;
            //initialize password to match userid passcode
            string correctPassword = customerBasedOffID.First().passcode;
            //loop till password is correct or till number of tries reacher 5
            while(password != correctPassword && incorrectTries < 5)
            {
                Console.Write("password is incorrect. enter another password: ");
                password = Console.ReadLine();
                incorrectTries++;
            }

            //if tries is less than five then the login info was correct, 
            //if tries is 5 or greater than the main menu loop will stop and the user is notified of failure
            if(incorrectTries < 5)
            {
                customerLoggedIn = true;
                currentCustomerLoggedIn = customerBasedOffID.First();
                Console.WriteLine($"Customer #{customerBasedOffID.First().customerID} logged in!");
            }
            else
            {
                toContinue = false;
                Console.WriteLine("too many wrong log in attempts");
            }
        }

        //Transfer: get from and recieve account (validated), get transfer amount(validated), perform transfer
        private static void Transfer()
        {
            if(currentCustomerLoggedIn.accounts.Count <= 1)
            {
                Console.WriteLine("this customer does not have 2 or more accounts");
            }
            else
            {
                //establish from and recieve accounts
                Account fromAccount;
                Account recieveAccount;

                //ask for from account id
                Console.Write("enter from account id: ");
                int fromAccountID = int.Parse(Console.ReadLine());

                //search for from account in records
                var accounts = currentCustomerLoggedIn.accounts.Where(x => x.accountNumber == fromAccountID).ToList();

                //validate from account
                while(accounts.Count() != 1)
                {
                    Console.Write("account does not exist. enter another account id: ");
                    fromAccountID = int.Parse(Console.ReadLine());

                    accounts = currentCustomerLoggedIn.accounts.Where(x => x.accountNumber == fromAccountID).ToList();
                }

                //set from account to validated account given
                fromAccount = accounts.First();

                //inform user of current from balance
                Console.WriteLine($"from account balance: {fromAccount.balance:C}");

                //ask for recieve account id
                Console.Write("enter recieve account id: ");
                int recieveAccountID = int.Parse(Console.ReadLine());

                //search for recieve account in records
                accounts = currentCustomerLoggedIn.accounts.Where(x => x.accountNumber == recieveAccountID).ToList();

                //validate recieve account
                while(accounts.Count() != 1)
                {
                    Console.Write("account does not exist. enter another account id: ");
                    fromAccountID = int.Parse(Console.ReadLine());

                    accounts = currentCustomerLoggedIn.accounts.Where(x => x.accountNumber == recieveAccountID).ToList();
                }

                //set recieve account to validated account given
                recieveAccount = accounts.First();

                //inform user of current recieve balance
                Console.WriteLine($"recieve account balance: {recieveAccount.balance:C}");

                //ask for transfer amount
                Console.Write("enter transfer amount: ");
                decimal transferAmount = decimal.Parse(Console.ReadLine());

                //validate transfer amount to be greater than 0 and less than half the account balance
                while(transferAmount <= 0.0m || transferAmount > fromAccount.balance*0.5m)
                {
                    Console.Write("the transfer amount must be greater than zero and can't be more than half the account balance. enter another amount: ");
                    transferAmount = decimal.Parse(Console.ReadLine());
                }
                
                //take transfer from account and move to other
                fromAccount.balance -= transferAmount;
                recieveAccount.balance += transferAmount;

                Console.WriteLine($"{transferAmount:C} moved from {fromAccount.accountNumber} to {recieveAccount.accountNumber}!");
            }
        }

        //ADMINISTRATOR METHODS
        //AdminLogIn: asks for admin login, user id must match in order to confirm password. if incorrectTries gets to 5 the app will quit
        private static void AdminLogIn()
        {
            //initialize number of tries to 0
            int incorrectTries = 0;

            //ask for userid and password
            Console.Write("userid: ");
            int userID = int.Parse(Console.ReadLine());
            Console.Write("password: ");
            string password = Console.ReadLine();

            //find an account with a userid that matches
            var employeeBasedOffID = employees.Where(x => x.employeeID == userID).ToList();
            //loop till a valid userid is given or till number of tries reaches 5
            while(employeeBasedOffID.Count() < 1 && incorrectTries < 5)
            {
                Console.Write("user id does not exist. enter another user id: ");
                userID = int.Parse(Console.ReadLine());

                employeeBasedOffID = employees.Where(x => x.employeeID == userID).ToList();
                incorrectTries++;
            }
            //initialize password to match userid passcode
            string correctPassword = employeeBasedOffID.First().passcode;
            //loop till password is correct or till number of tries reacher 5
            while(password != correctPassword && incorrectTries < 5)
            {
                Console.Write("password is incorrect. enter another password: ");
                password = Console.ReadLine();
                incorrectTries++;
            }

            //if tries is less than five then the login info was correct, 
            //if tries is 5 or greater than the main menu loop will stop and the user is notified of failure
            if(incorrectTries < 5)
            {
                adminLoggedIn = true;
                currentEmployeeLoggedIn = employeeBasedOffID.First();
                Console.WriteLine($"Admin #{employeeBasedOffID.First().employeeID} logged in!");
            }
            else
            {
                toContinue = false;
                Console.WriteLine("too many wrong log in attempts");
            }
        }

        //AdminMenu: 
        public static void AdminMenu()
        {
            toContinueAdmin = true;
            while(toContinueAdmin)
            {
                Console.Write("admin> ");
                string function = Console.ReadLine();

                if(adminRegexes[0].IsMatch(function)) //create customer validation
                {
                    if(!adminLoggedIn)
                    {
                        //admin login
                        AdminLogIn();
                        //check if admin logged in succesfully
                        if(toContinue)
                        {
                            CreateCustomer();
                        }
                    }
                    else if(adminLoggedIn)
                    {
                        CreateCustomer();
                    }
                }
                else if(adminRegexes[1].IsMatch(function)) //create account validation
                {
                    if(!adminLoggedIn)
                    {
                        //admin login
                        AdminLogIn();
                        //check if admin logged in succesfully
                        if(toContinue)
                        {
                            CreateAccount();
                        }
                        
                    }
                    else if(adminLoggedIn)
                    {
                        CreateAccount();
                    }    
                }
                else if(adminRegexes[2].IsMatch(function)) //create deposit validation
                {
                    if(!adminLoggedIn)
                    {
                        //admin login
                        AdminLogIn();
                        //check if admin logged in succesfully
                        if(toContinue)
                        {
                            Deposit();
                        }
                    }
                    else if(adminLoggedIn)
                    {
                        Deposit();
                    }
                }
                else if(adminRegexes[3].IsMatch(function)) //create withdraw validation
                {
                    if(!adminLoggedIn)
                    {
                        //admin login
                        AdminLogIn();
                        //check if admin logged in succesfully
                        if(toContinue)
                        {
                            Withdraw();
                        }
                    }
                    else if(adminLoggedIn)
                    {
                        Withdraw();
                    }
                }
                else if(adminRegexes[4].IsMatch(function)) //quit validation
                {
                    toContinueAdmin = false;
                }
                else if(adminRegexes[5].IsMatch(function)) //show customers validation
                {
                     if(!adminLoggedIn)
                    {
                        //admin login
                        AdminLogIn();
                        //check if admin logged in succesfully
                        if(toContinue)
                        {
                            ShowCustomers();
                        }
                    }
                    else if(adminLoggedIn)
                    {
                        ShowCustomers();
                    }
                }
                else if(adminRegexes[6].IsMatch(function)) //show accounts validation
                {
                     if(!adminLoggedIn)
                    {
                        //admin login
                        AdminLogIn();
                        //check if admin logged in succesfully
                        if(toContinue)
                        {
                            ShowAccounts();
                        }
                    }
                    else if(adminLoggedIn)
                    {
                        ShowAccounts();
                    }
                }
                else if(adminRegexes[7].IsMatch(function)) //simulate validation
                {
                    Simulate(function);
                }
                else if(adminRegexes[8].IsMatch(function)) //show functions validation
                {
                    ShowFunctions();
                }
                else
                {
                    Console.WriteLine("invalid function: enter /funcs for a list of current functions");
                }
            }
        }

        //ShowFunctions: show all admin functions
        private static void ShowFunctions()
        {
            Console.WriteLine("- create customer\n- create account\n- create deposit\n- create withdraw\n- show customers\n- show accounts\n- simulate\nquit");
        }

        //CreateCustomer: 
        private static void CreateCustomer()
        {
            int customerID = rnd.Next(100000,1000000);
            var mathcedCustomers = from customer in customers
                                   where customer.customerID == customerID
                                   select customer;

            while(mathcedCustomers.Count() != 0)
            {
                Console.WriteLine("customer id already exists");
                customerID = rnd.Next(1,1000000);
                mathcedCustomers = from customer in customers
                                    where customer.customerID == customerID
                                    select customer;
            }

            Console.Write("enter first name: ");
            string firstName = Console.ReadLine();

            Console.Write("enter last name: ");
            string lastName = Console.ReadLine();

            Console.Write("enter social security number: ");
            string ssn = Console.ReadLine();

            Console.Write("enter address: ");
            string address = Console.ReadLine();

            string password1;
            string password2;
            string passWord;

            do{
                Console.Write("enter password: ");
                password1 = Console.ReadLine();

                Console.Write("enter password again: ");
                password2 = Console.ReadLine();
            }
            while(password1 != password2);

            passWord = password1;

            customers.Add(new Customer(customerID,firstName,lastName,ssn,address,passWord));

            Console.WriteLine($"Customer ID {customerID} created!");
        }

        //CreateAccount: 
        private static void CreateAccount()
        {
            Console.Write("Enter customer ID: ");
            int customerID = int.Parse(Console.ReadLine());
            var matchedCustomers = from customer in customers
                                   where customer.customerID == customerID
                                   select customer;
            
            while(matchedCustomers.Count() != 1)
            {  
                Console.Write("customer does not exist! Enter another customer id: ");
                customerID = int.Parse(Console.ReadLine());

                matchedCustomers = from customer in customers
                                   where customer.customerID == customerID
                                   select customer;
            }

            /*Console.Write("Enter customer password: ");
            string password = Console.ReadLine();*/

            Console.Write("Enter account type: ");
            string accountType = Console.ReadLine();

            int accountNum = JoinNumbers(customerID,accountType);

            Customer accountCustomer = matchedCustomers.First();

            accountCustomer.accounts.Add(new Account(accountNum,accountType,0.0m));
        }

        //joinnumbers: makes an account number based off the customer id and account type {customerid}+00/01/02/03/04/05/06/07/08/09
        public static int JoinNumbers(int cusID,string accType)
        {
            int accNum;
            int joinedNumber = cusID;
            switch(accType)
            {
                case "checking":
                    accNum = 00;
                    joinedNumber = int.Parse(cusID.ToString()+"0"+accNum.ToString());
                    break;
                case "savings":
                    accNum = 01;
                    joinedNumber = int.Parse(cusID.ToString()+"0"+accNum.ToString());
                    break;
                case "money market":
                    accNum = 02;
                    joinedNumber = int.Parse(cusID.ToString()+"0"+accNum.ToString());
                    break;
                default:
                    accNum = 11;
                    joinedNumber = int.Parse(cusID.ToString()+"0"+accNum.ToString());
                    break;
            }

            return joinedNumber;
        }

        //deposit: ask for customer id(validated) then ask for account id(validated). When the deposit amount is given it must be greater than 0. The deposit amount is added to the selected account.
        private static void Deposit()
        {
            Account matchedAccount;

            Console.Write("Enter customer id: ");
            int cusID = int.Parse(Console.ReadLine());

            var chosenCustomer = customers.Where(x => x.customerID == cusID).ToList();

            while(chosenCustomer.Count() != 1)
            {
                Console.Write("customer id does not exist. enter another customer id: ");
                cusID = int.Parse(Console.ReadLine());

                chosenCustomer = customers.Where(x => x.customerID == cusID).ToList();
            }

            if(chosenCustomer.First().accounts.Count() == 0)
            {
                Console.WriteLine("this customer has no accounts");
            }
            else
            {
                Console.Write("enter account id: ");
                int accID = int.Parse(Console.ReadLine());

                var chosenAccount = chosenCustomer.First().accounts.Where(x => x.accountNumber == accID).ToList();

                while(chosenAccount.Count() != 1)
                {
                    Console.Write("account does not exist. enter another account id: ");
                    accID = int.Parse(Console.ReadLine());

                    chosenAccount = chosenCustomer.First().accounts.Where(x => x.accountNumber == accID).ToList();
                }

                matchedAccount = chosenAccount.First();

                Console.Write("enter deposit amount: ");
                decimal deposit = decimal.Parse(Console.ReadLine());

                while(deposit <= 0.0m)
                {
                    Console.Write("deposit must be greater than 0.00. enter another deposit amount: ");
                    deposit = decimal.Parse(Console.ReadLine());
                }

                matchedAccount.balance += deposit;
                Console.WriteLine($"{deposit:C} added to account {matchedAccount.accountNumber}");
            }
        }

        //withdraw: ask for customerID(validation) then ask for accountID(validation). When the withdraw amount is given it must be greater than 0 and less than the current balance
        private static void Withdraw()
        {
            Account matchedAccount;
            
            Console.Write("Enter customer id: ");
            int cusID = int.Parse(Console.ReadLine());

            var chosenCustomer = customers.Where(x => x.customerID == cusID).ToList();

            while(chosenCustomer.Count() != 1)
            {
                Console.Write("customer id does not exist. enter another customer id: ");
                cusID = int.Parse(Console.ReadLine());

                chosenCustomer = customers.Where(x => x.customerID == cusID).ToList();
            }

            if(chosenCustomer.First().accounts.Count() == 0)
            {
                Console.WriteLine("this customer has no accounts");
            }
            else
            {
                Console.Write("enter account id: ");
                int accID = int.Parse(Console.ReadLine());

                var chosenAccount = chosenCustomer.First().accounts.Where(x => x.accountNumber == accID).ToList();

                while(chosenAccount.Count() != 1)
                {
                    Console.Write("account does not exist. enter another account id: ");
                    accID = int.Parse(Console.ReadLine());

                    chosenAccount = chosenCustomer.First().accounts.Where(x => x.accountNumber == accID).ToList();
                }

                matchedAccount = chosenAccount.First();
                decimal currentBalance = matchedAccount.balance;

                Console.Write("enter withdrawal amount: ");
                decimal withdrawal = decimal.Parse(Console.ReadLine());

                while(withdrawal <= 0.0m || withdrawal >= (currentBalance * 0.5m))
                {
                    Console.Write("withdrawal must be greater than 0.00\nand can not be more than half your current balance.\nenter another withdrawal amount: ");
                    withdrawal = decimal.Parse(Console.ReadLine());
                }

                matchedAccount.balance -= withdrawal;
                Console.WriteLine($"withdrew {withdrawal:C} from account {matchedAccount.accountNumber}");
            }
        }

        //ShowCustomers: call customer toString method for each customer in the customers list
        private static void ShowCustomers()
        {
            foreach(Customer customer in customers)
            {
                Console.Write($"**********\n{customer}");
            }
        }

        //ShowAccounts: get the customerid, then call the showallaccounts method
        private static void ShowAccounts()
        {
            Console.Write("Enter customer ID: ");
            int customerID = int.Parse(Console.ReadLine());
            var matchedCustomers = from customer in customers
                                   where customer.customerID == customerID
                                   select customer;
            
            while(matchedCustomers.Count() != 1)
            {  
                Console.Write("customer does not exist! Enter customer id: ");
                customerID = int.Parse(Console.ReadLine());

                matchedCustomers = from customer in customers
                                   where customer.customerID == customerID
                                   select customer;
            }

            Customer pickedCustomer = matchedCustomers.First();

            pickedCustomer.PrintAllAccounts();
        }

        //Simulate: take the function, split the string to get the cusid,accid and years, validate the customer and account, determine the compound interest and show the user
        private static void Simulate(string function)
        {
            //split the function to get the three variables after simulate keyword
            string[] simulateCusAccYears = function.Split(" ");
            int customerID = int.Parse(simulateCusAccYears[1]);
            int accountID = int.Parse(simulateCusAccYears[2]);
            int years = int.Parse(simulateCusAccYears[3]);

            //function vars
            Customer matchedCustomer;
            Account matchedAccount;
            double totalValue = 0.0;

            //go through customer list to find the given customer based of ID
            var customer = customers.Where(x => x.customerID == customerID).ToList();

            if(customer.Count != 1)
            {
                Console.WriteLine("this function could not be completed due to a non-existent customer id. please try again");
            }
            else
            {
                //set matched customer to customer
                matchedCustomer = customer.First();

                //go through customer accounts to find given account based off ID
                var account = matchedCustomer.accounts.Where(x => x.accountNumber == accountID).ToList();

                if(account.Count != 1)
                {
                    Console.WriteLine("this could not be completed due to a non existent account id. please try again");
                }
                else
                {
                    //set matched account to account
                    matchedAccount = account.First();

                    if(matchedAccount.balance <= 0.0m)
                    {
                        Console.WriteLine("the balance on this account is too low. please try again");
                    }
                    else
                    {
                        double dub = 1.0+(((double)matchedAccount.apy)/12);

                        totalValue = (double)matchedAccount.balance * Math.Pow(dub,12*years);

                        Console.WriteLine($"Customer ID: {customerID}\nAccount ID: {accountID}\nStarting Amount: {matchedAccount.balance:C}\nYears: {years}\nInterest Rate: {matchedAccount.apy}\nExpected Value: {totalValue:C}");
                    }
                }
            }
        }
    }
}
