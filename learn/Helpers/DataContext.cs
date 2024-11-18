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
            await _refreshTables();
            await _initTables();
            await _seedData();
        }

        private async Task _initDatabase()
        {
            var connectionString = $"Server={_dbSetting.Server};Database={_dbSetting.Database};User Id={_dbSetting.UserId};Password={_dbSetting.Password};";
            using var connection = new MySqlConnection(connectionString);
            var sql = $"CREATE DATABASE IF NOT EXISTS {_dbSetting.Database};";
            await connection.ExecuteAsync(sql);

        }

        private async Task _refreshTables()
        {
            using var connection = CreateConnection();

            await connection.ExecuteAsync("DROP TABLE IF EXISTS `transactions`");
            await connection.ExecuteAsync("DROP TABLE IF EXISTS `products`");
            await connection.ExecuteAsync("DROP TABLE IF EXISTS `users`");
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
                    `Fullname` varchar(50) NOT NULL,
                    `Username` varchar(50) NOT NULL,
                    `Email` varchar(255) NOT NULL,
                    `Password` varchar(255) NOT NULL,
                    `Role` int NOT NULL,
                    PRIMARY KEY (`id`)
                )";
                await connection.ExecuteAsync(sql);
            }

            async Task _initProducts()
            {
                var sql = @"CREATE TABLE IF NOT EXISTS `products` (
                    `id` int NOT NULL AUTO_INCREMENT,
                    `Name` varchar(50) NOT NULL,
                    `Description` text NOT NULL,
                    `Price` decimal(10,2) NOT NULL,
                    PRIMARY KEY (`id`)
                )";
                await connection.ExecuteAsync(sql);
            }

            async Task _initTransaction()
            {
                var sql = @"CREATE TABLE IF NOT EXISTS `transactions` (
                    `id` int NOT NULL AUTO_INCREMENT,
                    `UserId` int NOT NULL,
                    `ProductId` int NOT NULL,
                    `Quantity` int NOT NULL,
                    `Total` decimal(10,2) NOT NULL,
                    `Date` datetime NOT NULL,
                    PRIMARY KEY (`id`),
                    FOREIGN KEY (`UserId`) REFERENCES `users`(`id`),
                    FOREIGN KEY (`ProductId`) REFERENCES `products`(`id`)
                )";
                await connection.ExecuteAsync(sql);
            }
        }

        private async Task _seedData()
        {
            using var connection = CreateConnection();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password");

            // Seed Users with hashed passwords
            var userSql = @"INSERT INTO `users` (Fullname, Username, Email, Password, Role) VALUES
                            ('Admin', 'admin', 'admin@mail.com', @Password, 0),
                            ('User', 'user', 'user@mail.com', @Password, 1);";
            await connection.ExecuteAsync(userSql, new { Password = hashedPassword });


            // Seed Products
            var productSql = @"INSERT INTO `products` (Name, Description, Price) VALUES
                               ('Product A', 'Description for product A', 19.99),
                               ('Product B', 'Description for product B', 29.99);";
            await connection.ExecuteAsync(productSql);

            // Seed Transactions with the Date column
            //var transactionSql = @"INSERT INTO `transactions` (UserId, ProductId, Quantity, Total, Date) VALUES
              //                     (1, 1, 2, 39.98, @Date1),
              //                     (2, 2, 1, 29.99, @Date2);";
            //await connection.ExecuteAsync(transactionSql, new { Date1 = DateTime.Now, Date2 = DateTime.Now.AddDays(-1) });
        }
    }
}
