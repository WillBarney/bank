using System;

namespace Bank
{
    class Employee : AdminTools
    {
        public int employeeID;
        public string firstName;
        public string lastName;
        public string address;
        public string phone;
        public string ssn;
        public string position;
        public int levelOfAccess;
        public string passcode;

        public Employee()
        {

        }

        public Employee(int employeeID,string firstName,string lastName,string position,string passcode)
        {
            this.employeeID = employeeID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.position = position;
            this.passcode = passcode;
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
    }
}
