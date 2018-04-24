using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace PasswordValidation
{
    
    //TODO: Client Class must log on its own; code refactor on handling the response of the only public function of this class.
    //TODO: Inheritance; e.g. BaseClass, Interface, etc.
    public class PasswordValidationBusiness
    {
        //CONSTRUCTOR(S)

        enum PasswordType : int
        {
            LoginPassword = 1,
            TransactionPassword = 2
        };
        public PasswordValidationBusiness()
        {   

        }

        /// <summary>
        /// Checks the validity of the password using pre-defined set of criteria. Returns an Object that contains the Code, Status, and Message of Validation.
        /// </summary>
        /// <param name="username">Login Username</param>
        /// <param name="password">Raw(Unencrypted) Password</param>
        /// <returns>Returns an Object that contains the Code, Status, and Message for Password Validation.</returns>
        public PasswordValidationOutput ValidatePassword(string username, string password)
        {
            PasswordValidationOutput passwordValidationOutput = new PasswordValidationOutput();
            passwordValidationOutput.Code = "0";
            passwordValidationOutput.Status = "1";
            passwordValidationOutput.Message = "Password is Valid.";
            
            //NOTE: All Function Names Used Positive Words:
            //e.g. Strong instead of Weak, Contain instead of NotContaining, Valid instead of Invalid
            //TODO: Each Validation MUST have a Code of its own (For Error Message Reference). Add new entries in the database for these Error Messages.
            if (IsPasswordContainingUsername(username, password))
            {
                //TODO: Set Values for Output if IsPasswordContainingUsername triggers
            }
            else if (IsPasswordEqualToUserName(username, password))
            {
                //TODO: Set Values for Output if IsPasswordEqualToUserName triggers
            }
            else if (!IsPasswordFormatValid(username, password))
            {
                //TODO: Set Values for Output if !IsPasswordFormatValid triggers
            }
            else if (!IsPasswordStrong(username, password))
            {
                //TODO: Set Values for Output if !IsPasswordStrong triggers
            }
            return passwordValidationOutput; 
        }



        //PASSWORD VALIDITY FUNCTIONS
        //TRY TO AVOID CREATING MULTIPLE FUNCTIONS WITH A REGEX
        
        private bool IsPasswordContainingUsername(string username, string password)
        {
            //Previous (since 2014) Logic goes like this: input.LogInNewPassword.Contains(input.UserId)
            return password.ToLower().Contains(username.ToLower());
        }
        
        private bool IsPasswordEqualToUserName(string username, string password)
        {
            return password.ToLower() == username.ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool IsPasswordFormatValid(string username, string password)
        {
            //Note, there is already a function for checking if the password contains the username, this checking is redundant.
            if (!(password.Contains(username) && !(password.Contains(" "))))
            {
                if (password.Length > 7 && password.Length < 29)
                {
                    string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
                    Match _match = Regex.Match(password, pattern);
                    return _match.Success;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks the Strength of the Password based on patterns like "123". Returns True if the password is strong, returns false otherwise.
        /// </summary>
        /// <param name="password">Raw Password Text(Not Encrypted).</param>
        /// <returns></returns>
        private bool IsPasswordStrong(string username, string password)
        {
            //TODO: Retrieve the minimumPatternCount
            //TODO: Retrieve the weakPasswordPatternGestalt
            bool isPasswordStrong = true;
            int minimumPatternCount = 3;
            string weakPasswordPatternGestalt = "0123456789qwertyuiopasdfghjklzxcvbnmabcdefghijklmnopqrstuvwxyz0987654321poiuytrewqlkjhgfdsamnbvcxzzyxwvutsrpqonmlkjihgfedcbapasswordaaaabbbbccccddddeeeeffffgggghhhhiijjjjkkkkllllmmmmnnnnooooppppqqqqrrrrssssttttuuuuvvvvwwwwxxxxyyyyzzzz";
            //Creates an array of weak patterns out of the Gestalt.
            //For example, if the Gestalt is "12345" and the minimumPatternCount is 3...
            //A string array with following items will be created: "123", "234", "345"
            string patternBar = string.Join("|", Enumerable.Range(0, weakPasswordPatternGestalt.Length - minimumPatternCount + 1).Select(i => Regex.Escape(string.Concat(weakPasswordPatternGestalt.Skip(i).Take(minimumPatternCount)))));
            string[] weakPasswordPatternList = patternBar.Split('|');

            for (int i = 0; i < weakPasswordPatternList.Length; i++)
            {
                Regex rgx = new Regex(weakPasswordPatternList[i]);
                foreach (Match match in rgx.Matches(password))
                    isPasswordStrong = false;
            }
            return isPasswordStrong;
        }

    }

    public class PasswordValidationOutput
    {
        public string Code{ get ; set; }
        public string Status { get; set; }
        public string Message { get; set; }

    }
}