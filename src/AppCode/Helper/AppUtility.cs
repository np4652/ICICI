
using ICICI.AppCode.CustomAttributes;
using ICICI.AppCode.Reops;
using ICICI.AppCode.Reops.Entities;
using ICICI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ICICI.AppCode.Helper
{

    public class AppUtility
    {
        public static AppUtility O => instance.Value;
        private static Lazy<AppUtility> instance = new Lazy<AppUtility>(() => new AppUtility());
        private AppUtility() { }
        public Response UploadFile(FileUploadModel request)
        {
            var response = Validate.O.IsFileValid(request.file);
            if (response.StatusCode == ResponseStatus.Success)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(request.FilePath);
                    if (!Directory.Exists(sb.ToString()))
                    {
                        Directory.CreateDirectory(sb.ToString());
                    }
                    var filename = ContentDispositionHeaderValue.Parse(request.file.ContentDisposition).FileName.Trim('"');
                    string originalExt = Path.GetExtension(filename).ToLower();
                    string[] Extensions = { ".png", ".jpeg", ".jpg" };
                    if (Extensions.Contains(originalExt))
                    {
                        //originalExt = ".jpg";
                    }
                    string originalFileName = Path.GetFileNameWithoutExtension(filename).ToLower() + originalExt;
                    if (!string.IsNullOrEmpty(request.FileName))
                    {
                        request.FileName = Path.GetFileNameWithoutExtension(request.FileName).ToLower() + originalExt;
                    }
                    request.FileName = string.IsNullOrEmpty(request.FileName) ? originalFileName.Trim() : request.FileName;
                    sb.Append(request.FileName);
                    using (FileStream fs = File.Create(sb.ToString()))
                    {
                        request.file.CopyTo(fs);
                        fs.Flush();
                        if (request.IsThumbnailRequired)
                        {
                            GenrateThumbnail(request.file, request.FileName,20L);
                        }
                    }

                   
                    response.StatusCode = ResponseStatus.Success;
                    response.ResponseText = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    response.ResponseText = "Error in file uploading. Try after sometime...";
                   // response.Error = ex.Message;
                }
            }
            return response;
        }
        public bool GenrateThumbnail(IFormFile file, string fileName, long quality = 20L)
        {
            string tempImgNameWithPath = string.Concat(FileDirectories.Thumbnail,fileName);
            var newimg = new Bitmap(file.OpenReadStream());
            ImageCodecInfo jgpEncoder = GetEncoderInfo("image/jpeg");
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            try
            {
                if (File.Exists(tempImgNameWithPath))
                {
                    File.Delete(tempImgNameWithPath);
                }
                newimg.Save(tempImgNameWithPath, jgpEncoder, myEncoderParameters);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        public async Task SendMail(EmailSettings setting)
        {
            await Task.Delay(0);
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(setting.EmailFrom);
                    mail.To.Add(setting.EmailTo);
                    mail.Subject = setting.Subject;
                    mail.Body = setting.Body;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment
                    using (SmtpClient smtp = new SmtpClient(setting.HostName, setting.Port))
                    {
                        smtp.Credentials = new NetworkCredential(setting.EmailFrom, setting.Password);
                        smtp.EnableSsl = setting.EnableSSL;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
