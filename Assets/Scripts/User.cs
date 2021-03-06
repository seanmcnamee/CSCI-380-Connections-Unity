using System;
using System.Collections.Generic;

using SecAuth;

namespace Data
{
    public class User
    {
        public static string verifiedString = "Verified";
        private string chatrooms = "Select chat room";

        public enum UserType {
            HighSchooler, CollegeModerator, Advisor, Developer
        }

        public string userName {get;}
        private UserType userType {get;}
        private string isVerified;
        private string email;
        public List<string> schoolNames { get;}
        public List<string> messages { get;}

        public User(string userName, UserType userType, string isVerified, string email, List<string> schoolNames) {
            this.userName = userName;
            this.userType = userType;
            this.isVerified = isVerified;
            this.email = email;
            this.schoolNames = schoolNames;
            schoolNames.Insert(0, chatrooms);
            this.messages = new List<string>();
        }

        public void SendMessageHistory() {
            string subject = "College Connections Message Log";
            string body = "Hello " + this.userName + ",\n\n" +
            "This is your message history from " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n\n";

            foreach (string message in messages) {
                body += message + "\n";
            }
            EmailSender.SendEmailTo(this.email, subject, body);
        }

        public bool IsHighSchooler() {
            return this.userType == UserType.HighSchooler;
        }

        public bool IsCollegeModerator() {
            return this.userType == UserType.CollegeModerator;
        }

        public bool IsAdvisor() {
            return this.userType == UserType.Advisor;
        }

        public bool IsDeveloper() {
            return this.userType == UserType.Developer;
        }

        public bool IsVerified() {
            return this.isVerified.Equals(verifiedString);
        }

        public bool IsVerifiableBy(User other) {
            if (other == null) {
                return false;
            }

            if ((IsHighSchooler() || IsCollegeModerator()) && (other.IsAdvisor() || other.IsDeveloper())) {
                return true;
            }
            if ((IsAdvisor()) && (other.IsDeveloper())) {
                return true;
            }

            return false;
        }
    }
}