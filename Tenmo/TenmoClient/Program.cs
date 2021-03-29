using System;
using System.Collections.Generic;
using TenmoClient.Data;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly TransferService transferService = new TransferService();
        private static readonly AccountService accountService = new AccountService();

        static void Main(string[] args)
        {
            Run();
        }
        private static void Run()
        {
            int loginRegister = -1;
            while (loginRegister != 1 && loginRegister != 2)
            {
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out loginRegister))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (loginRegister == 1)
                {
                    while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                    {
                        LoginUser loginUser = consoleService.PromptForLogin();
                        API_User user = authService.Login(loginUser);
                        if (user != null)
                        {
                            UserService.SetLogin(user);
                        }
                    }
                }
                else if (loginRegister == 2)
                {
                    bool isRegistered = false;
                    while (!isRegistered) //will keep looping until user is registered
                    {
                        LoginUser registerUser = consoleService.PromptForLogin();
                        isRegistered = authService.Register(registerUser);
                        if (isRegistered)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Registration successful. You can now log in.");
                            loginRegister = -1; //reset outer loop to allow choice for login
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            MenuSelection();
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {



                    //Console.WriteLine(UserService.GetToken());
                    Console.Write("Your current account balance is: $");
                    Console.WriteLine(accountService.GetBalanceOfAccount(UserService.GetUserId()));


                    //Console.Write("Enter Account ID to retrieve balance: ");

                    //int accountId = int.Parse(Console.ReadLine());

                    //Console.Write($"The balance of account {accountId} is ");
                    //Console.WriteLine(accountService.GetBalanceOfAccount(accountId));






                }
                else if (menuSelection == 2)
                {

                    int accountId = UserService.GetUserId();
                    List<TransferDetails> transfers = transferService.GetTransfers(accountId);
                    Console.WriteLine($"\n\nWhere did my money go? \nWe would all like to know. \nNow you can view below \nhow your dough got so low:");
                    Console.WriteLine($"------------------------------------------");
                    Console.WriteLine("\nTransfers \nID        From/To        Amount");
                    Console.WriteLine($"------------------------------------------");
                    foreach (TransferDetails tD in transfers)
                    {
                        Console.WriteLine($"{tD.ID}".PadRight(10) + $"To:  {tD.ToUser}".PadRight(17) + $"$ {tD.Amount}");
                    }
                    Console.WriteLine("\nWhat do IDs like to chase?... (press enter or you will never know)");
                    string nothing = Console.ReadLine();
                    Console.WriteLine("Their IDetails!\n... . . . . .  .  .  .   .   .   ");
                    Console.WriteLine("\nTo view the details from a transfer enter the ID number. To return to the main menu press 0.");
                    int userInput = int.Parse(Console.ReadLine());
                    if (userInput > 0)
                    {
                        transferService.GetTransfer(accountId, userInput);

                    }
                    else
                    {
                        Console.WriteLine("Sorry, that's not a valid transfer ID");
                    }

                }

                else if (menuSelection == 3)
                {

                }

                else if (menuSelection == 4)
                {
                    Console.WriteLine("Here is a list of Users and Ids available to send TEnmo Bucks to: ");
                    transferService.GetUsers();
                    Transfer transferAttempt = consoleService.PromptForTransferData();
                    if (transferAttempt == null || !transferAttempt.IsValid)
                    {
                        Console.WriteLine("Transfer failed - incorrect data entered.");
                    }
                    else
                    {
                        TransferDetails completedTransfer = transferService.TransferRequest(transferAttempt);
                        if (completedTransfer != null)
                        {
                            Console.WriteLine("Transfer successful.");
                            Console.WriteLine($"Transfer details: {completedTransfer.ToUser}, {completedTransfer.Amount} ");
                        }
                        else
                        {
                            Console.WriteLine("Transfer failed.");
                        }
                    }
                }


                else if (menuSelection == 5)
                {

                }


                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new API_User()); //wipe out previous login info
                    Run(); //return to entry point
                }

                else
                {
                    Console.WriteLine("Ok, come back with more munny soon!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
