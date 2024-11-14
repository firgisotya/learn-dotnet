using AutoMapper;
using learn.Entities;
using learn.Helpers;
using learn.Models.Users;
using learn.Repositories;

namespace learn.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task Create(CreateRequest model);
        Task Update(int id, UpdateRequest model);
        Task Delete(int id);
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return user;
        }

        public async Task Create(CreateRequest model)
        {
            if (await _userRepository.GetByEmail(model.Email) != null)
                throw new AppException("User with the email '"+ model.Email +"' already exists");

            var user = _mapper.Map<User>(model);
            
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            await _userRepository.Create(user);
        }

        public async Task Update(int id, UpdateRequest model)
        {
            var user = await GetById(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            // validate email
            var emailChanged = !string.IsNullOrEmpty(model.Email) && model.Email != user.Email;
            if (emailChanged && await _userRepository.GetByEmail(model.Email) != null)
                throw new AppException("User with the email '" + model.Email + "' already exists");

            //hash password
            if(!string.IsNullOrEmpty(model.Password))
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            _mapper.Map(model, user);

            await _userRepository.Update(user);
        }

        public async Task Delete(int id)
        {
            var user = await GetById(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            await _userRepository.Delete(id);
        }



    }
}
