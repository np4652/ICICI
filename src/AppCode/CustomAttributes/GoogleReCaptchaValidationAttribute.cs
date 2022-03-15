using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

//Refrece: https://dejanstojanovic.net/aspnet/2018/may/using-google-recaptcha-v2-in-aspnet-core-web-application/
namespace ICICI.AppCode.CustomAttributes
{
    public class GoogleReCaptchaValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Lazy<ValidationResult> errorResult = new Lazy<ValidationResult>(() => new ValidationResult("Google reCAPTCHA validation failed", new String[] { validationContext.MemberName }));

            if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            {
                return errorResult.Value;
            }

            IConfiguration configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
            String reCaptchResponse = value.ToString();
            String reCaptchaSecret = configuration.GetValue<String>("GoogleReCaptcha:SecretKey");


            HttpClient httpClient = new HttpClient();
            var httpResponse = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={reCaptchaSecret}&response={reCaptchResponse}").Result;
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return errorResult.Value;
            }

            String jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(jsonResponse);
            if (jsonData.success != true.ToString().ToLower())
            {
                return errorResult.Value;
            }
            return ValidationResult.Success;
        }
    }

    public abstract class GoogleReCaptchaModelBase
    {
        [Required]
        [GoogleReCaptchaValidation]
        [BindProperty(Name = "g-recaptcha-response")]
        public string GoogleReCaptchaResponse { get; set; }
    }

    public static class GoogleReCaptchaTagHelper
    {
        public static IHtmlContent GoogleReCaptcha(this IHtmlHelper htmlHelper, String siteKey, String callback = null)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.Attributes.Add("class", "g-recaptcha");
            tagBuilder.Attributes.Add("data-sitekey", siteKey);
            if (callback != null && String.IsNullOrWhiteSpace(callback))
            {
                tagBuilder.Attributes.Add("data-callback", callback);
            }
            using (var writer = new StringWriter())
            {
                tagBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                var htmlOutput = writer.ToString();
                return htmlHelper.Raw(htmlOutput);
            }
        }
    }
}

