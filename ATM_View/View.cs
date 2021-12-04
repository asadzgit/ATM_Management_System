using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using ATM_BO;
using ATM_BLL;

namespace ATM_View
{
    public class View
    {
        public void getPin(ref char[] pin,ref string pinInput)
        {
            int i = 0;
            pin = new char[pinInput.Length];
            for (; i < pinInput.Length; i++)
            {
                pin[i] = pinInput[i];
            }
        }
        public void GetInput()
        {
            BLL bll = new BLL();
            string login, input,pinInput;
            char[] pin=null;
            int fails = 0;
            bool pinIsWrong = false;
            Console.Write("Enter login: ");
            login = Console.ReadLine();
            // admin logins
            if (login == "admin")   
            {
                do
                {
                label:
                    Console.Write("Enter 5 digit pincode: ");
                    pinInput = Console.ReadLine();
                    try
                    {
                        int.Parse(pinInput);
                    }
                    catch(Exception ex)
                    {
                        WriteLine(ex.Message);
                        goto label;
                    }
                    if (pinInput == string.Empty || pinInput.Length<5 || pinInput.Length>5 )
                        goto label;
                    //pin is confirmed to be 5 digit
                    getPin(ref pin, ref pinInput);
                   // WriteLine(pin);
                    pinIsWrong = bll.matchAdminPin(pin, login);

                    if (pinIsWrong == true)
                        Console.WriteLine("Wrong PinCode");

                } while (pinIsWrong == true);
                //pin is correct
                showAdminMenu();
            }
            //cutomer logins
            else
            {
                bool loginMatch = false;
                char status;
                BO customer = new BO();
                bool customersMade = bll.customersExist();
                if (customersMade == false)
                {
                    Console.WriteLine("No customer data in file\n" + "Please LogIn as aAdmin and create some customer accounts first  ");
                    return;
                }
                char[] cLogin = new char[login.Length];
                int i = 0;
                for(;i<login.Length;i++)
                {
                    cLogin[i] = login[i];
                }
                loginMatch = bll.matchCustomerLogin(ref customer,cLogin);
               
                if (loginMatch == true)
                    status = bll.getStatus(cLogin, ref customer);
                else
                {
                    Console.WriteLine("The Login did not match any account\n!!Bye!!");
                    return;
                }
                if (status == 'd')
                {
                    Console.WriteLine("This account is Deactivated");
                    return;
                }
                //now login and status is valid
                do
                {
                    fails++;
                    Console.Write("Enter 5 digit pincode: ");
                    pinInput = Console.ReadLine();
                    try
                    {
                        int.Parse(pinInput);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    if (fails == 3)
                    {
                        Console.WriteLine("The Pin Code did not match any account\nThe account is going to be deactivated");
                        bll.deactivateAndSave(ref customer);
                        return;
                    }
                    getPin(ref pin, ref pinInput);
                } while (pinInput == string.Empty || pin.Length < 5|| pin.Length>5 || bll.matchPin(ref pin, ref customer) == false);
                //pin is matched
                showCustomerMenu(ref customer);
            }

        }//getinput func

        public void showAdminMenu()
        {
            BLL logicObj = new BLL();

        label1:
            Console.WriteLine("--Admin Menu--");
            Console.Write("1----Create New Account\n" + "2----Delete Existing Account\n" + "3----Update Account Information\n" +
                           "4----Search for Account\n" + "5----View Reports\n" +
                           "6----Exit\n" + "Please select one of the above options: ");
            int? option = null;
            string input;
            //get input
            input = Console.ReadLine();
            try
            {
                option = int.Parse(input);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //call functions accordingly
            switch (option)
            {
                case 1:
                    Clear();
                    createAccount();
                    break;
                case 2:
                    Clear();
                    deleteAccount();
                    break;
                case 3:
                    Clear();
                    updateAccount();
                    break;
                case 4:
                    Clear();
                    searchAccount();
                    break;
                case 5:
                    Clear();
                    ViewReports();
                    break;
                case 6:
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("You must chose among 1,2,3,4,5 and 6 ");
                    goto label1;
            }

        }//admin menu
        public void createAccount()
        {
            BLL logicObj = new BLL();
            string login, pinInput,balanceInput, name, type, status;
            decimal balance;
            char[] pin=null;
            //getting necessary data
        label0:
            Write("Login: ");
            login = ReadLine();
            if(login == string.Empty)
            {
                WriteLine("You can not leave this filed empty");
                goto label0;
            }
                
        label1:
            Write("Pin Code: ");
            pinInput = Console.ReadLine();
            try
            {
                int.Parse(pinInput);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto label1;
            }
            getPin(ref pin, ref pinInput);
            if (pin == null || pin.Length < 5 || pin.Length>5)
            {
                Console.WriteLine("Pin Code should be 5-digit");
                goto label1;
            }
        label1b:
            Write("Holders Name: ");
            name = ReadLine();
            if(name == string.Empty)
            {
                WriteLine("You can not leave this filed empty");
                goto label1b;
            }
            int i;
            bool numberExist = false;
            for(i=0;i<name.Length;i++)
            {
                if(('0' <=name[i] && name[i]<='9') && ('A' > name[i] && name[i] > 'Z') && ('a' > name[i] && name[i] > 'z'))
                {
                    numberExist = true;
                    break;
                }
            }
            if(numberExist)
            {
                WriteLine("Name must have all alphabets");
                goto label1b;
            }
        label2:
            Write("Type (Savings, Current): ");
            type = ReadLine();
            if (type != "Savings" && type != "Current")
            {
                Console.WriteLine("Account should be of Savings or Current type only");
                goto label2;
            }

        label3:
            Write("Starting Balance: ");
            balanceInput = ReadLine();
            try
            {
                balance = decimal.Parse(balanceInput);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto label3;
            }
        label10:
            Write("Status: ");
            status = ReadLine();
            if (status != "Active" && status != "Deactivated")
            {
                Console.WriteLine("Status must be Active or Deactivated");
                goto label10;
            }
            char[] cLogin = new char[login.Length];
            i = 0;
            for (; i < login.Length; i++)
            {
                cLogin[i] = login[i];
            }
            //logic layer will create a new customer object
            int accountNo = logicObj.createAccount(cLogin,  ref pin, name, type, balance, status);
            Console.WriteLine($"Account Successfully Created – the account number assigned is: {accountNo}");

            quit();
            showAdminMenu();
        }// crate account

        public void deleteAccount()
        {
            BLL logicObj = new BLL();
            int account = -1 , confirm = -1;
        label1:
            Write("Enter the account number to which you want to delete: ");
            string input = ReadLine();
            try
            {
                account = int.Parse(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto label1;
            }
            // checks if customer records exist in files
            bool flag = logicObj.customersExist();
            if (flag == false)
            {
                Console.WriteLine("No data in file");
                quit();
                showAdminMenu();
            }
            // checks if current customer's record exist in files
            flag = logicObj.isExist(account);
            if (flag == false)
            {
                Console.WriteLine("No such account exists" + "Try again");
                goto label1;
            }
            string name="";
            //get customer name through its account number
            logicObj.getCustomerName(account, ref name);
            Write(" You wish to delete the account held by Mr " + name + ". If this information is correct please re - enter the account number: ");
            input = ReadLine();
            try
            {
                confirm = int.Parse(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                quit();
                showAdminMenu();
            }
            //action confirmed
            if (account == confirm)
            {
                logicObj.deleteAccount(ref account);
                Console.WriteLine("Account Deleted Successfully");
            }
            else //wrong number entered at confirm
            {
                Console.WriteLine("You did not eneter the same account number!");
            }
            quit();
            showAdminMenu();
        }//delete account

        public void updateAccount()
        {
            BLL logicObj = new BLL();
            string input = "";
            BO bo = new BO();
            int account = -1;
        label1:
            Write("Enter the Account Number: ");
            try
            {
                input = ReadLine();
                account = int.Parse(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto label1;
            }
            // checks if customer records exist in files
            bool flag = logicObj.customersExist();
            if (flag == false)
            {
                Console.WriteLine("No data in file");
                quit();
                showAdminMenu();
            }
            // checks if current customer's record exist in files
            flag = logicObj.isExist(account);
            if (flag == false)
            {
                Console.WriteLine("No such account exists" + "Try again");
                return;
            }
            //getting the cooresponding customer object in bo from file
            logicObj.getCustomer(ref account, ref bo);
            //prompting the user
            Console.WriteLine($"Account # {bo.AccountNo}");
            Console.WriteLine($"type : {bo.Type}");
            Console.WriteLine($"Holder : {bo.Name}");
            Console.WriteLine($"Balance : {bo.Balance}");
            Console.WriteLine($"Status : {bo.Status}");
            WriteLine("Please enter in the fields you wish to update (leave blank otherwise):");
            string login, pinInput, name, type, status;
            char[] pin=null;
            char[] cLogin = null;
        label1b:    
            Write("Login: "); login = ReadLine();
            if(login != string.Empty)
            {
                cLogin = new char[login.Length];
                getPin(ref cLogin, ref login);
            }

        label2:
            Write("Pin Code: "); pinInput = ReadLine();
            if (pinInput != string.Empty)
            {
                try
                {
                    int.Parse(pinInput);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    goto label2;
                }
                if (pin.Length < 5 )
                {
                    Console.WriteLine("Pin Code must be 5 digit");
                    goto label2;
                }
                getPin(ref pin, ref pinInput);
            }

            Write("Holders Name: "); name = ReadLine();
        label3:
            Write("Type (Savings, Current): "); type = ReadLine();
            if ((type != string.Empty) && (type != "Savings" && type != "Current"))
            {
                Console.WriteLine("Type must be of Savings or Current");
                goto label3;
            }
        label4:
            Write("Status: "); status = ReadLine();
            if ((status != string.Empty) && (status != "Active" && status != "Deactivated"))
            {
                Console.WriteLine("Status can be either Activate or Deactivated");
                goto label4;
            }
            //updating the corresponding customer object with given data
            logicObj.updateAccount(account,cLogin,pin,name,type,status);

            WriteLine("Your account has been successfully updated.");
            quit();
            showAdminMenu();
        }//update account

        public void searchAccount()
        {
            BLL logicObj = new BLL();
            string name="", type="", status="";
            decimal balance = -1; 
            int account = 0, userID = 0;
            string[] input = new string[6];
            
            Console.WriteLine("SEARCH MENU:");
            // checks if customer records exist in files
            bool flag = logicObj.customersExist();
            if (flag == false)
            {
                Console.WriteLine("No data in file");
                quit();
                showAdminMenu();
            }
        labelA:
            Write("Account ID: ");
            input[0] = ReadLine();
            if (input[0] != string.Empty)
            {
                try
                {
                    account = int.Parse(input[0]);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    goto labelA;
                }
            }            
        label9:
            //prompting search criteria
            Write("User ID: ");
            input[1] = ReadLine();
            if (input[1] != string.Empty)
            {
                try
                {
                    userID = int.Parse(input[1]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    goto label9;
                }
            }  
            Write("Holders Name: ");
            input[2] = ReadLine();
            if (input[2] != string.Empty)
                name = input[2];
        label2:
            Write("Type (Savings, Current): ");
            input[3] = ReadLine();
            if (input[3] != string.Empty)
            {
                type = input[3];
                if (input[3] != "Savings" && input[3] != "Current")
                {
                    Console.WriteLine("Type must be Savings or Current");
                    goto label2;
                }
            }    
            Write("Starting Balance: ");
            input[4] = ReadLine();
            if (input[4] != string.Empty)
            {
                balance = decimal.Parse(input[4]);
            }
        label3:
            Write("Status: "); input[5] = ReadLine();
            if (input[5] != string.Empty)
            {
                status = input[5];
                if (input[5] != "Active" && input[5] != "Deactivated")
                {
                    Console.WriteLine("Status must be Active or Deactivated");
                    goto label3;
                }
                
            }
            bool match = false;
            List<BO> dummy = new List<BO>();
            List<BO> bo = new List<BO>();
            //searching form file
            logicObj.searchAccount(ref input, account, userID, name, type, balance, status, ref bo, ref dummy);
            Console.WriteLine("==== SEARCH RESULTS ======");
            //writing matched results
            Console.WriteLine("Account ID  User ID  Type    Balance  Status   Holders Name ");
            for (int j = 0; j < dummy.Count; j++)
            {
                if ((dummy[j].AccountNo == bo[j].AccountNo) && (dummy[j].userID == bo[j].userID) && (dummy[j].Name == bo[j].Name) &&
                    (dummy[j].Type == bo[j].Type) && (dummy[j].Balance == bo[j].Balance) && (dummy[j].Status == bo[j].Status))
                {
                    Console.WriteLine($"{bo[j].AccountNo}           {bo[j].userID}     {bo[j].Type}  {bo[j].Balance}    {bo[j].Status}   {bo[j].Name}");
                   //atleast one records matches the search criteria
                    match = true;
                }
            }
            //none of the records matches the search criteria
            if(match == false)
            {
                Console.WriteLine("\n<----!No such record exists!---->");
            }
            quit();
            showAdminMenu();
        }

        public void ViewReports()
        {
            int? option = null;
            string input;
            BLL bll = new BLL();
            //prompt user and get option
            //confirm that a valid option is given
        label1:
            Console.WriteLine("1-- - Accounts By Amount\n2-- - Accounts By Date\n");
            input = Console.ReadLine();
            try
            {
                option = int.Parse(input);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto label1;
            }

            switch (option)
            {
                //Accounts by amount
                case 1:
                    int min = -1, max = -1;
              label2:
                    Console.Write("Enter the minimum amount: ");
                    input = Console.ReadLine();
                    try
                    {
                        min = int.Parse(input);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        goto label2;
                    }
            label3:
                    Console.Write("Enter the maximum amount: ");
                    input = Console.ReadLine();
                    try
                    {
                        max = int.Parse(input);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        goto label3;
                    }
                    bool flag = bll.customersExist();
                    if(flag == false)
                    {
                        Console.WriteLine("No data exists in files\n<--!!BYE!!-->");
                        quit();
                        showAdminMenu(); 
                    }
                    List<BO> bo = new List<BO>();
                    bll.ViewReports(ref bo);
                    Console.WriteLine("==== SEARCH RESULTS ======");
                    Console.WriteLine("Account ID  User ID  Type     Balance  Status   Holders Name ");
                    foreach (BO ob in bo)
                    {
                        if (min <= ob.Balance && ob.Balance <= max)
                            Console.WriteLine($"{ob.AccountNo}           {ob.userID}     {ob.Type}  {ob.Balance}   {ob.Status}  {ob.Name}");
                    }
                    quit();
                    showAdminMenu();
                    break;
                case 2:
                    //Accounts by Date
                    string dateInput;
                    DateTime startDate = new DateTime();
                    DateTime endDate = new DateTime(); 
                  label4:
                    Console.Write("Enter the starting date: ");
                    dateInput = Console.ReadLine();
                    try
                    {
                        startDate = DateTime.Parse(dateInput);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        goto label4;
                    }
                label5:
                    Console.Write("Enter the ending date: ");
                    dateInput = Console.ReadLine();
                    try
                    {
                        endDate = DateTime.Parse(dateInput);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        goto label5;
                    }
                    //reading transactions data from file
                    List<string> transactions = new List<string>();
                    transactions = bll.readTransactions();
                    //no transaction data in file
                    if(transactions.Count == 0 )
                    {
                        Console.Write("<--!!No transactions made yet!!-->");
                        quit();
                        showAdminMenu();
                    }
                    //transaction data found
                    Console.WriteLine("==== SEARCH RESULTS ======");
                    Console.WriteLine("Transaction Type        User ID   Amount   Date      Holders Name");
                    //matching and printing results
                    for (int i=0;i<transactions.Count;i++)
                    {
                        string[] data = transactions[i].Split(',');
                        if (startDate <= System.Convert.ToDateTime( data[5]) && System.Convert.ToDateTime(data[5]) <= endDate)
                            Console.WriteLine($"{data[1]}       {data[2]}        {data[4]}   {data[5]}  {data[3]}");
                    }
                    quit();
                    showAdminMenu();
                    break;
                default:
                    Console.WriteLine("<--!!You entered wrong input!!-->");
                    quit();
                    showAdminMenu();
                    break;
            }

        }
        //Administrator's functions finished

        public void printReceipt(char c, decimal amount, ref BO bo)
        {
            Console.WriteLine("Account #" + bo.AccountNo);
            DateTime date = new DateTime();
            Console.WriteLine("Date: " + date.Date + "\n");
            if (c == 'd')
                Console.WriteLine("Deposited: " + amount);
            else if (c == 'w')
                Console.WriteLine("Withdrawn: " + amount);
            else if (c == 't')
                Console.WriteLine("Amount Transferred: " + amount);
            Console.WriteLine("Balance: " + bo.Balance);
        }

        public void showCustomerMenu(ref BO customer)
        {
            Console.WriteLine("---CustomerMenu---");
        start:
            Console.Write("1----Withdraw Cash\n" + "2----Cash Transfer\n" + "3----Deposit Cash\n" +
                           "4----Display Balance\n" + "5----Exit\n" +
                           "Please select one of the above options: ");
            int? option = null;
            string input;
            //prompting the user
            input = Console.ReadLine();
            try
            {
                option = int.Parse(input);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto start;
            }
            //calling functions accordingly
            switch (option)
            {
                case 1:
                    withdrawCash(ref customer);
                    break;
                case 2:
                    transferCash(ref customer);
                    break;
                case 3:
                    depositCash(ref customer);
                    break;
                case 4:
                    displayBalance(ref customer);
                    break;
                case 5:
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please choose among [1,2,3,4,5] only");
                    goto start;

            }
        }

        public void withdrawCash(ref BO customer)
        {
            //prompting the user
            char option = 'o';
        start:
            Console.Write("a) Fast Cash\n" + "b) Normal Cash\n" +
                   "Please select a mode of withdrawal:");
            string input;
            input = Console.ReadLine();
            try
            {
                option = char.Parse(input);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto start;
            }
            if (option != 'a' && option != 'b')
            {
                Console.WriteLine("<--Invalid choice-->");
                goto start;
            }
            //calling functions accordingly
            if (option == 'a')
                fastCash(ref customer);
            else if (option == 'b')
                normalCash(ref customer);
        }

        public decimal fastCash(ref BO bo)
        {
            BLL bll = new BLL();
        //prompting the user
        start:

            Console.WriteLine("1----500\n2----1000\n3----2000\n" +
                "4----5000\n5----10000\n6----15000\n7----20000\n" +
                "Select one of the denominations of money: ");

            string input;
            int option = 0;
            input = Console.ReadLine();
            try
            {
                option = int.Parse(input);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto start;
            }
            decimal amount = -1;
            bool success = false;
            decimal withdrawn = 0;
            //getting the summ of all withdrawn amounts by the user in today's date
            withdrawn = bll.getWithdrawnAmountSum(ref bo);
            switch (option)
            {
                case 1:
                    Console.Write("Are you sure you want to withdraw Rs.500 (Y/N)? ");
                    input = Console.ReadLine();
                    if (input == "Y")
                    {
                        amount = 500;
                        if (bo.Balance < amount)
                        {
                            Console.WriteLine("You do not have enough Balance");
                            quit();
                        }
                        //checking today's withdraw limit
                        if (amount + withdrawn > 20000)
                        {
                            WriteLine("------------------S O R R Y-------------------------------\nYou are about to reach the limit of 20,000 withdrawal for today");
                            WriteLine($"You can only upto withdraw {20000 - withdrawn}");
                            quit(); showCustomerMenu(ref bo);
                        }
                            
                        //withdrawing amount
                        success = bll.fastCash(ref amount, ref bo);
                        if (success)
                            Console.Write("Cash Successfully Withdrawn!\n");
                        else
                            Console.WriteLine("You do not have enough Balance");
                    }
                    else
                        Console.WriteLine("Cash withdrawn was cancelled");
                    break;
                case 2:
                    Console.Write("Are you sure you want to withdraw Rs.1000 (Y/N)? ");
                    input = Console.ReadLine();
                    if (input == "Y")
                    {                        
                        amount = 1000;
                        if (bo.Balance < amount)
                        {
                            Console.WriteLine("You do not have enough Balance");
                            quit(); showCustomerMenu(ref bo);
                        }
                        //checking today's withdraw limit
                        if (amount + withdrawn > 20000)
                        {
                            WriteLine("------------------S O R R Y-------------------------------\nYou are about to reach the limit of 20,000 withdrawal for today");
                            WriteLine($"You can only upto withdraw {20000 - withdrawn}"); quit(); showCustomerMenu(ref bo);
                        }
                        //withdrawing amount
                        success = bll.fastCash(ref amount, ref bo);
                        if (success)
                            Console.Write("Cash Successfully Withdrawn!");
                        else
                            Console.WriteLine("You do not have enough Balance");
                    }
                    else
                        Console.WriteLine("Cash withdrawn was cancelled");
                    break;
                case 3:
                    Console.Write("Are you sure you want to withdraw Rs.2000 (Y/N)? ");
                    input = Console.ReadLine();
                    if (input == "Y")
                    {

                        amount = 2000;                        
                        if (bo.Balance < amount)
                        {
                            Console.WriteLine("You do not have enough Balance");
                            quit(); showCustomerMenu(ref bo);
                        }
                        //checking today's withdraw limit
                        if (amount + withdrawn > 20000)
                        {
                            WriteLine("------------------S O R R Y-------------------------------\nYou are about to reach the limit of 20,000 withdrawal for today");
                            WriteLine($"You can only upto withdraw {20000 - withdrawn}"); quit(); showCustomerMenu(ref bo);
                        }
                        //withdrawing amount
                        success = bll.fastCash(ref amount, ref bo);
                        if (success)
                            Console.Write("Cash Successfully Withdrawn!");
                        else
                            Console.WriteLine("You do not have enough Balance");
                    }
                    else
                        Console.WriteLine("Cash withdrawn was cancelled");
                    break;
                case 4:
                    Console.Write("Are you sure you want to withdraw Rs.5000 (Y/N)? ");
                    input = Console.ReadLine();
                    if (input == "Y")
                    {

                        amount = 5000;
                        if (bo.Balance < amount)
                        {
                            Console.WriteLine("You do not have enough Balance");
                            quit(); showCustomerMenu(ref bo);
                        }
                        //checking today's withdraw limit
                        if (amount + withdrawn > 20000)
                        {
                            WriteLine("------------------S O R R Y-------------------------------\nYou are about to reach the limit of 20,000 withdrawal for today");
                            WriteLine($"You can only upto withdraw {20000 - withdrawn}"); quit(); showCustomerMenu(ref bo);
                        }
                        //withdrawing amount
                        success = bll.fastCash(ref amount, ref bo);
                        if (success)
                            Console.Write("Cash Successfully Withdrawn!");
                        else
                            Console.WriteLine("You do not have enough Balance");
                    }
                    else
                        Console.WriteLine("Cash withdrawn was cancelled");
                    break;
                case 5:
                    Console.Write("Are you sure you want to withdraw Rs.10000 (Y/N)? ");
                    input = Console.ReadLine();
                    if (input == "Y")
                    {
                        //checking today's withdraw limit
                        amount = 10000;
                        if (bo.Balance < amount)
                        {
                            Console.WriteLine("You do not have enough Balance");
                            quit(); showCustomerMenu(ref bo);
                        }
                        if (amount + withdrawn > 20000)
                        {
                            WriteLine("------------------S O R R Y-------------------------------\nYou are about to reach the limit of 20,000 withdrawal for today");
                            WriteLine($"You can only upto withdraw {20000 - withdrawn}"); quit(); showCustomerMenu(ref bo);
                        }
                        //withdrawing amount
                        success = bll.fastCash(ref amount, ref bo);
                        if (success)
                            Console.Write("Cash Successfully Withdrawn!");
                        else
                            Console.WriteLine("You do not have enough Balance");
                    }
                    else
                        Console.WriteLine("Cash withdrawn was cancelled");
                    break;
                case 6:
                    Console.Write("Are you sure you want to withdraw Rs.15000 (Y/N)? ");
                    input = Console.ReadLine();
                    if (input == "Y")
                    {
                        amount = 15000;                        //checking today's withdraw limit
                        if (bo.Balance < amount)
                        {
                            Console.WriteLine("You do not have enough Balance");
                            quit(); showCustomerMenu(ref bo);
                        }
                        if (amount + withdrawn > 20000)
                        {
                            WriteLine("------------------S O R R Y-------------------------------\nYou are about to reach the limit of 20,000 withdrawal for today");
                            WriteLine($"You can only upto withdraw {20000 - withdrawn}");
                            quit();     showCustomerMenu(ref bo);
                        }
                        //withdrawing amount
                        success = bll.fastCash(ref amount, ref bo);
                        if (success)
                            Console.Write("Cash Successfully Withdrawn!");
                        else
                            Console.WriteLine("You do not have enough Balance");

                    }
                    else
                        Console.WriteLine("Cash withdrawn was cancelled");
                    break;
                case 7:
                    Console.Write("Are you sure you want to withdraw Rs.20000 (Y/N)? ");
                    input = Console.ReadLine();
                    if (input == "Y")
                    {
                        //checking today's withdraw limit
                        amount = 20000;
                        if (bo.Balance < amount)
                        {
                            Console.WriteLine("You do not have enough Balance");
                            quit(); showCustomerMenu(ref bo);
                        }
                        if (amount + withdrawn > 20000)
                        {
                            WriteLine("------------------S O R R Y-------------------------------\nYou are about to reach the limit of 20,000 withdrawal for today");
                            WriteLine($"You can only withdraw upto {20000 - withdrawn}"); quit(); showCustomerMenu(ref bo);
                        }
                        //withdrawing amount
                        success = bll.fastCash(ref amount, ref bo);
                        if (success)
                            Console.Write("Cash Successfully Withdrawn!");
                        else
                            Console.WriteLine("You do not have enough Balance");
                    }
                    else
                        Console.WriteLine("Cash withdrawn was cancelled");
                    break;
                default:
                    Console.WriteLine("<--Invalid choice-->");
                    goto start;

            }
            return -1;
        }

        public void normalCash(ref BO customer)
        {
            //prompting the user
        normalcashstart:
            Console.Write("Enter the withdrawal amount: ");
            string input;
            decimal amount;
            input = Console.ReadLine();
            try
            {
                amount = decimal.Parse(input);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto normalcashstart;
            }
            BLL bll = new BLL();
            decimal withdrawn = 0;
            //getting todays total withdrawn amount
            withdrawn = bll.getWithdrawnAmountSum(ref customer);
            if(customer.Balance < amount)
            {
                Console.WriteLine("You do not have enough Balance");
                quit();
                showCustomerMenu(ref customer);
            }
            //checking withdrawn limit
            if (amount + withdrawn > 20000)
            {
                WriteLine("------------------S O R R Y-------------------------------\nYou are about to reach the limit of 20,000 withdrawal for today");
                WriteLine($"You can only upto withdraw {20000 - withdrawn}");
                quit(); showCustomerMenu(ref customer);
            }
            //withdrawing cash
            bool success = bll.normalCash(amount,ref customer);
            if (success == true)
            {
                input = "";
                Console.Write("Cash Successfully Withdrawn!\n");
                Console.Write("Do you wish to print a receipt (Y/N)?  ");
                input = Console.ReadLine();
                if (input == "Y")
                    printReceipt('w', amount, ref customer);
                else if (input == "N")
                    quit();
                else
                    Console.WriteLine("<--Invalid choice-->");
                quit();
                
                
            }
            else if (success == false)
                Console.WriteLine("You do not have enough Balance");
        }//normal cash

        public void transferCash(ref BO customer)
        {
            //prompting the user
            string input;
            decimal amount = 0;
        transferstart:
            Console.Write("Enter amount in multiples of 500: ");
            input = Console.ReadLine();
            try
            {
                amount = decimal.Parse(input);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto transferstart;
            }
            if (amount % 500 != 0)
                goto transferstart;

            int account = -1;
        label1:
            Write("Enter the account number to which you want to transfer: ");
            input = ReadLine();
            try
            {
                account = int.Parse(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto label1;
            }
            //checking whether the account of reciever exists or not
            BLL bll = new BLL();
            bool exist = bll.isExist(account);
            if (exist == false)
            {
                Console.WriteLine("Account does not exist");
                return;
            }
            string name="";     int confirm = -1;   bool success = false;
            //get receiver name
            bll.getCustomerName(account, ref name);
            Write($" You wish to deposit {amount} in the account held by Mr " + name + " If this information is correct please re - enter the account number: ");
            input = ReadLine();
            try
            {
                confirm = int.Parse(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                quit();
                showCustomerMenu(ref customer);
            }
            //action confirmed
            if (account == confirm)
            {
                success = bll.transferCash(amount, ref customer, account);
                if (success == false)
                {
                    Console.WriteLine("You do notv have enough balance");
                    quit();
                    showCustomerMenu(ref customer);
                }
                else    //have enough balance
                {
                    Console.WriteLine("Transaction confirmed");
                    Console.Write("Do you wish to print a receipt (Y/N)?  ");
                    input = Console.ReadLine();
                    if (input == "Y")
                        printReceipt('t', amount, ref customer);
                    else if (input == "N")
                        return;
                    else
                        Console.WriteLine("<--Invalid choice-->");
                    return;
                }
            }
            else //wrong number entered at confirm
            {
                Console.WriteLine("You did not eneter the same account number!");
            }
           
        }//transfer cash

        public void depositCash(ref BO customer)
        {
            //prompting user
            string input;
            decimal amount = 0;
        start:
            Console.Write("Enter the cash amount to deposit: ");
            input = Console.ReadLine();
            try
            {
                amount = decimal.Parse(input);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto start;
            }
            BLL bll = new BLL();
            //depositing cash
            bll.depositCash(ref customer,ref amount);
            Console.WriteLine("Cash Deposited Successfully.");
           //ask for receipt
            Console.Write("Do you wish to print a receipt (Y/N)?  ");
            input = Console.ReadLine();
            if (input == "Y")
                 printReceipt('d', amount, ref customer);
            else if (input == "N")
            {
                quit();
                showCustomerMenu(ref customer);
            }
            
        }//deposit cash

        public void displayBalance(ref BO customer)
        {
            Console.WriteLine("Account #" + customer.AccountNo);
            DateTime date = new DateTime();
            Console.WriteLine("Date: " + date.Date + "\n");

            Console.WriteLine("Balance: " + customer.Balance);
            Console.WriteLine("--------------------------------------");
            quit();
            showCustomerMenu(ref customer);
        }

        public void quit()
        {
            Console.WriteLine("Press any key to return");
            string s=Console.ReadLine();
            Clear();
        }

    }//class view
}//namespace