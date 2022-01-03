using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MVC_Messenger.Data;
using MVC_Messenger.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Messenger.Controllers
{
    public class EmailController : Controller
    {
        private readonly ILogger<EmailController> _logger;
        private readonly MessengerContext _context;
        public PaginationOptions _options { get; set; }

        public EmailController(ILogger<EmailController> logger, MessengerContext context, IOptions<PaginationOptions> options)
        {
            _context = context;
            _logger = logger;
            _options = options.Value;
        }

        [Authorize(Roles = "admin")]
        // GET: Emails with pagination
        public async Task<IActionResult> Index(int page = 1)
        {
            _logger.LogInformation($"Index page #{page} visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");

            int pageSize = _options.EmailItemsCount; // numbers of elements on page

            var count = await _context.Emails.CountAsync();
            var items = await _context.Emails
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PaginationViewEmails viewModel = new PaginationViewEmails
            {
                PageViewModel = pageViewModel,
                Emails = items
            };
            return View(viewModel);
        }

        [Authorize(Roles = "admin, user")]
        // GET: Sended
        public async Task<IActionResult> Sended(int page = 1)
        {
            _logger.LogInformation($"Sended page #{page} visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");

            int pageSize = _options.EmailItemsCount; // numbers of elements on page

            var count = await _context.Emails
                .Where(e => e.Sender.Equals(User.Identity.Name))
                .CountAsync();
            var items = await _context.Emails
                .Where(e => e.Sender.Equals(User.Identity.Name))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PaginationViewEmails viewModel = new PaginationViewEmails
            {
                PageViewModel = pageViewModel,
                Emails = items
            };
            return View(viewModel);
        }

        [Authorize(Roles = "admin, user")]
        // GET: Received
        public async Task<IActionResult> Received(int page = 1)
        {
            _logger.LogInformation($"Received page #{page} visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");

            int pageSize = _options.EmailItemsCount; // numbers of elements on page

            var count = await _context.Emails
                .Where(e => e.Receiver.Equals(User.Identity.Name))
                .CountAsync();
            var items = await _context.Emails
                .Where(e => e.Receiver.Equals(User.Identity.Name))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PaginationViewEmails viewModel = new PaginationViewEmails
            {
                PageViewModel = pageViewModel,
                Emails = items
            };
            return View(viewModel);
        }
    }
}
