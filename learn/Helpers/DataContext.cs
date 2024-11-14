using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace learn.Helpers
{
    public class DataContext
    {
        private DBSetting _dbSetting;

        public DataContext(IOptions<DBSetting> dbSetting)
        {
            _dbSetting = dbSetting.Value;
        }

        public IDbConnection CreateConnection()
        {
           var connectionString = $"Server={_dbSetting.Server};Database={_dbSetting.Database};User Id={_dbSetting.UserId};Password={_dbSetting.Password};";
            return new MySqlConnection(connectionString);
        }

        public async Task init()
        {
            await _initDatabase();
            await _initTables();
        }

        private async Task _initDatabase()
        {
            var connectionString = $"Server={_dbSetting.Server};Database={_dbSetting.Database};User Id={_dbSetting.UserId};Password={_dbSetting.Password};";
            using var connection = new MySqlConnection(connectionString);
            var sql = $"CREATE DATABASE IF NOT EXISTS {_dbSetting.Database};";
            await connection.ExecuteAsync(sql);

        }

        private async Task _initTables()
        {
            using var connection = CreateConnection();
            await _initUsers();
            await _initProducts();
            await _initTransaction();

            async Task _initUsers()
            {
                var sql = @"CREATE TABLE IF NOT EXISTS `users` (
                    `id` int NOT NULL AUTO_INCREMENT,
                    `fullname` varchar(50) NOT NULL,
                    `username` varchar(50) NOT NULL,
                    `email` varchar(50) NOT NULL,
                    `password` varchar(50) NOT NULL,
                    `role` varchar(50) NOT NULL,
                    PRIMARY KEY (`id`)
                )";
                await connection.ExecuteAsync(sql);
            }

            async Task _initProducts()
            {
                var sql = @"CREATE TABLE IF NOT EXISTS `products` (
                    `id` int NOT NULL AUTO_INCREMENT,
                    `name` varchar(50) NOT NULL,
                    `description` text NOT NULL,
                    `price` decimal(10,2) NOT NULL,
                    PRIMARY KEY (`id`)
                )";
                await connection.ExecuteAsync(sql);
            }

            async Task _initTransaction()
            {
                var sql = @"CREATE TABLE IF NOT EXISTS `orders` (
                    `id` int NOT NULL AUTO_INCREMENT,
                    `user_id` int NOT NULL,
                    `product_id` int NOT NULL,
                    `quantity` int NOT NULL,
                    `total` decimal(10,2) NOT NULL,
                    PRIMARY KEY (`id`)
                )";
                await connection.ExecuteAsync(sql);
            }
        }
       }
