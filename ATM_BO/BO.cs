using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_BO
{
    public class BO
    {
        public string adminLogin { get; set; }
        public char[] Login { get; set; }
        public char[] Pin { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public int userID { get; set; }
        public int AccountNo { get; set; }
        
        public DateTime firstWithdraw { get; set; }

        //non parameterized constructor
        public BO()    
        {

        }

        //parameterized constructor
        public BO(char[] login, char[] pin, string name, string type, decimal balance, string status, int account) 
        {
            Login = new char[login.Length];
            Login = login;
            
            Pin = new char[pin.Length];
            Pin = pin;
            Name = name;
            Type = type;
            Balance = balance;
            Status = status;
            AccountNo = account;
            userID = account * 1000 + (account * 100) / 11;
        }
        
    }
}