using System.Net;

namespace Tracking_data.Hepler
{
    public class Ftp_helper
    {
        public static string DownloadImageFromFtp(
        string host,
        string user,
        string password,
        string fileName,
        string remoteFolder = "/WAR_ROOM/FACE",
        string saveFolder = "temp_images")
        {
            // ftp://ip/path/file.jpg
            string ftpUrl = $"ftp://{host}{remoteFolder}/{fileName}";

            // Tạo thư mục local nếu chưa có
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            string localPath = Path.Combine(saveFolder, fileName);

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(user, password);
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = false;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream ftpStream = response.GetResponseStream())
                using (FileStream fileStream = new FileStream(localPath, FileMode.Create))
                {
                    ftpStream.CopyTo(fileStream);
                }

                return localPath; // ✅ giống Python return local_path
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ FTP download error: {ex.Message}");
                return null;
            }
        }
    }
}
