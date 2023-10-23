using System;
using System.Data;
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

            // Tạo đối tượng SqlCommand
            MySqlCommand command = new MySqlCommand(query, connect);

            // Cung cấp giá trị cho các tham số
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);

            // Thực thi truy vấn
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
                if (reader.Read())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool CheckUser(string username)
        {
            string query = "SELECT * FROM users WHERE Nickname = @username";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@username", username);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                if (reader.Read()) {
                    // Dữ liệu trùng
                    return false;
                } else {
                    // Dữ liệu không trùng
                    return true;
                }
            }
        }
        public void CreateAcc(int Uin, string username)
        {
            string query = "INSERT INTO users (Uin, Nickname) VALUES (@uid, @user);";

            
            // Tạo đối tượng SqlCommand
            MySqlCommand command = new MySqlCommand(query, connect);

            // Cung cấp giá trị cho các tham số
            command.Parameters.AddWithValue("@uid", Uin);
            command.Parameters.AddWithValue("@user", username);
            // Thực thi truy vấn
            command.ExecuteNonQuery();
        }
    }
}