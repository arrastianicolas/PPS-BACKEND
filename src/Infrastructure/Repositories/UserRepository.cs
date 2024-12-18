﻿using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository 
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context; 
        }
        public IEnumerable<object> Get()
        {
            var users = _context.Users
                .Select(user => new UserWithDetailsDto
                {
                    User = UserDto.Create(user),
                    Client = _context.Clients
                        .FirstOrDefault(c => c.Iduser == user.Id),
                    Trainer = _context.Trainers

                        .FirstOrDefault(t => t.Iduser == user.Id)
                })
                .ToList();
            return users;
        }



        public User GetByUserEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetUsersByType(string type)
        {
            return _context.Users.Where(u => u.Type == type).ToList();
        }
    }
}
