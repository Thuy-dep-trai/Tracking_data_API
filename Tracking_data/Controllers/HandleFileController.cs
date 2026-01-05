using Microsoft.AspNetCore.Mvc;

namespace Tracking_data.Controllers
{
    [ApiController]
    [Route("api/handleFile")]
    public class HandleFileController : Controller
    {
        private readonly string UploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        public HandleFileController()
        {
            Directory.CreateDirectory(UploadFolder);
        }

        // upload từ client 
        [HttpPost("upload")]
        public async Task<IActionResult> Upload (IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { status = "error", message = "No file uploaded" });
            var filePath = Path.Combine(UploadFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(new { status = "success", filename = file.FileName });
        }

        // 2️⃣ Download file từ server
        [HttpGet("download/{filename}")]
        public IActionResult Download(string filename)
        {
            var filePath = Path.Combine(UploadFolder, filename);
            if (!System.IO.File.Exists(filePath))
                return NotFound(new { status = "error", message = "File not found" });

            var mime = "application/octet-stream";
            // 1️⃣ PhysicalFile trả về file trực tiếp
            return PhysicalFile(filePath, "application/octet-stream", filename);
        }

    }
}
