using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRESTServices.BLL
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserData _userData;
        private readonly IMapper _mapper;

        public UserBLL(IUserData userData, IMapper mapper)
        {
            _userData = userData;
            _mapper = mapper;
        }

        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            await _userData.ChangePassword(username, newPassword);
            return Task.CompletedTask;
        }

        public Task<Task> Delete(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await _userData.GetAll();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<IEnumerable<UserDTO>> GetAllWithRoles()
        {
            var users = await _userData.GetAllWithRoles();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetByUsername(string username)
        {
            var user = await _userData.GetByUsername(username);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserWithRoles(string username)
        {
            var user = await _userData.GetUserWithRoles(username);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<Task> Insert(UserCreateDTO entity)
        {
            var user = _mapper.Map<User>(entity);
            await _userData.Insert(user);
            return Task.CompletedTask;
        }

        public async Task<UserDTO> Login(LoginDTO loginDTO)
        {
            var user = await _userData.Login(loginDTO.Username, loginDTO.Password);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> LoginMVC(LoginDTO loginDTO)
        {
            var user = await _userData.Login(loginDTO.Username, loginDTO.Password);
            return _mapper.Map<UserDTO>(user);
        }

    }
}
