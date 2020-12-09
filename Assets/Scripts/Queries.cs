
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
            Console.WriteLine("Opening Connectin...");

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
            return this.myCommand.ExecuteReader();
        }

        public void insertUser() {
            string userInsert = "insert into `csci380`.`user` (firstName, lastName, password, type, isVerified, email) VALUES ('testUnity', 'LastName', 'test', 'HighSchooler', '12345', 'BSEmail');";
            prepareAndRunStatement(userInsert);
        }

        public string getPassword(string firstName, string lastName) {
            string userInsert = "select password FROM `csci380`.`user` WHERE (firstName, lastName)=('" + firstName + "', '" + lastName + "');";
            MySqlDataReader dataReader = prepareAndRunQuery(userInsert);
            dataReader.Read();
            return dataReader["password"] + "";
        }
    }
}