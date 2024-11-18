using Dapper;
using learn.Entities;
using learn.Helpers;

namespace learn.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task Create(User user);
        Task Update(User user);
        Task Delete(int id);
    }

    public class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM users";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM users WHERE id = @id";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
        }


        public async Task<User> GetByEmail(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM users WHERE Email = @email";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        }

        public async Task Create(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO users (Fullname, Username, Email, Password, Role) VALUES (@fullname, @username, @email, @password, @role)";
            await connection.ExecuteAsync(sql, user);
        }

        public async Task Update(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE users SET Fullname = @fullname, Username = @username, Email = @email,  Role = @role WHERE id = @id";
            await connection.ExecuteAsync(sql, user);
        }

        public async Task Delete(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM users WHERE id = @id";
            await connection.ExecuteAsync(sql, new { id });
        }
    }

}
