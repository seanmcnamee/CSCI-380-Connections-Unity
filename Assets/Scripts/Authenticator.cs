using System;
using System.Net.Mail;
using System.Net;

using UnityEngine;
using DB;
using Data;

namespace SecAuth
{
    public class Authenticator : MonoBehaviour
    {
        private static System.Random randomNum = new System.Random();

        public static void Register(string firstName, string lastName, string password, User.UserType userType, string email, string homeSchool, string[] schools=null) {
            password = PasswordEncryption(password);
            string verificationCode = GenerateVerificationCode();

            //Store user info into database
            Queries conn = new Queries();
            conn.insertUser(firstName, lastName, password, userType, verificationCode, email);
            

            //Storing USER-SCHOOL values if needed
            if (schools != null) {
                foreach (string school in schools) {
                    //Store Username and School in USER-SCHOOL
                    conn.insertUserSchool(school, firstName, lastName);
                }
            }

            //Send email with verification code
            string emailOfVerifier = null;
            if (userType == User.UserType.HighSchooler || userType == User.UserType.CollegeModerator) {
                // TODO get email of advisor (using homeschool to find them)
                emailOfVerifier = conn.getAdvisorEmail(homeSchool);
            } else {
                // TODO get email of developer (search for developer userType)
                emailOfVerifier = conn.getDeveloperEmail();
            }

            conn.closeConenction();


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

        public bool IsValidEmail(string email) {
            MailAddress address;
            try {
                address = new MailAddress(email);
                return (address.Address == email);
            } catch {
               return false; 
            }
        }

        public static bool VerifyAccount(string firstName, string lastName, string verificationCode) {
            Queries conn = new Queries();

            //grab verification code from Database
            string verificationCodeFromDB = conn.getVerification(firstName, lastName);
            if (String.IsNullOrEmpty(verificationCodeFromDB)) {
                return false;
            }

            if (verificationCodeFromDB.Equals(verificationCode)) {
                // TODO Update verification code in database
                conn.setVerified(firstName, lastName);
                
                conn.closeConenction();
                return true;
            }

            conn.closeConenction();
            return false;
        }

        public static User Login(string firstName, string lastName, string password) {
            //Grab password from Database
            Queries conn = new Queries();
            string passFromDB = conn.getPassword(firstName, lastName);
            if (String.IsNullOrEmpty(passFromDB)) {
                return null;
            }
            
            if (PasswordEncryption(passFromDB).Equals(password)) {
                Debug.Log("PASSWORD MATCHED!!!: " + passFromDB + " same as given " + password);
                User user = conn.getUser(firstName, lastName);
                
                conn.closeConenction();
                return user;
            }
            conn.closeConenction();
            Debug.Log("PASSWORD: " + passFromDB + " didn't match :(");
            return null;
        }
    }
}