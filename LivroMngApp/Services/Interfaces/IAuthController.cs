using LivroMngApp.Constants;
using LivroMngApp.Models.AuthModels;
using System.Threading.Tasks;

namespace LivroMngApp.Services
{
    public interface IAuthController
    {
        Task<string> Execute(UserModel userModel, AuthOperations operation);
        Task<bool> ChangePhone(string phonenr);
    }
}
