using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Net;
using System.Reflection.PortableExecutable;
using Tracking_data.DTO;
using Tracking_data.Hepler;
using Tracking_data.Repositories;

namespace Tracking_data.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {

        private readonly DataRepository _repo;
        public AuthController(DataRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("login")]
        public IActionResult Login( DTO.LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) ||
                string.IsNullOrEmpty(request.Password))
                return BadRequest("Thiếu dữ liệu");

            var user = _repo.GetByUsername(request.Username , request.class_name);

            if (user == null)
                return Unauthorized("Sai tài khoản hoặc mật khẩu");
            if(user.password != request.Password)
                return Unauthorized("Sai tài khoản hoặc mật khẩu");
            return Ok(new
            {
                message = "Login OK",
                emp_no = user.emp_no,
                emp_name = user.emp_name,
                emp_rank = user.emp_rank,
            });
        }
        
    }
}
