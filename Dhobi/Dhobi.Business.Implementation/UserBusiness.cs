using Dhobi.Business.Interface;
using System;
using System.Threading.Tasks;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.UserModel.ViewModels;
using Dhobi.Core;
using Dhobi.Repository.Interface;
using Dhobi.Core.RegistrationSms;
using Dhobi.Common;

namespace Dhobi.Business.Implementation
{
    public class UserBusiness : IUserBusiness
    {
        private IUserRepository _userRepository;
        private IRegistrationSmsRepository _registrationSmsRepository;
        public UserBusiness(IUserRepository userRepository, IRegistrationSmsRepository registrationSmsRepository)
        {
            _userRepository = userRepository;
            _registrationSmsRepository = registrationSmsRepository;
        }
        private string GetRandomApprovalCode()
        {
            Random generator = new Random();
            string approvalCode = generator.Next(0, 1000000).ToString("D6");
            return approvalCode;
        }
        private string GetSmsText(string approvalCode)
        {
            var smsTextTemplate = Constants.NEWREGISTERSMS;
            var smsText = smsTextTemplate.Replace("__CODE__", approvalCode);
            return smsText;
        }
        private async Task<bool> IsPhoneNumberAvailable(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }
            return await _userRepository.IsPhoneNumberAvailable(phoneNumber);
        }
        private async Task<bool> SendRegistrationSms(User user)
        {
            var approvalCode = GetRandomApprovalCode();
            var registrationSms = new RegistrationSms
            {
                UserId = user.UserId,
                PhoneNumber = user.PhoneNumber,
                ApprovalCode = approvalCode,
                Text = GetSmsText(approvalCode),
                Status = (int)RegistrationSmsStatus.Unapproved
            };
            return await _registrationSmsRepository.AddRegistrationSms(registrationSms);
        }
        public async Task<User> AddUser(UserViewModel userModel)
        {
            try
            {
                if (!await IsPhoneNumberAvailable(userModel.PhoneNumber))
                {
                    return null;
                }
                var user = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Name = userModel.PhoneNumber,
                    PhoneNumber = userModel.PhoneNumber,
                    IsVerified = false
                };
                var response = await _userRepository.AddUser(user);
                var sendSmsResponse = await SendRegistrationSms(user);
                if (!response || !sendSmsResponse)
                {
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error int Add User" + ex);
            }
        }

        public async Task<User> ValidateRegisteredUser(string code, string userId)
        {
            try
            {
                var isCodeValid = await _registrationSmsRepository.ValidateRegisteredUser(code, userId);
                if (!isCodeValid)
                {
                    return null;
                }
                return await _userRepository.UpdateUserAsVerified(userId);
            }
            catch(Exception ex)
            {
                throw new Exception("Error validating user" + ex);
            }
        }

        public async Task<User> GetUserById(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return null;
                }
                return await _userRepository.GetUserById(userId);
            }
            catch (Exception exception)
            {

                throw new Exception("Error getting user" + exception);
            }
        }
    }
}
