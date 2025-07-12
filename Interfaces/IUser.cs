using Application.DataTransferModels.ResponseModels;
using HerbalHub.DTO.UserVM;
using HerbalHub.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HerbalHub.Interfaces
{
    public interface IUser
    {
        Task<ResponseVM> createuser(CreateUserVm user);
        Task<ResponseVM> LoginUser(LoginUserVm user);

        Task<ResponseVM> GetAllUsersAsync();
        Task<ResponseVM> VerifyEmail(string userEmail);
        Task<ResponseVM>GetUserByName(string UserName);
        Task<ResponseVM> ContactUs(ContactFormVM contact);
        Task<ResponseVM> Consultation(consultationvm con);
        Task<ResponseVM> checkouts(checkoutvm check);
        // Task<ResponseVM> SendMessage(MessageVm msg);
    }
}
