using Microsoft.Extensions.Logging;
using MimeKit;
using MVC_Messenger.Data.Repository;
using MVC_Messenger.Models;
using MVC_Messenger.Services;
using PerfomanceLib;
using Quartz;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace MVC_Messenger.Jobs
{
    public class EmailSenderJob : IJob
    {
        private IEmailSender _emailSender { get; }
        private readonly IDataBaseRepository _repository;
        private readonly ILogger<EmailSenderJob> _logger;
        public EmailSenderJob(ILogger<EmailSenderJob> logger, IDataBaseRepository repository, IEmailSender emailSender)
        {
            _repository = repository;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Job 'SendEmailsToAdminsAsync' Execute {DateTime.Now.ToLongTimeString()}");

            Email newEmail = new Email();
            InternetAddressList emailsList = new InternetAddressList();

            var emails = await _repository.GetAllAdminsEmail();            
            var em = emails.ToList();
            foreach (var email in em)
            {
                emailsList.Add(new MailboxAddress(email, email));
            }
            newEmail.Sender = "internal Job Perfomance";

            IPerfomance perfomance = new Perfomance();
            var outerModel = perfomance.GetPerfomanceData();

            PerfomanceModel model = new PerfomanceModel()
            {
                Cpu = outerModel.Cpu,
                Ram = outerModel.Ram,
                NetWork = outerModel.NetWork,
                Hdd = outerModel.Hdd,
            };

            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", "TemplateEmailSenderJob.cshtml");
            var emailHtmlBody = File.ReadAllText(templatePath);

            emailHtmlBody = Engine.Razor.RunCompile(emailHtmlBody, "keyJob", typeof(PerfomanceModel), model);

            newEmail.SendDateTime = DateTime.Now;
            newEmail.Subject = "Данные о состоянии 'Итоговой проект' за " + newEmail.SendDateTime;
            newEmail.Message = emailHtmlBody;            

            //Save to DataBase
            await _repository.SetNewEmailList(newEmail, emailsList);

            // send
            await _emailSender.SendEmailsToAdminsAsync(
                emailsList,
                newEmail.Subject,
                newEmail.Message);
        }
    }
}
