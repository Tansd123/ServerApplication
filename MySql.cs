using System;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
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
            command.Parameters.AddWithValue("@uid", Uin.ToString());
            command.Parameters.AddWithValue("@user", username);
            command.ExecuteNonQuery();

            int IDAquarium = 0;
            query = "SELECT * FROM aquarium";
            command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@username", username);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read())
                {
                    IDAquarium++;
                }
            }

            query = "INSERT INTO aquarium (Uin, ID, Slot,MaxFish, CurFish) VALUES (@uid, @id, @slot, @maxfish, @curfish)";
            command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@uid", Uin.ToString());
            command.Parameters.AddWithValue("@id", IDAquarium++);
            command.Parameters.AddWithValue("@slot", "1");
            command.Parameters.AddWithValue("@maxfish", "4");
            command.Parameters.AddWithValue("@curfish", "0");
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
            string query = $"UPDATE users SET Level = {Level.ToString()}, Exp = {Exp.ToString()} WHERE Uin = {Uin.ToString()}";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.ExecuteNonQuery();
        }

        public void GetInfomationShop(int ItemID, out string name, out int level, out float time, out float exp,out int getgold, out int cost)
        {
            name = null;
            level = 0;
            time = 0;
            exp = 0;
            getgold = 0;
            cost = 0;
            foreach (var Shop in Network.Shops)
            {
                if (Shop.ItemId == ItemID)
                {
                    name = Shop.Name;
                    level = Shop.Level;
                    time = Shop.Time;
                    exp = Shop.Exp;
                    getgold = Shop.Gold;
                    cost = Shop.Cost;
                }
            }
        }

        public bool checkcost(int cost, int Uin)
        {
            string query = "SELECT  Gold FROM users WHERE Uin = @uin";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@uin", Uin.ToString());
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (cost >= reader.GetInt32("Gold")) return true;
                    else return false;
                }
            }

            return false;
        }
        
        public void GoldRemove(int cost, int Uin)
        {
            string query = $"UPDATE users SET Gold = Gold - {cost.ToString()} WHERE Uin = {Uin.ToString()}"; //UpdateGold
            MySqlCommand command  = new MySqlCommand(query, connect);
            command.ExecuteNonQuery();
            
        }

        public void addFish(int ItemId, int IDAqua, string name, int g, int coin, float exp)
        {
            string query = $"UPDATE aquarium SET CurFish = CurFish + 1 WHERE ID = {IDAqua.ToString()}";
            MySqlCommand command = new MySqlCommand(query, connect); // Update CurFish
            command.ExecuteNonQuery();
            
            query = $"INSERT INTO fish (IDAquarium, ItemID, Name, Level, Food, Grow, Gender, Coin, Exp) VALUES ({IDAqua.ToString()}, {ItemId.ToString()}, {name}, 1 , 0 , 0, {g.ToString()}, {coin.ToString()}, {exp.ToString()});"; // Add Item to Table
            command = new MySqlCommand(query, connect);
            command.ExecuteNonQuery();
        }
        
        public void addItem(int Uin, int ItemID, int ItemNum, int AvailPeriod)
        {
            string query = "INSERT INTO item (Uin, ItemID, ItemNum, AvailPeriod, Status) VALUES ({Uin}, {ItemID}, {ItemNum}, {AvailPeriod}, 0);"; // Add Item to Table
            MySqlCommand command = new MySqlCommand(query, connect);
            command.ExecuteNonQuery();
        }

        public void getaquarium(int Uin, out Aquarium[] Aquariums, out int numaqua)
        {
            Aquariums = new Aquarium[5];
            numaqua = 0;
            string query = "SELECT ID, Slot, MaxFish, CurFish FROM aquarium WHERE Uin = @uin"; //Find Data Aquarium
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@uin", Uin.ToString());
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Aquariums[numaqua] = new Aquarium();
                    Aquariums[numaqua].ID = reader.GetInt32("ID");
                    Aquariums[numaqua].Slot = reader.GetInt32("Slot");
                    Aquariums[numaqua].MaxFish = reader.GetInt32("MaxFish");
                    Aquariums[numaqua].CurFish = reader.GetInt32("CurFish");
                    numaqua++;
                }
            }
        }

        public void getfish(int IDAqua, out Fish[] fishs, out int numfish)
        {
            fishs = new Fish[20];
            numfish = 0;
            string query = "SELECT ID,ItemID, Name, Level, Food, Grow,Gender,Coin,Exp, IDAquarium FROM fish WHERE IDAquarium = @ID";
            MySqlCommand command = new MySqlCommand(query, connect);
            command.Parameters.AddWithValue("@ID", IDAqua.ToString());
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    fishs[numfish] = new Fish();
                    fishs[numfish].ID = reader.GetInt32("ID");
                    fishs[numfish].FishID = reader.GetInt32("ItemID");
                    fishs[numfish].name = reader.GetString("Name");
                    fishs[numfish].Level = reader.GetInt32("Level");
                    fishs[numfish].Food = reader.GetFloat("Food");
                    fishs[numfish].Grow = reader.GetFloat("Grow");
                    fishs[numfish].Gender = reader.GetInt32("Gender");
                    fishs[numfish].IDAqua = reader.GetInt32("IDAquarium");
                    fishs[numfish].gold = reader.GetInt32("Coin");
                    fishs[numfish].exp = reader.GetFloat("Exp");
                    Console.WriteLine(fishs[numfish].FishID);
                    numfish++;
                }
            }
        }

        public void createaquarium(int Uin, int Slot)
        {
            string query = "INSERT INTO aquarium (Uin, Slot) VALUES ({Uin}, {Slot});"; // Add Item to Table
            MySqlCommand command = new MySqlCommand(query, connect);
            command.ExecuteNonQuery();
        }

        public void buyfish(int IDAqua, int id, int g, out int ID)
        {
            string query = "INSERT INTO fish (IDAquairum, ItemID, Gender) VALUES ({IDAqua}, {id}, {g});"; // Add Item to Table
            MySqlCommand command = new MySqlCommand(query, connect);
            command.ExecuteNonQuery();
            int lastInsertedId = Convert.ToInt32(command.LastInsertedId);
            ID = lastInsertedId;
        }

        public void eatting(int ID, float food)
        {
            
        }
        
    }
}