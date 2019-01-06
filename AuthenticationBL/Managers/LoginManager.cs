﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationCommon.Execeptions;
using AuthenticationCommon.Models;
using AuthenticationCommon.ModelsDTO;
using AuthenticationRepository.DynamoDb;
using Jose;

namespace AuthenticationBL.Managers
{
    public class LoginManager
    {
        DynamoService _dynamo;
        TokenManager _tokenManager;

        //Ctor
        public LoginManager()
        {
            _dynamo = new DynamoService();
            _tokenManager = new TokenManager();
        }

        //Public Methods
        public string Register(UserLoginDTO userDTO)
        {
            if (IsEmailAvailable(userDTO.Email))
            {
                string token;
                User userToRegister = new User() { Email = userDTO.Email, Password = userDTO.Password };
                try
                {
                    _dynamo.Store(userToRegister);
                }
                catch (Exception)
                {
                    throw new FaildToConnectDbException();
                }

                token = _tokenManager.GenerateToken(userToRegister.UserId, userToRegister.Email);

                return token;
            }
            else
            {
                throw new AlreadyExistException("Email");
            }
        }

        public string LoginManual(UserLoginDTO userLoginDTO)
        {
            string token;
            User foundUser = GetUser(userLoginDTO.Email);

            if (foundUser == null)
            {
                throw new NotMatchException("Email");
            }
            if (foundUser.Password != userLoginDTO.Password)
            {
                throw new NotMatchException("Password");
            }

            token = _tokenManager.GenerateToken(foundUser.UserId, foundUser.Email);

            return token;
        }

        public void ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            User foundUser = GetUser(resetPasswordDTO.Email);

            if(foundUser == null)
            {
                throw new NotMatchException("Email");
            }
            if(foundUser.Password != resetPasswordDTO.OldPassword)
            {
                throw new NotMatchException("Old password");
            }

            foundUser.Password = resetPasswordDTO.NewPassword;

            try
            {
                _dynamo.UpdateItem(foundUser);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }


        //Private Methods
        private bool IsEmailAvailable(string email)
        {
            User foundUser;
            try
            {
                foundUser = _dynamo.GetItem<User>(email);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
            return foundUser == null;
        }

        private User GetUser(string email)
        {
            User foundUser;
            try
            {
                foundUser = _dynamo.GetItem<User>(email);
            }
            catch (Exception)
            {

                throw new FaildToConnectDbException();
            }
            return foundUser;
        }
    }
}
