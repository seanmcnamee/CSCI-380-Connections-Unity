
using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Data;

namespace DB {
    public class Queries {
        
        //private string connString = "server=173.3.21.87;user=CSCI380;database=csci380;port=3306;password=CSCI380DBPASSWORD";
        private string connString = "server=173.3.21.87;uid=CSCI380;pwd=CSCI380DBPASSWORD;database=csci380";
        private MySqlConnection myConnection;
        private MySqlCommand myCommand;

        public Queries() {
            Console.WriteLine("Opening Connection...");

            this.myConnection = new MySqlConnection(connString);
            this.myConnection.Open();
            this.myCommand = new MySqlCommand();
            this.myCommand.Connection = myConnection;
        }

        public void closeConenction() {
            this.myCommand.Connection.Close();
        }

        private void prepareAndRunStatement(string statement) {
            this.myCommand.CommandText = statement;
            this.myCommand.ExecuteNonQuery();
            //MySqlDataReader test;
        }
        
        private MySqlDataReader prepareAndRunQuery(string statement) {
            this.myCommand.CommandText = statement;
            MySqlDataReader dataReader = this.myCommand.ExecuteReader();
            dataReader.Read(); //TODO if this fucks up move back into calling method
            return dataReader;
        }

        //Example with no variables
        public void insertUser(string firstName, string lastName, string password, User.UserType type, string isVerified, string email) { //TODO add variable
            string userInsert = "insert into `csci380`.`user` (firstName, lastName, password, type, isVerified, email) VALUES ('"+ firstName + "', '" + lastName + "', '" + password + "', '" + ((int)type) + "', '" + isVerified + "', '" + email + "');";
            prepareAndRunStatement(userInsert);
        }

        //Working with variables
        public void insertSchool(string school, string firstName, string lastName){
            string insertSchool = "insert into `csci380`.`school` (school, advisorFirstName, advisorLastName) VALUES ('" + school + "', '" + firstName + "', '" + lastName + "')";
            prepareAndRunStatement(insertSchool);
        }

        //Not working :(
        public void insertUserSchool(string school, string firstName, string lastName){
            string insertUser = "insert into `csci380`.`user-school` (firstName, lastName, schoolName) VALUES ('" + firstName + "', '" + lastName + "', " + school + "');";
            prepareAndRunStatement(insertUser);
        }

        public void setVerified(string firstName, string lastName) {
            string setVerified = "UPDATE `csci380`.`user` SET isVerified='Verified' WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "')";
            prepareAndRunStatement(setVerified);
        }

        public string getVerification(string firstName, string lastName){
            string getCode = "select isVerified FROM `csci380`.`user` WHERE (firstName, lastName)=('" + firstName + "', '" +  lastName + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(getCode);
            return dataReader["isVerified"] + "";
        }

        public User getUser(string firstName, string lastName) {
            //Getting general user info
            string getInfo = "select type, isVerified, email FROM `csci380`.`user` WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(getInfo);
            
            User.UserType userType = (User.UserType)(int)dataReader["type"];
            string isVerified = dataReader["isVerified"] + "";
            string email = dataReader["email"] + "";
            string[] schools = null;
            //string[] schools = getSchools(firstName, lastName);

            return new User(firstName+lastName, userType, isVerified, email, schools);
        }

        private string[] getSchools(string firstName, string lastName){
            string getSchools = "select schoolName FROM `csci380`.`user-school` WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(getSchools);
            List<string> listSchools = new List<string>();
            do {
                listSchools.Add(dataReader["school"] + "");
            } while (dataReader.Read());

            return listSchools.ToArray();
        }


        public string getSchool(string school){
            string getSchool = "select school FROM 'csci380', 'school' WHERE (school)=('" + school + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(getSchool);
            return dataReader["school"] + "";
        }

        public string getAdvisorEmail(string school){
            string getAdvisorEmail = "select email FROM `csci380`.`user` WHERE (firstName, lastName)=(select advisorFirstName, advisorLastName FROM `csci380`.`school` WHERE school='" + school + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(getAdvisorEmail);
            return dataReader["email"] + "";
        }

        public string getDeveloperEmail(){
            string getDeveloperEmail = "select email FROM `csci380`.`user` WHERE userType='Developer';";
            MySqlDataReader dataReader = prepareAndRunQuery(getDeveloperEmail);
            return dataReader["email"] + "";
        }

        public string getPassword(string firstName, string lastName) {
            string userInsert = "select password FROM `csci380`.`user` WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(userInsert);
            return dataReader["password"] + "";
        }
        

/*         //Not working :(
        public void insertUserSchool(string school, string firstName, string lastName){
            string insertUS = "insert into `csci380`.`user-school` (firstName, lastName, schoolName) VALUES (' " + firstName + ' ", ' + lastName + " ' " + school " ');";
        }

        public string getVerification(string firstName, string lastName){
            string getCode = "select isVerified FROM `csci380`.`user` WHERE (firstName, lastName)=(' " + firstName + ' ", '  + lastName + " ');";
            MySqlDataReader dataReader = prepareAndRunQuery(getCode);
            dataReader.Read();
            return dataReader["verification code"] + "";
        }

        public string getGeneralInfo(string firstName, string lastName){
            string getInfo = "select type, isVerified, email FROM `csci380`.`user` WHERE (firstName, lastName)=(' " + firstName + ' ", '  + lastName " ');";
            MySqlDataReader dataReader = prepareAndRunQuery(getInfo);
            dataReader.Read();
            return dataReader["isVerfied and email"] + "";
        }

        public string getSchool(string school){
            string getSchool = "select school FROM 'csci380', 'school' WHERE (school)=(' " + school " ');";
            MySqlDataReader dataReader = prepareAndRunQuery(getSchool);
            dataReader.Read();
            return dataReader["school"] + "";
        }

        public string getAdvisorEmail(string advisorFirstName, string advisorLastName, string school){
            string getAdvisorEmail = "select email FROM `csci380`.`user` WHERE (firstName, lastName)=(select ' " + advisorFirstName + ' ", ' + advisorLastName + " ' FROM `csci380`.`school` WHERE school=' " + school + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(getAdvisorEmail);
            dataReader.Read();
            return dataReader["advisor email"] + "";
        }

        public string getPassword(string firstName, string lastName) {
            string userInsert = "select password FROM `csci380`.`user` WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(userInsert);
            return dataReader["password"] + "";
        }

        public ArrayList getSchools(string firstName, string lastName){
            string getSchools = "select schoolName FROM `csci380`.`user-school` WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(userInsert);
            ArrayList schools = new ArrayList();
            do {
                schools.Add(dataReader['school']);
            } while (dataReader.Read());
            return schools;
        } */
    }
}