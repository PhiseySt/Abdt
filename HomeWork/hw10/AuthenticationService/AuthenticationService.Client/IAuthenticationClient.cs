using AuthenticationService.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading.Tasks;

namespace AuthenticationService.Client
{
    public interface IAuthenticationClient
    {
        [Post("/Auth/login")]
        Task<ObjectResult> Login([Body] LoginModel loginModel);
    }
}
