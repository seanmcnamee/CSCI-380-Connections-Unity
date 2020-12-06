using System;
using System.Net.Mail;
using System.Net;

using UnityEngine;

namespace SecAuth
{
    class Authentication : MonoBehaviour
    {
        private static System.Random randomNum = new System.Random();

        public static void Register(string firstName, string lastName, string password, User.UserType userType, string email, string homeSchool, string[] schools=null) {
            password = PasswordEncryption(password);
            string verificationCode = GenerateVerificationCode();
            //TODO make sure the email is an actual email
            
            // TODO Store user info into database


            //Storing USER-SCHOOL values if needed
            if (schools != null) {
                foreach (string school in schools) {
                    // TODO Store Username and School in USER-SCORE

                }
            }

            //Send email with verification code
            string emailOfVerifier = null;
            if (userType == User.UserType.HighSchooler || userType == User.UserType.CollegeModerator) {
                // TODO get email of advisor (using homeschool to find them)
                
            } else {
                // TODO get email of developer (search for developer userType)

            }
            //Send email to advisor
            sendAuthenticationEmail(emailOfVerifier, homeSchool, firstName, lastName, email, verificationCode);
        }

        private static string PasswordEncryption(string password) {
            // TODO make an encryption system
            return password;
        }

        private static string GenerateVerificationCode() {
            //Want a n digit code
            int n = 6;
            int numNumberCode = randomNum.Next((int) Math.Pow(10, n-1), (int) Math.Pow(10, n)-1);
            return numNumberCode.ToString();
        }

        public static void sendAuthenticationEmail(string toEmail, string homeSchool, string firstName, string lastName, string newUserEmail, string verificationCode) {
            //Thanks to docs.microsoft.com
            string subject = "College Connections Verification Code";
            string body = "Hello " + homeSchool + " Advisor,\n\n" +
            "This user needs verification: " + firstName + " " + lastName +  
            "\nTheir email is: " + newUserEmail +
            "\nThis is their verification code: " + verificationCode;

            EmailSender.SendEmailTo(toEmail, subject, body);
        }

        public static bool VerifyAccount(string firstName, string lastName, string verificationCode) {
            // TODO grab verification code from Database
            string verificationCodeFromDB = "test";
            if (verificationCodeFromDB.Equals(verificationCode)) {
                // TODO Update verification code in database
                string verifiedString = User.verifiedString;

                return true;
            }
            return false;
        }

        public static User Login(string username, string password) {
            // TODO grab from Database
            string passFromDB = "test";

            if (PasswordEncryption(passFromDB).Equals(password)) {
                // TODO grab the user info from the Database
                string userName = null;
                User.UserType userType = User.UserType.CollegeModerator;

                string isVerified = null;
                string email = null;
                
                // TODO grab all the schools from the database
                string[] schoolNames = null;

                User newUser = new User(userName, userType, isVerified, email, schoolNames);

                SceneInstanceControl.User = newUser;
                return newUser;
            }
            return null;
        }
    }
}