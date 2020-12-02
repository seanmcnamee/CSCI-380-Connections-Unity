using System;
using System.Collections.Generic;

namespace SecAuth
{
    public class User
    {
        public enum UserType {
            HighSchooler, CollegeModerator, Advisor, Developer
        }
        private string userName {get;}
        private UserType userType {get;}
        private string isVerified {get;}
        private string email {get;}
        private string[] schoolNames;
        public List<string> messages { get;}

        public User(string userName, UserType userType, string isVerified, string email, string[] schoolNames) {
            this.userName = userName;
            this.userType = userType;
            this.isVerified = isVerified;
            this.email = email;
            this.schoolNames = schoolNames;
            this.messages = new List<string>();
        }

        public bool canChatIn(string schoolName) {
            var index = Array.FindIndex(schoolNames, x => x == schoolName);
            return index != -1;
        }

        public void sendMessageHistory() {
            string subject = "College Connections Message Log";
            string body = "Hello " + this.userName + ",\n\n" +
            "This is your message history from " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n\n";

            foreach (string message in messages) {
                body += message + "\n";
            }
            EmailSender.sendEmailTo(this.email, subject, body);
        }
    }
}