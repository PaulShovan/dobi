using Dhobi.Business.Interface;
using System;
using System.Threading.Tasks;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.UserModel.ViewModels;
using Dhobi.Core;
using Dhobi.Repository.Interface;
using Dhobi.Core.UserSms;
using Dhobi.Common;

namespace Dhobi.Business.Implementation
{
    public class UserBusiness : IUserBusiness
    {
        private IUserRepository _userRepository;
        private IUserSmsRepository _userSmsRepository;
        public UserBusiness(IUserRepository userRepository, IUserSmsRepository userSmsRepository)
        {
            _userRepository = userRepository;
            _userSmsRepository = userSmsRepository;
        }
        private string GetRandomApprovalCode()
        {
            Random generator = new Random();
            string approvalCode = generator.Next(0, 1000000).ToString("D6");
            return approvalCode;
        }
        private string GetSmsText(string approvalCode)
        {
            var smsTextTemplate = Constants.USERVERIFYSMS;
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
        private async Task<bool> SendUserSms(User user, SmsType smsType)
        {
            var approvalCode = GetRandomApprovalCode();
            var userSms = new UserSms
            {
                UserId = user.UserId,
                PhoneNumber = user.PhoneNumber,
                ApprovalCode = approvalCode,
                Text = GetSmsText(approvalCode),
                Status = (int)SmsStatus.Unapproved,
                SmsType = (int)smsType,
                IsSent = 0
            };
            return await _userSmsRepository.AddUserSms(userSms);
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
                    Name = userModel.Name,
                    PhoneNumber = userModel.PhoneNumber,
                    IsVerified = false
                };
                var response = await _userRepository.AddUser(user);
                var sendSmsResponse = await SendUserSms(user, SmsType.NewRegistration);
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
                var isCodeValid = await _userSmsRepository.ValidateUser(code, userId);
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

        public async Task<User> UserLogin(string phone, bool isVerificationRequired)
        {
            try
            {
                var user = await _userRepository.UserLogin(phone);
                if(user == null)
                {
                    return null;
                }
                if (isVerificationRequired)
                {
                    await SendUserSms(user, SmsType.NewLogin);
                }
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
