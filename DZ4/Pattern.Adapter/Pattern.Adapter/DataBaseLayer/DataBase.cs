using Pattern.Adapter.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;

namespace Pattern.Adapter.DataBaseLayer
{
    class DataBase : IDataBase
    {
        SQLiteConnection connection;
        SQLiteCommand command;
        SQLiteDataReader dataReader;

        public void CreateDatabaseAndTable()
        {
            if (!File.Exists("MyDatabase.sqlite"))
            {
                SQLiteConnection.CreateFile("MyDatabase.sqlite");

                string sql = @"CREATE TABLE products(
                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                productId        INTEGER,
                                source           TEXT,
                                name             TEXT,
                                description      TEXT,
                                categoryId       INTEGER,
                                price            TEXT,
                                inStockQuantity  INTEGER
                            );";
                connection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
                connection.Open();
                command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                connection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            }
        }

        public List<ResponceObject> GetAllProducts()
        {
            List<ResponceObject> result = new List<ResponceObject>();

            command = new SQLiteCommand("Select * From products", connection);
            connection.Open();
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                CultureInfo info = CultureInfo.GetCultureInfo("ru-Ru");
                decimal priceDecimal;
                Decimal.TryParse(dataReader[6].ToString(), NumberStyles.Any, info, out priceDecimal);

                result.Add(new ResponceObject
                {
                    id = dataReader.GetInt32(1),
                    source = dataReader.GetString(2),
                    name = dataReader.GetString(3),
                    description = dataReader.GetString(4),
                    categoryId = dataReader.GetInt32(5),
                    price = priceDecimal,
                    inStockQuantity = dataReader.GetInt32(7)
                });
            }
            connection.Close();

            return result;
        }

        public void SetNewRecord(ResponceObject newRecord)
        {
            command = new SQLiteCommand();
            connection.Open();
            command.Connection = connection;
            command.CommandText = "insert into products" +
                "(productId,source,name,description,categoryId,price,inStockQuantity) " +
                "values " +
                "('" 
                + newRecord.id + "','" 
                + newRecord.source + "','" 
                + newRecord.name + "','" 
                + newRecord.description + "','"
                + newRecord.categoryId + "','"
                + newRecord.price + "','"
                + newRecord.inStockQuantity 
                + "')";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
