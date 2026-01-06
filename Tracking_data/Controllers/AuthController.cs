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
        [HttpPost("user_info")]
        public IActionResult get_emp_info(DTO.AVC_emp request)
        {
            if (string.IsNullOrEmpty(request.emp_no) && string.IsNullOrEmpty(request.c_mail))
                return BadRequest(new { message =" không có dữ liệu"});
            var user_info = _repo.user_info(request.emp_no,request.c_mail);
            if (user_info == null)
                return Unauthorized(new { message = " không tồn tại" });
            return Ok(new
            {
                message = "OK",
                emp_no = user_info.emp_no,
                emp_name =user_info.emp_name,
                emp_birthday = user_info.emp_birthday,
                emp_inAVCdate = user_info.emp_inAVCdate,
                emp_address =user_info.emp_address,
                emp_qdate = user_info.emp_qdate,
                email = user_info.email,
            }
            );
            

        }

        // 2️⃣ Download file từ server
        [HttpGet("user_image/{emp_no}")]
        public IActionResult getimagebyuser(string emp_no)
        {
            string[] exts = [".jpg", ".png"];
            var localPath = Path.Combine(Directory.GetCurrentDirectory(), "Image");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Image");
            foreach (var ext in exts)
            {
                filePath = Ftp_helper.DownloadImageFromFtp("10.120.15.43", "cncuser", "cnc%123", emp_no+ext, "/WAR_ROOM/FACE", localPath);
                if (System.IO.File.Exists(filePath))
                    break;
            }                
            if (!System.IO.File.Exists(filePath))
                return NotFound(new { status = "error", message = "File not found !" });
            
            // 1️⃣ PhysicalFile trả về file trực tiếp
            return PhysicalFile(filePath, "image/jpeg");
        }
    }
}
