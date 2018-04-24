using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ChainOfResponsibility;
using AO.PasswordValidation;

namespace PlayGround
{
    class Program
    {
        static void Main(string[] args)
        {
            PasswordValidationBusiness pvb = new PasswordValidationBusiness();
            pvb.ValidatePassword("", "", PasswordType.LoginPassword);
            
        }

        private static bool CheckPasswordStrength(string password)
        {
            string weakPasswordPattern = "0123456789qwertyuiopasdfghjklzxcvbnmabcdefghijklmnopqrstuvwxyz0987654321poiuytrewqlkjhgfdsamnbvcxzzyxwvutsrpqonmlkjihgfedcbapasswordaaaabbbbccccddddeeeeffffgggghhhhiijjjjkkkkllllmmmmnnnnooooppppqqqqrrrrssssttttuuuuvvvvwwwwxxxxyyyyzzzz";
            int minimumPattern = 3;
            string divide = string.Join("|", Enumerable.Range(0, weakPasswordPattern.Length - minimumPattern + 1).Select(i => Regex.Escape(string.Concat(weakPasswordPattern.Skip(i).Take(minimumPattern)))));
            string[] result = divide.Split('|');
            bool isPasswordStrong = true;

            for (int i = 0; i < result.Length; i++)
            {
                Regex rgx = new Regex(result[i]);
                foreach (Match match in rgx.Matches(password))
                    isPasswordStrong = false;
            }
            return isPasswordStrong;
        }

        private void CoR()
        {
            // Build the chain of responsibility
            Logger logger, logger1, logger2;
            logger = new ConsoleLogger(LogLevel.All);
            logger1 = logger.SetNext(new EmailLogger(LogLevel.FunctionalMessage | LogLevel.FunctionalError));
            logger2 = logger1.SetNext(new FileLogger(LogLevel.Warning | LogLevel.Error));

            // Handled by ConsoleLogger since the console has a loglevel of all
            logger.Message("Entering function ProcessOrder().", LogLevel.Debug);
            logger.Message("Order record retrieved.", LogLevel.Info);

            // Handled by ConsoleLogger and FileLogger since filelogger implements Warning & Error
            logger.Message("Customer Address details missing in Branch DataBase.", LogLevel.Warning);
            logger.Message("Customer Address details missing in Organization DataBase.", LogLevel.Error);

            // Handled by ConsoleLogger and EmailLogger as it implements functional error
            logger.Message("Unable to Process Order ORD1 Dated D1 For Customer C1.", LogLevel.FunctionalError);

            // Handled by ConsoleLogger and EmailLogger
            logger.Message("Order Dispatched.", LogLevel.FunctionalMessage);

            Console.ReadKey();
        }


    }
}
