using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using MVC_Messenger.Data.Repository;
using MVC_Messenger.Models;
using MVC_Messenger.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;


namespace MVC_Messenger.Controllers
{
    public class HomeController : Controller
    {
        private IEmailSender _emailSender { get; }
        private readonly IDataBaseRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, IDataBaseRepository repository)
        {
            _logger = logger;
            _emailSender = emailSender;
            _repository = repository;
        }

        public IActionResult Index()
        {
            _logger.LogInformation($"Index page visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation($"Privacy page visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string message = $"Error  {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' " + Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError(message);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SendEmails()
        {
            _logger.LogInformation($"SendEmails visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailsAsync(Email newEmail)
        {
            ViewData["Message"] = null;
            ViewData["Error"] = null;

            if (ModelState.IsValid)
            {
                _logger.LogInformation($"SendEmailsAsync by '{User.Identity.Name}' with correct models {DateTime.Now.ToLongTimeString()}" +
                    $", subj={newEmail.Subject} message={newEmail.Message}");

                //receiver email list
                InternetAddressList emailsList = new InternetAddressList();
                var emails = await _repository.GetAllUsersEmail();
                var em = emails.ToList();
                foreach (var email in em)
                {
                    emailsList.Add(new MailboxAddress(email, email));
                }

                newEmail.Sender = User.Identity.Name;

                //compile email body to template
                var templatePath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    "EmailTemplates", 
                    "TemplateEmailAdminToAllUsers.cshtml");
                var emailHtmlBody = System.IO.File.ReadAllText(templatePath);

                newEmail.Subject = "Информация от администрации: " + newEmail.Subject;
                newEmail.Message = Engine.Razor.RunCompile(emailHtmlBody, "keyAdmin", typeof(Email), newEmail);
                newEmail.SendDateTime = DateTime.Now;

                //Save to DataBase
                await _repository.SetNewEmailList(newEmail, emailsList);

                //send
                await _emailSender.SendEmailsToAdminsAsync(
                    emailsList,
                    newEmail.Subject,
                    newEmail.Message);

                ViewData["message_color"] = "green";
                ViewData["Message"] = "*Your Message Has Been Sent.";
            }
            else
            {
                _logger.LogInformation($"warning SendEmailsAsync by '{User.Identity.Name}' incorrect models {DateTime.Now.ToLongTimeString()}, " +
                    $"subj={newEmail.Subject} message={newEmail.Message}");
                
                ViewData["message_color"] = "red";
                ViewData["Message"] = "*Please complete the required fields";
            }

            ModelState.Clear();
            return View();
        }

    }
}
