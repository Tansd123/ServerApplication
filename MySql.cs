using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

namespace ServerApplication
{
    public class MySql
    {
        public static MySql mysql = new MySql();
        public static MySqlConnection connect = new MySqlConnection();
        public void InitDatabase()
        {
            string MyConnectString;
            MyConnectString = "server=127.0.0.1;uid=root;" +"pwd=159753;database=user";
            try
            {
                connect.ConnectionString = MyConnectString;
                connect.Open();
                if (connect.State == ConnectionState.Open)
                {
                    Console.WriteLine("Database has open");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public bool CheckUsername(string username)
        {
            string query = "SELECT * FROM accounts WHERE Username = @username";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@username", username);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                if (reader.Read()) {
                    // Dữ liệu trùng
                    return true;
                } else {
                    // Dữ liệu không trùng
                    return false;
                }
            }

        
        }

        public void CreateUser(string username, string password, string email)
        {
            string query = "INSERT INTO accounts (username, email, password) VALUES (@username, @email, @password);";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            command.ExecuteNonQuery();
        }

        public bool CheckPass(string username ,string password)
        {
            string query = "SELECT Password FROM accounts WHERE Username = @user";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@user", username);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    string dbpass = reader["Password"].ToString();
                    if (dbpass == password)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public int GetUin(string Username)
        {
            string query = "SELECT Uin FROM accounts WHERE Username = @user";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@user", Username);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    int Uin = reader["Uin"].GetHashCode();
                    Console.WriteLine(Uin);
                    return Uin;
                }
                else return 0;
            }
        }

        public bool checkAcc(int Uin)
        {
            string query = "SELECT * FROM users WHERE Uin = @uin";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@uin", Uin.ToString());
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read()) return false;
                else return true;
            }
        }

        public bool CheckUser(string username)
        {
            string query = "SELECT * FROM users WHERE Nickname = @username";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@username", username);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                if (reader.Read()) return false;
                else return true;
            }
        }
        public void CreateAcc(int Uin, string username)
        {
            string query = "INSERT INTO users (Uin, Nickname) VALUES (@uid, @user);";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@uid", Uin);
            command.Parameters.AddWithValue("@user", username);
            command.ExecuteNonQuery();
        }

        public void GetInfomationUser(int _toClient ,int Uin, out string nickname,out int level,out float exp,out int gold)
        {
            string query = "SELECT Nickname, Level, Exp, Gold FROM users WHERE Uin = @uin";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@uin", Uin.ToString());
            nickname = null;level = 0;exp = 0;gold = 0;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    nickname = reader["Nickname"].ToString();
                    level = reader["Level"].GetHashCode();
                    exp = reader.GetFloat("Exp");
                    gold = reader["Gold"].GetHashCode();
                }
            }
        }

        public float GetMaxExp(int Level)
        {
            int i = 1;
            foreach (var MaxExp in Network.MaxExps)
            {
                if (i == Level)return MaxExp.Exp;
                i++;
            }

            return 0;
        }

        public void GainExp(int Uin, float Exp)
        {
            string query = "UPDATE users SET Exp = @Exp WHERE Uin = @uin";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@Exp", Exp.ToString());
            command.Parameters.AddWithValue("@uin", Uin.ToString());
            
            command.ExecuteNonQuery();
        }
        public void LevelUp(int Uin,int Level, float Exp)
        {
            string query = $"UPDATE users SET Level = {Level}, Exp = {Exp} WHERE Uin = {Uin}";
            MySqlCommand command = new MySqlCommand(query, connect);
            Console.WriteLine($"{Level} {Exp}");
            command.ExecuteNonQuery();
        }
    }
}