using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ATM_BO;
namespace ATM_DAL
{
    public class DAL
    {
        public void encryptDecrypt(ref char[] cryptedPin, char[] pin)     //Encrypt and Decrypt the Login and Pin
        {
            int i = 0;
            try
            {
                for (; i < pin.Length; i++)
                {
                    if (pin[i] >= '0' && pin[i] <= '9')       //handles digits
                    {
                        cryptedPin[i] = ((char)(pin[i] + ('9' - pin[i]) - (pin[i] - '0')));

                    }
                    else if (pin[i] >= 'A' && pin[i] <= 'Z')  //handles Uppercase Alphabets
                    {
                        cryptedPin[i] = System.Convert.ToChar(pin[i] + ('Z' - pin[i]) - (pin[i] - 65));
                    }
                    else if (pin[i] >= 'a' && pin[i] <= 'z')   //handles Lowercase Alphabets
                    {
                       cryptedPin[i] = Convert.ToChar(pin[i] + ('z' - pin[i]) - (pin[i] - 97));
                    }       
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }

        }

        public void SaveAdmin(BO bo)
        {
            try
            {
                char[] encryptedPin = new char[5];
                encryptDecrypt(ref encryptedPin, bo.Pin); //got encrypted pin in encryptedPin variable
                char[] login = new char[bo.adminLogin.Length];
                int i = 0;
                for (; i < bo.adminLogin.Length; i++)
                    login[i] = bo.adminLogin[i];
                char[] encryptedLogin = new char[bo.adminLogin.Length];
                encryptDecrypt(ref encryptedLogin, login); //got encrypted login in encryptedPin variable
                string filePath = Path.Combine(Environment.CurrentDirectory, "AdminData.txt");
                StreamWriter sw = new StreamWriter(filePath, append: false);
                sw.Write(encryptedLogin);
                sw.Write(',');
                sw.WriteLine(encryptedPin);
                sw.Close();                               //written the login and pin into the file
            }
            catch(Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        public char[] readAdminPin()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "AdminData.txt");
            StreamReader sr = new StreamReader(filePath);
            string line = String.Empty;
            try
            {
                if (sr.ReadLine() == null)         //file is empty
                {
                    sr.Close();
                    return null;
                }
                filePath = Path.Combine(Environment.CurrentDirectory, "AdminData.txt");
                sr = new StreamReader(filePath);
                line = sr.ReadLine();
                string[] data = line.Split(',');
                char[] Pin = new char[5];
                char[] storedPin= new char[5];
                int i=0;
                for (; i < data[1].Length; i++)
                    storedPin[i] = data[1][i];            //getting saved pin
               
                encryptDecrypt(ref Pin, storedPin);       //decrypting the saved pin
                sr.Close();
                return Pin;
            }
            catch(Exception ex)
            {
                sr.Close();
                Console.WriteLine(ex.Message + "while reading admin pin");
                return null;
            }
        }
        public void SaveCustomer(BO bo)                            //saves a single customer in file
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "CustomerData.csv");
            StreamWriter sw = new StreamWriter(filePath, append: true);
            try
            {
                char[] encryptedPin = new char[5];
                encryptDecrypt(ref encryptedPin, bo.Pin);              //encrypting the pin
                string text = $"{bo.AccountNo},{bo.userID},{bo.Name},{bo.Type},{bo.Balance},{bo.Status},";
                char[] encryptedLogin = new char[bo.Login.Length];
                encryptDecrypt(ref encryptedLogin, bo.Login);        //encrypting the login

                sw.Write(text);
                sw.Write(encryptedLogin);
                sw.Write(',');
                sw.WriteLine(encryptedPin);                          //writing to file
                sw.Close();
            }
            catch(Exception ex)
            {
                sw.Close();
                WriteLine(ex.Message);
            }
        }
        public void updateCustomers(List<BO> bo)                    //updates all customers in the file
        {
            string text; string filePath;
            filePath = Path.Combine(Environment.CurrentDirectory, "CustomerData.csv");
            StreamWriter sw = new StreamWriter(filePath, append: false);
            char[] encryptedPin = new char[5];
            char[] encryptedLogin = null;
            try
            {
                foreach (BO b in bo)
                {
                    if (b.Pin != null)
                        encryptDecrypt(ref encryptedPin, b.Pin);                //encrypting the pin of each customer
                                                                                //else
                                                                                //    encryptDecrypt(ref encryptedPin,)
                    encryptedLogin = new char[b.Login.Length];
                    if (b.Login != null)
                        encryptDecrypt(ref encryptedLogin, b.Login);            //encrypting the Login of each customer
                    text = $"{b.AccountNo},{b.userID},{b.Name},{b.Type},{b.Balance},{b.Status},";
                    sw.Write(text);
                    sw.Write(encryptedLogin);
                    sw.Write(',');
                    sw.WriteLine(encryptedPin);                             //writing to file
                }
                sw.Close();
            }
            catch(Exception ex)
            {
                sw.Close();
                WriteLine(ex.Message);
            }
        }
        internal List<string> Read(string fileName)
        {

            List<string> list = new List<string>();
            string filePath = Path.Combine(Environment.CurrentDirectory,
                fileName);
            StreamReader sr = new StreamReader(filePath);
            
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                list.Add(line);
            }
            sr.Close();
            return list;
        }

        public List<BO> ReadCustomers()                       //returns the list of all custmer records from the file
        {
            List<String> stringList = Read("CustomerData.csv");
            if (stringList.Count == 0)                       //file is empty
            {
                return null;

            }
            List<BO> customerList = new List<BO>();
            try
            {
                foreach (string s in stringList)
                {
                    string[] data = s.Split(',');
                    BO bo = new BO();                                                     //reading form file
                    bo.AccountNo = System.Convert.ToInt32(data[0]);
                    bo.userID = System.Convert.ToInt32(data[1]);
                    bo.Name = data[2];
                    bo.Type = data[3];
                    bo.Balance = System.Convert.ToDecimal(data[4]);
                    bo.Status = data[5];
                    char v; int i = 0;
                    bo.Login = new char[data[6].Length];
                    for (; i < data[6].Length; i++)
                    {
                        v = (char)data[6][i];
                        bo.Login[i] = v;
                    }
                    bo.Pin = new char[5];
                    i = 0;
                    for (; i < 5; i++)
                    {
                        v = (char)data[7][i];
                        bo.Pin[i] = v;
                    }

                    char[] decryptedPin = new char[5];
                    encryptDecrypt(ref decryptedPin, bo.Pin);                    //decrypting the saved pin
                    i = 0;

                    bo.Pin = decryptedPin;
                    char[] decryptedLogin = new char[bo.Login.Length];
                    encryptDecrypt(ref decryptedLogin, bo.Login);               //decrypting the saved Login
                    bo.Login = decryptedLogin;
                    customerList.Add(bo);                                        //adding in the List
                }
                return customerList;
            }
            catch (Exception ex)
            {
                Write(ex.Message);
            }
            return customerList;
        }
        public void saveTransaction(ref BO bo,ref decimal amount,ref string type)    //save transactions into the file
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Transactions.txt");
            StreamWriter sw = new StreamWriter(filePath, append: true);    //writing to file
            try
            {
                string date = string.Format(                          //making the date format as asked
                format: "{0}/{1}/{2}",
                arg0: DateTime.Now.Month,
                arg1: DateTime.Now.Day,
                arg2: DateTime.Now.Year
                );
                string text = $"{bo.AccountNo},{type},{bo.userID},{bo.Name},{amount},{date}";

                sw.WriteLine(text);
                sw.Close();
            }
            catch(Exception ex)
            {
                sw.Close();
                Write(ex.Message);
            }
            
        }
        public List<string> readTransactions()                       //read all transactions information from the file into the list
        {
            List<string> list = new List<string>();
            string filePath = Path.Combine(Environment.CurrentDirectory, "Transactions.txt");
            StreamReader sr = new StreamReader(filePath);
            try
            {
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {

                    list.Add(line);
                }
                sr.Close();
                return list;
            }
            catch(Exception ex)
            {
                sr.Close();
                Write(ex.Message);
                return list;
            }

        }
    }
  
}