
using System;
using MySql.Data;
using MySql.Data.MySqlClient;

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

        public void insertUser() {
            string userInsert = "insert into `csci380`.`user` (firstName, lastName, password, type, isVerified, email) VALUES ('testUnity', 'LastName', 'test', 'HighSchooler', '12345', 'BSEmail');";
            prepareAndRunStatement(userInsert);
        }

        public void insertSchool(string school, string firstName, string lastName){
            string insertSchool = "insert into `csci380`.`school` (school, advisorFirstName, advisorLastName) VALUES (' " + school + ' ", ' + firstName + " ' " + lastName " ') ";
            prepareAndRunStatement(insertSchool);
        }

        public void insertUserSchool(string school, string firstName, string lastName){
            string insertUS = "insert into `csci380`.`user-school` (firstName, lastName, schoolName) VALUES (' " + firstName + ' ", ' + lastName + " ' " + school " ') ";
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
            string getAdvisorEmail = "select email FROM `csci380`.`user` WHERE (firstName, lastName)=(select ' " + advisorFirstName + ' ", ' +advisorLastName " ' FROM `csci380`.`school` WHERE school=' "school');";
            MySqlDataReader dataReader = prepareAndRunQuery(getAdvisorEmail);
            dataReader.Read();
            return dataReader["advisor email"] + "";
        }

        public string getPassword(string firstName, string lastName) {
            string userInsert = "select password FROM `csci380`.`user` WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(userInsert);
            return dataReader["password"] + "";
        }

        public string getSchools(string firstName, string lastName){
            string getSchools = "select schoolName FROM `csci380`.`user-school` WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "');";
        }

        //test from sean
    }
}