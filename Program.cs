
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static BankApplication.BankApp;
using static BankApplication.BankApp.Bank;

namespace BankApplication
{
    // The main class of the application
    class BankApp
    {
        // Nested class representing the bank system
        public class Bank
        {
            // List to store all bank accounts
            private List<Account> accounts = new List<Account>();

            // Counter to generate unique account numbers
            private int nextAccountNumber = 1001;

            // Counter to track failed login attempts
            private int loginAttempts = 0;

            // Maximum number of allowed login attempts before exiting
            private const int maxLoginAttempts = 3;

            // Method to create a new bank account
            public void CreateAccount()
            {
                Account newAccount = new Account(); // Instantiate a new account object

                // Loop to get and validate the first name
                while (true)
                {
                    Console.Write("Enter Your First Name: ");
                    newAccount.FirstName = Console.ReadLine();
                    if (ValidateName(newAccount.FirstName))
                        break;
                    else
                        Console.WriteLine("Invalid first name. Please try again.\n");
                }

                // Loop to get and validate the last name
                while (true)
                {
                    Console.Write("Enter Your Last Name: ");
                    newAccount.LastName = Console.ReadLine();
                    if (ValidateName(newAccount.LastName))
                        break;
                    else
                        Console.WriteLine("Invalid last name. Please try again.\n");
                }

                // Loop to get and validate the email address
                while (true)
                {
                    Console.Write("Enter Your Email Address: ");
                    newAccount.Email = Console.ReadLine();
                    if (ValidateEmail(newAccount.Email))
                        break;
                    else
                        Console.WriteLine("Invalid email address. Please try again.\n");
                }

                Console.Write("Enter Your Address: ");
                newAccount.Address = Console.ReadLine(); // Get the user's address

                // Loop to get and validate the date of birth
                while (true)
                {
                    Console.Write("Enter Your Date of Birth (YYYY-MM-DD): ");
                    newAccount.Dob = Console.ReadLine();
                    if (ValidateDob(newAccount.Dob))
                        break;
                    else
                        Console.WriteLine("Invalid date of birth. Please try again.\n");
                }

                // Loop to get and validate the phone number
                while (true)
                {
                    Console.Write("Enter Your Phone Number: ");
                    newAccount.PhoneNumber = Console.ReadLine();
                    if (ValidatePhoneNumber(newAccount.PhoneNumber))
                        break;
                    else
                        Console.WriteLine("Invalid phone number. Please try again.\n");
                }

                // Loop to get and validate the starting balance
                while (true)
                {
                    Console.Write("Enter Your Starting Balance: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal balance))
                    {
                        newAccount.Balance = balance; // Set the balance
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid balance. Please enter a valid decimal number.\n");
                    }
                }

                // Loop to get and validate the password
                while (true)
                {
                    Console.Write("Enter Password (min 6 characters, 1 letter, 1 number, 1 special character): ");
                    newAccount.Password = Console.ReadLine();
                    if (ValidatePassword(newAccount.Password))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Password does not meet requirements. Please try again. \n");
                    }
                }

                // Assign a unique account number and username to the new account
                newAccount.AccountNumber = nextAccountNumber;
                newAccount.Username = nextAccountNumber.ToString();
                accounts.Add(newAccount); // Add the new account to the list
                nextAccountNumber++; // Increment the account number for the next account

                Console.WriteLine($"\nAccount created successfully. Your account number is {newAccount.AccountNumber} \n");
            }

            // Method to check the balance of an account
            public void CheckBalance(Account account)
            {
                Console.WriteLine($"Your balance is: {account.Balance} \n");
            }

            // Method to withdraw money from an account
            public void MakeWithdrawal(Account account)
            {
                Console.Write("Enter amount to withdraw: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                if (account.Balance >= amount)
                {
                    account.Balance -= amount; // Subtract the amount from the balance
                    Console.WriteLine($"Withdrawal successful. New balance: {account.Balance} \n");
                }
                else
                {
                    Console.WriteLine("Insufficient funds. \n");
                }
            }

            // Method to deposit money into an account
            public void MakeDeposit(Account account)
            {
                Console.Write("Enter amount to deposit: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                account.Balance += amount; // Add the amount to the balance
                Console.WriteLine($"Deposit successful. New balance: {account.Balance} \n");
            }

            // Method to transfer funds between accounts
            public void TransferFunds(Account account)
            {
                Console.Write("Enter recipient account number: ");
                string recipientAccountNumber = Console.ReadLine();
                Account recipientAccount = accounts.Find(a => a.AccountNumber.ToString() == recipientAccountNumber);

                if (recipientAccount == null)
                {
                    Console.WriteLine("Recipient account not found.\n");
                    return;
                }

                Console.Write("Enter amount to transfer: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                if (account.Balance >= amount)
                {
                    account.Balance -= amount; // Subtract the amount from the sender's balance
                    recipientAccount.Balance += amount; // Add the amount to the recipient's balance
                    Console.WriteLine($"Transfer successful. Your new balance: {account.Balance} \n");
                }
                else
                {
                    Console.WriteLine("Insufficient funds for transfer. \n");
                }
            }

            // Method to display account information
            public void DisplayAccountInformation(Account account)
            {
                Console.WriteLine($"{account.FirstName}'s Account Information:\n\n");
                Console.WriteLine($"Account Number: {account.AccountNumber}");
                Console.WriteLine($"Name: {account.FirstName} {account.LastName}");
                Console.WriteLine($"Date of Birth: {account.Dob}");
                Console.WriteLine($"Email: {account.Email}");
                Console.WriteLine($"Address: {account.Address}");
                Console.WriteLine($"Phone Number: {account.PhoneNumber}");
                Console.WriteLine($"Balance: {account.Balance}\n\n");
                Console.WriteLine();
            }

            // Method to authenticate a user based on account number and password
            public Account AuthenticateUser()
            {
                // Check if the maximum number of login attempts has been reached
                if (loginAttempts >= maxLoginAttempts)
                {
                    Console.WriteLine("You have tried too many times, Program will now end. Goodbye!");
                    Environment.Exit(0); // Terminate the program
                }

                // Prompt the user for account number and password
                Console.Write("Enter Account Number: ");
                string accountNumber = Console.ReadLine();
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                // Iterate through accounts to find a match
                foreach (var account in accounts)
                {
                    if (account.AccountNumber.ToString() == accountNumber && account.Password == password)
                    {
                        loginAttempts = 0; // Reset the login attempts counter on successful login
                        return account; // Return the authenticated account
                    }
                }

                Console.WriteLine("Authentication failed. Please try again. \n");
                loginAttempts++; // Increment the login attempts counter on failure
                return null; // Return null if authentication fails
            }

            // Helper method to validate the password based on criteria
            private static bool ValidatePassword(string password)
            {
                if (password.Length < 6)
                    return false;

                bool hasLetter = false, hasDigit = false, hasSpecialChar = false;
                foreach (char c in password)
                {
                    if (char.IsLetter(c)) hasLetter = true;
                    if (char.IsDigit(c)) hasDigit = true;
                    if ("!@#$%^&*()<>?,./".Contains(c)) hasSpecialChar = true;
                }

                return hasLetter && hasDigit && hasSpecialChar;
            }

            // Helper method to validate names (only letters, no spaces)
            private static bool ValidateName(string name)
            {
                return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
            }

            // Helper method to validate email format
            private static bool ValidateEmail(string email)
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern);
            }

            // Helper method to validate date of birth format (YYYY-MM-DD)
            private static bool ValidateDob(string dob)
            {
                DateTime parsedDate;
                return DateTime.TryParseExact(dob, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedDate);
            }

            // Helper method to validate phone numbers (10 digits)
            private static bool ValidatePhoneNumber(string phoneNumber)
            {
                string phonePattern = @"^\d{10}$";
                return Regex.IsMatch(phoneNumber, phonePattern);
            }

            // Nested class representing a bank account
            public class Account
            {
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Email { get; set; }
                public string Address { get; set; }
                public string Dob { get; set; }
                public string PhoneNumber { get; set; }
                public decimal Balance { get; set; }
                public int AccountNumber { get; set; }
                public string Username { get; set; }
                public string Password { get; set; }
            }
        }

        // start of the program
        static void Main(string[] args)
        {
            Bank bank = new Bank(); // Instantiate the bank object
            bool quit = false; // Flag to control the main program loop

            // Main program loop
            while (!quit)
            {
                Console.WriteLine("Welcome to ABC Bank!\n");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Quit Program \n");
                Console.Write("Choose an option: ");
                int choice1 = int.Parse(Console.ReadLine());

                // Switch case to handle main menu choices
                switch (choice1)
                {
                    case 1:
                        Account account = bank.AuthenticateUser(); // Authenticate the user
                        if (account != null)
                        {
                            bool userSessionActive = true; // Flag to control the user session loop
                            while (userSessionActive)
                            {
                                Console.WriteLine($"Welcome {account.FirstName}!\n\n");
                                Console.WriteLine("1. Check Balance");
                                Console.WriteLine("2. Make Withdrawal");
                                Console.WriteLine("3. Make Deposit");
                                Console.WriteLine("4. Transfer Funds");
                                Console.WriteLine("5. Display Account Information");
                                Console.WriteLine("6. Logout \n");
                                Console.Write("Choose an option: ");
                                int choice2 = int.Parse(Console.ReadLine());

                                // Switch case to handle user session choices
                                switch (choice2)
                                {
                                    case 1:
                                        bank.CheckBalance(account); // Check the account balance
                                        break;
                                    case 2:
                                        bank.MakeWithdrawal(account); // Withdraw money
                                        break;
                                    case 3:
                                        bank.MakeDeposit(account); // Deposit money
                                        break;
                                    case 4:
                                        bank.TransferFunds(account); // Transfer funds
                                        break;
                                    case 5:
                                        bank.DisplayAccountInformation(account); // Display account info
                                        break;
                                    case 6:
                                        Console.WriteLine("Thank you for using ABC Bank!");
                                        userSessionActive = false; // End the user session
                                        break;
                                    default:
                                        Console.WriteLine("Invalid option. Please try again. \n");
                                        break;
                                }
                            }
                        }
                        break;
                    case 2:
                        bank.CreateAccount(); // Create a new account
                        break;
                    case 3:
                        Console.WriteLine("Thank you for using ABC Bank!");
                        quit = true; // Exit the program
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again. \n");
                        break;
                }
            }
        }
    }
}