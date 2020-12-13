using System;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;

using UnityEngine;
using DB;
using Data;

namespace SecAuth
{
    public class Authenticator : MonoBehaviour
    {
        private static System.Random randomNum = new System.Random();

        public static void Register(string firstName, string lastName, string password, User.UserType userType, string email, string homeSchool, List<string> schools) {
            password = PasswordEncryption(password);
            string verificationCode = GenerateVerificationCode();

            //Store user info into database
            Queries conn = new Queries();
            conn.insertUser(firstName, lastName, password, userType, verificationCode, email);

            //Storing USER-SCHOOL values if needed
            if (schools != null && schools.Count > 0) {
                Debug.Log("Will be added");
                foreach (string school in schools) {
                    Debug.Log(school);
                    //Store Username and School in USER-SCHOOL
                    conn.insertUserSchool(school, firstName, lastName);
                }
                
            } else {
                Debug.Log("No schools added");
            }

            //Send email with verification code
            string emailOfVerifier = null;
            if (userType == User.UserType.HighSchooler || userType == User.UserType.CollegeModerator) {
                // TODO get email of advisor (using homeschool to find them)
                emailOfVerifier = conn.getAdvisorEmail(homeSchool);
            } else {
                // TODO get email of developer (search for developer userType)
                conn.insertSchool(homeSchool, firstName, lastName);
                emailOfVerifier = conn.getDeveloperEmail();
            }

            conn.closeConenction();


            //Send email to advisor
            sendAuthenticationEmail(emailOfVerifier, homeSchool, firstName, lastName, email, verificationCode);
        }

        //TODO change back to private
        public static string PasswordEncryption(string password) {
            // TODO make an encryption system
            char[] charArr = password.ToCharArray();

            for(int i = 0; i < password.Length; i++) {
                charArr[i] = (char)(((int)charArr[i]*997 + 1) % 256);
                if ((int)charArr[i] == 39 || (int)charArr[i] == 34) {
                    charArr[i] = (char)((int)charArr[i]+1);
                }
            }
            return new string(charArr);
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

            if (verificationCodeFromDB.Equals(verificationCode) && (conn.getUser(firstName, lastName).IsVerifiableBy(SceneInstanceControl.User))) {
                //Update verification code in database
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
            
            password = PasswordEncryption(password);
            
            if (passFromDB.Equals(password)) {
                Debug.Log("PASSWORD MATCHED!!!: " + passFromDB + " same as given " + passFromDB);
                //Get that user
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