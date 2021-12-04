using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM_BO;
using ATM_DAL;
using static System.Console;
namespace ATM_BLL
{
    public class BLL
    {

        public int createAccount(char[] login, ref char[] pin,string name, string type, decimal balance, string status)
        {
            
            DAL dal = new DAL();
            List<BO> bo = new List<BO>();
            //reading customers records form files
            bo= dal.ReadCustomers();            
            BO newCustomer;
            int accountNo = 0;
            //assign account no = 1 if no customer exists already
            if (bo == null)
            {
                newCustomer = new BO(login, pin, name, type, balance, status, 1);
                accountNo = 1;
            }
            //assign new account number as asked
            else
            {
                newCustomer = new BO(login, pin, name, type, balance, status, bo[bo.Count - 1].AccountNo + 1);
                accountNo = bo[bo.Count - 1].AccountNo + 1;
            }
            //save changes in iles
            dal.SaveCustomer(newCustomer);
            return accountNo;
        }//create account

        public void deleteAccount(ref int account)
        {
            DAL dal = new DAL();
            List<BO> bo = new List<BO>();
            //reading customers records form files
            bo = dal.ReadCustomers();
            int index = -1;
            //get index of desired account
            foreach (BO b in bo)
            {
                index++;
                if (b.AccountNo == account)
                {
                    break;
                }
            }
            //delete the record at that index and save changes
            bo.RemoveAt(index);
            dal.updateCustomers(bo);

        }//delete account

        public void updateAccount(int account, char[] login, char[] pin,  string name, string type, string status)
        {
            DAL dal = new DAL();
            List<BO> bo = new List<BO>();
            //reading customers records form files
            bo = dal.ReadCustomers();
            foreach (BO b in bo)
            {
                //creating the customer object satisfying update quey
                if (b.AccountNo == account)
                {
                    if (login != null) b.Login = login;
                    if (pin != null) b.Pin = pin;
                    if (name != "") b.Name = name;
                    if (type != "") b.Type = type;
                    if (status != "") b.Status = status;
                }
            }
            dal.updateCustomers(bo);
        }//update account

        public void searchAccount(ref string[] input,int account, int userID,string name,string type, decimal balance,string status,ref List<BO> bo, ref List<BO> dummy)
        {
            DAL dal = new DAL();
            bo = dal.ReadCustomers();
        //create a dummy object according to search criteria
            dummy = bo;
            if(input[0] != string.Empty)
            {
                foreach (BO b in dummy)
                    b.AccountNo = account;
            }
            
            else if (input[0] == string.Empty)
            {
                for (int j = 0; j < dummy.Count; j++)
                    dummy[j].AccountNo = bo[j].AccountNo;
                
            }
            if (input[1] != string.Empty)
                foreach (BO b in dummy)
                    b.userID = userID;
            else
            {
                for (int j = 0; j<dummy.Count; j++)
                    dummy[j].userID = bo[j].userID;
                
            }
            if (input[2] != string.Empty)
            {
                foreach (BO b in dummy)
                {
                    b.Name = name;
                }
            }
            else
            {
                for (int j = 0; j < dummy.Count; j++)
                {
                    dummy[j].Name = bo[j].Name;
                }

            }
            if (input[3] != string.Empty)
                foreach (BO b in dummy)
                    b.Type = type;
            else
            {
                for (int j = 0; j<dummy.Count; j++)
                {
                    dummy[j].Type = bo[j].Type;
                }

            }
            if (input[4] != string.Empty)
            {
                foreach (BO b in dummy)
                    b.Balance = balance;
            }
            else
            {
                for (int j = 0; j<dummy.Count; j++)
                {
                    dummy[j].Balance = bo[j].Balance;
                }

            }
            if (input[5] != string.Empty)
                foreach (BO b in dummy)
                    b.Status = status;
            else
            {
                for (int j = 0; j < dummy.Count; j++)
                {
                    dummy[j].Status = bo[j].Status;
                }

            }
            
            bo = dal.ReadCustomers();
           
        }//search account

        public void ViewReports(ref List<BO> bo)
        {
            DAL dal = new DAL();
            bo = dal.ReadCustomers();

        }
        public List<string> readTransactions()
        {
            DAL dal = new DAL();
            List<string> s =dal.readTransactions();
            return s;
        }

        //--------------------M E N U------------------------------
        public bool matchAdminPin(char[] pin, string login)
        {
            bool pinIsWrong;
            DAL dal = new DAL();
            char[] storedPIN = dal.readAdminPin();
            if (storedPIN == null)   //admn logins first time
            {
                pinIsWrong = false;
                BO bo = new BO();
                bo.adminLogin = login;
                bo.Pin = pin;
                dal.SaveAdmin(bo);
                return false;     
            }
            else     //admin logins afterwards
            {
                int i = 0;
                int c = 0;
                for(;i<pin.Length;i++)
                    if(pin[i] == storedPIN[i])
                        c++;
                
                if (c == pin.Length)  //pin matches
                {
                    return false;
                }

                else  //wrong pin
                {
                    return true;                    
                }
            }
        }

        public void getCustomer(ref int account, ref BO customer)
        {
            List<BO> businessOBJ = new List<BO>();
            readCustomers(ref businessOBJ);
            foreach (BO b in businessOBJ)
            {
                if (b.AccountNo == account)
                {
                    customer = b;
                }
            }
        }
        //return true if customers records exist in file
        public bool customersExist()
        {
            List<BO> businessOBJ = new List<BO>();
            readCustomers(ref businessOBJ); 
            if (businessOBJ == null)
                return false;
            else
                return true;
        }
        //return true if customer exist in file
        public bool isExist(int account)
        {
            List<BO> businessOBJ = new List<BO>();
            readCustomers(ref businessOBJ);
            foreach (BO b in businessOBJ)
            {
                if (b.AccountNo == account)
                {
                    return true;
                }
            }
            return false;
        }

        public void getCustomerName(int account, ref string name)
        {
            List<BO> businessOBJ = new List<BO>();
            readCustomers(ref businessOBJ);
            foreach (BO b in businessOBJ)
            {
                if (b.AccountNo == account)
                {
                    name = b.Name;
                }
            }
        }

        public void printReceipt(char c, decimal amount, ref BO bo)
        { 
        }
        ///////////////////C U S T O M E R  f u n c t i o n s///////////////////
        public bool normalCash(decimal amount,ref BO bo)
        {
            DAL dal = new DAL();
            List<BO> bolist = new List<BO>();
            bolist = dal.ReadCustomers();
            int customerIndex = -1;
            int index = -1;
            foreach (BO b in bolist)
            {
                index++;
                if (b.AccountNo == bo.AccountNo)
                {
                    customerIndex = index;
                }

            }
            //if balance is enough then withdraw
            if (bo.Balance >= amount)
            {
                bo.Balance -= amount;
                bolist[customerIndex].Balance -= amount;
                dal.updateCustomers(bolist);
                string type = "Cash Withdrawal";
                dal.saveTransaction(ref bo, ref amount, ref type);
                return true;
            }
            //not enough balance
            else
                return false;

        }//Normal cash
        
        public bool fastCash(ref decimal amount,ref BO bo)
        {
            
            DAL dal = new DAL();
            List<BO> bolist = new List<BO>();
            bolist = dal.ReadCustomers();
            int customerIndex = -1;
            int index = -1;
            foreach (BO b in bolist)
            {
                index++;
                if (b.AccountNo == bo.AccountNo)
                {
                    customerIndex = index;
                }

            }
            //if balance is enough then withdraw
            if (bo.Balance >= amount)
            {
                bo.Balance -= amount;
                bolist[customerIndex].Balance -= amount;
                dal.updateCustomers(bolist);
                string type = "Cash Withdrawal";
                dal.saveTransaction(ref bo, ref  amount,ref type);
                return true;
            }
            else//not enough cash
            {
                return false;
            }

        }//fast cash
        public void withdrawCash(ref BO bo,char option)
        {

            
        }//withdraw func

        public bool transferCash(decimal amount,ref BO customer,int account)
        {

            DAL dal = new DAL();
            List<BO> bolist = new List<BO>();
            bolist = dal.ReadCustomers();
            if (customer.Balance < amount)
            {
                return false;

            }
            else    //have enough balance
            {
                //update the balance of sender and receiver
                foreach (BO b in bolist)
                {
                    if (b.AccountNo == customer.AccountNo)
                        b.Balance -= amount;
                    if (b.AccountNo == account)
                        b.Balance += amount;
                }
                customer.Balance -= amount;
                dal.updateCustomers(bolist);
                string type = "Cash Transfer";
                dal.saveTransaction(ref customer, ref amount, ref type);
                return true;
            }  
        }

        public void depositCash(ref BO bo,ref decimal amount)
        {
           
            DAL dal = new DAL();
            List<BO> bolist = new List<BO>();
            bolist = dal.ReadCustomers();
            int customerIndex = -1;
            int index = -1;
            foreach (BO b in bolist)
            {
                index++;

                if (b.AccountNo == bo.AccountNo)
                {
                    customerIndex = index;
                }
            }            
            bo.Balance += amount;
            bolist[customerIndex].Balance += amount;
            dal.updateCustomers(bolist);
            string type = "Cash Deposit";
            dal.saveTransaction(ref bo, ref amount, ref type);
        }

        public void displayBalance(ref BO bo)
        {
        }
        public void showCustomerMenu(ref BO x)
        {
        }
        public bool matchCustomerLogin(ref BO bo,char[] login)
        {
            List<BO> businessOBJ = new List<BO>();
            readCustomers(ref businessOBJ);
            int i = 0,match=0;
            foreach (BO bobj in businessOBJ)
            {
                match = 0;
                i = 0;
                if(login.Length == bobj.Login.Length)
                {
                    for (; i < bobj.Login.Length; i++)
                    {
                        if (login[i] == bobj.Login[i])
                            match++;
                    }
                    //login matches
                    if (match == login.Length)
                    {
                        bo = bobj;
                        return true;
                    }
                }
                
            }
            //login doesnt match any customer
            return
                false;
        }
        //returns the status of a custimer account from file
        public char getStatus(char[] login, ref BO obj)
        {
            List<BO> businessOBJ = new List<BO>();
            readCustomers(ref businessOBJ);
            foreach (BO bobj in businessOBJ)
            {
                if (login.SequenceEqual(bobj.Login) )
                {
                    if (bobj.Status == "Activate")
                    {
                        obj = bobj;
                        return 'a';
                    }
                    else if (bobj.Status == "Deactivated")
                        return 'd';

                }
            }
            return 'z';
        }
        //deactivate a custmer account and save changes
        public void deactivateAndSave(ref BO obj)
        {
            DAL dataOBJ = new DAL();
            List<BO> businessOBJ = dataOBJ.ReadCustomers();
            foreach (BO bobj in businessOBJ)
            {
                if (obj.AccountNo == bobj.AccountNo)
                {
                    bobj.Status = "Deactivated";
                    WriteLine(bobj.Status);
                }
            }
            dataOBJ.updateCustomers(businessOBJ);
        }

        public bool matchPin(ref char[] pin, ref BO bo)          //returns true if enterd pin by user matches the saaved pin , else false
        {
            if (pin == null)
                return false;
            List<BO> businessOBJ = new List<BO>();
            readCustomers(ref businessOBJ);
            int i, match;
            foreach (BO bobj in businessOBJ)
            {
                match = 0;
                i = 0;
                if (pin.Length == bobj.Pin.Length)
                {
                    for (; i < bobj.Pin.Length; i++)
                    {
                        if (pin[i] == bobj.Pin[i])
                            match++;
                    }
                    //pin matched
                    if (match == pin.Length)
                    {
                        return true;
                    }
                }
            }
            //pin not matched
            return false;
        }

        public void readCustomers(ref List<BO> list)
        {
            DAL dataOBJ = new DAL();
            list = dataOBJ.ReadCustomers();
        }

        public decimal getWithdrawnAmountSum(ref BO bo)
        {
            List<string> transactions = new List<string>();
            transactions = readTransactions();
            if (transactions.Count == 0)
            {
                //<--!!No transactions made yet!!-->
                return 0;
            }
            int i;
            decimal x;
            string s,p;
            decimal sum = 0;
            //making the date format as asked
            string date = string.Format(          
                format: "{0}/{1}/{2}",
                arg0: DateTime.Now.Month,
                arg1: DateTime.Now.Day,
                arg2: DateTime.Now.Year
                );
            
            for (i = 0; i < transactions.Count; i++)
            {
                string[] data = transactions[i].Split(',');
                if(bo.AccountNo.ToString() == data[0] && data[1] == "Cash Withdrawal")
                {
                    s = data[4];
                    x=decimal.Parse(s);
                    p = data[5];
                    if (date.Equals(p))
                    {
                       sum += x;
                    }
                        
                }
            }
            return sum;
        }
    }//class
}//namespace