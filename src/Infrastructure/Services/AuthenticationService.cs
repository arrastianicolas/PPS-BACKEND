using Application.Interfaces;
using Application.Models.Requests;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.TempModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthenticationService : ICustomAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITrainerRepository _trainerRepository;
        private readonly AuthenticacionServiceOptions _options;

        public AuthenticationService(IUserRepository userRepository, IOptions<AuthenticacionServiceOptions> options, IClientRepository clientRepository ,ITrainerRepository trainerRepository )
        {
            _userRepository = userRepository;
            _options = options.Value;
            _clientRepository = clientRepository;
            _trainerRepository = trainerRepository;
        }
        private User? ValidateUser(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrEmpty(authenticationRequest.Email) || string.IsNullOrEmpty(authenticationRequest.Password))
                return null;

            var user = _userRepository.GetByUserEmail(authenticationRequest.Email);

            if (user == null) return null;

            if (user.Email != authenticationRequest.Email || user.Password != authenticationRequest.Password) return null;

            var client = _clientRepository.GetClientByUserId(user.Id);
            if (client != null && client.Isactive == 1)
            {
                return user;
            }

            //Validar si el usuario es un Trainer y está activo
            var trainer = _trainerRepository.GetTrainerByUserId(user.Id);
            if (trainer != null && trainer.Isactive == 1)
            {
                return user; // Retorna si el usuario es un Trainer y está activo 
            }


            var admin = _userRepository.GetById(user.Id);
            if (admin != null)
            {
                return user;
            }
               
            //Si no es ni Client/Trainer/Admin activo, retornamos null
            return null; 
        }


        public string Autenticar(AuthenticationRequest authenticationRequest)
        {
            //Paso 1: Validamos las credenciales
            var user = ValidateUser(authenticationRequest); //Lo primero que hacemos es llamar a una función que valide los parámetros que enviamos.

            if (user == null)
            {
                throw new Exception("User authentication failed");
            }


            //Paso 2: Crear el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey)); //Traemos la SecretKey del Json. agregar antes: using Microsoft.IdentityModel.Tokens;

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            //Los claims son datos en clave->valor que nos permite guardar data del usuario.
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString())); //"sub" es una key estándar que significa unique user identifier, es decir, si mandamos el id del usuario por convención lo hacemos con la key "sub".
            claimsForToken.Add(new Claim("mail", authenticationRequest.Email));
            claimsForToken.Add(new Claim("role", user.Type)); //Debería venir del usuario

            var jwtSecurityToken = new JwtSecurityToken( //agregar using System.IdentityModel.Tokens.Jwt; Acá es donde se crea el token con toda la data que le pasamos antes.
              _options.Issuer,
              _options.Audience,
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              credentials);

            var tokenToReturn = new JwtSecurityTokenHandler() //Pasamos el token a string
                .WriteToken(jwtSecurityToken);

            return tokenToReturn.ToString();
        }


        public class AuthenticacionServiceOptions
        {
            public const string AutenticacionService = "AutenticacionService";
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string SecretForKey { get; set; }
        }

    }
}
