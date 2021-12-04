using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ATM_View;
namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("                 Welcome to                      ");
            Console.WriteLine("                    A T M                        ");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Enter the following credentials to login as an Administrator:\nLogin=admin\nPin=90900");
            View view = new View();
            view.GetInput();          //start the ATM
        }
    } 
}