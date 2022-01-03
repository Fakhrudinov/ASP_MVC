using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MVC_Messenger.Data;
using MVC_Messenger.Data.Repository;
using MVC_Messenger.Models;
using MVC_Messenger.Services;
using RazorEngine;
using RazorEngine.Templating;

namespace MVC_Messenger.Controllers
{
    public class UsersController : Controller
    {
        private readonly MessengerContext _context;
        private readonly IDataBaseRepository _repository;
        private readonly ILogger<UsersController> _logger;
        private IEmailSender _emailSender { get; }
        public PaginationOptions _options { get; set; }

        public UsersController(
            ILogger<UsersController> logger, 
            MessengerContext context, 
            IDataBaseRepository repository, 
            IEmailSender emailSender, 
            IOptions<PaginationOptions> options)
        {
            _context = context;
            _repository = repository;
            _logger = logger;
            _emailSender = emailSender;
            _options = options.Value;
        }

        [Authorize(Roles = "admin, user")]
        // GET: Users
        public async Task<IActionResult> Index(int page = 1)
        {
            _logger.LogInformation($"Index page #{page} visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");

            int pageSize = _options.UserItemsCount; // numbers of elements on page

            var count = await _context.Users.CountAsync();
            var items = await _context.Users
                .Include(u => u.Role)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PaginationViewUsers viewModel = new PaginationViewUsers
            {
                PageViewModel = pageViewModel,
                Users = items
            };
            return View(viewModel);
        }

        [Authorize(Roles = "admin, user")]
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation($"Warning Details page {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' = id is null");
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                _logger.LogInformation($"Warning Details page {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' = user is null");
                return NotFound();
            }

            _logger.LogInformation($"Details page visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}': Get info about userId={user.UserID}");
            return View(user);
        }

        [Authorize(Roles = "admin")]
        // GET: Users/Create
        public IActionResult Create()
        {
            _logger.LogInformation($"Create page visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Name,Password,Email,PhoneNumber,IsPhoneNumberConfirmed,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Create action with walid model at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}', " +
                    $"model is Email={user.Email} Name={user.Name} Role={user.Role} Password={user.Password} PhoneNumber={user.PhoneNumber}");
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        [Authorize(Roles = "admin")]
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation($"Warning Edit page {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}) = id is null");
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogInformation($"Warning Edit page {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'  id({id}) = user is null");
                return NotFound();
            }

            _logger.LogInformation($"Edit page for id({id}) visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        [Authorize(Roles = "admin")]
        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Name,Password,Email,PhoneNumber,IsPhoneNumberConfirmed,RoleId")] User user)
        {
            if (id != user.UserID)
            {
                _logger.LogInformation($"Warning Edit Action {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}) != user.UserID {user.UserID}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        _logger.LogInformation($"Warning Edit Action {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}) = NotFound() user.UserID {user.UserID}");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _logger.LogInformation($"Edit Action ok {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' " +
                    $"Email={user.Email} Name={user.Name} Role={user.Role} Password={user.Password} PhoneNumber={user.PhoneNumber}");
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation($"Edit Action model state is invalid {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' " +
                $"Email={user.Email} Name={user.Name} Role={user.Role} Password={user.Password} PhoneNumber={user.PhoneNumber}");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        [Authorize(Roles = "admin")]
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation($"Warning Delete Action {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}), id == null");
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                _logger.LogInformation($"Warning Delete Action {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}), user == null");
                return NotFound();
            }

            _logger.LogInformation($"Delete Action ok {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}) ");
            return View(user);
        }

        [Authorize(Roles = "admin")]
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"DeleteConfirmed Action {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}) ");
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            _logger.LogInformation($"UserExists Action {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}) ");
            return _context.Users.Any(e => e.UserID == id);
        }

        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> SendEmail(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation($"Warning SendEmail page {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}) = id is null");
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogInformation($"Warning SendEmail page {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'  id({id}) = user is null");
                return NotFound();
            }

            _logger.LogInformation($"SendEmail form visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            return View();
        }

        [Authorize(Roles = "admin, user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailAsync(int id, Email newEmail)
        {
            if (!UserExists(id))
            {
                _logger.LogInformation($"Warning SendEmailAsync Action {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}' id({id}) = NotFound()");
                return NotFound();
            }

            ViewData["Message"] = null;
            ViewData["Error"] = null;

            if (ModelState.IsValid)
            {
                _logger.LogInformation($"SendEmailAsync by '{User.Identity.Name}' with correct models {DateTime.Now.ToLongTimeString()}" +
                    $", To userId={id} subj={newEmail.Subject} message={newEmail.Message}");

                //receiver email
                var recieverEmail = await _repository.GetUserEmailAsync(id);
                MailboxAddress emailAdress = new MailboxAddress(recieverEmail, recieverEmail);
                newEmail.Receiver = recieverEmail;
                newEmail.Sender = User.Identity.Name;

                //compile email body to template
                var templatePath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    "EmailTemplates", 
                    "TemplateEmailUserToUser.cshtml");
                var emailHtmlBody = System.IO.File.ReadAllText(templatePath);
                emailHtmlBody = Engine.Razor.RunCompile(emailHtmlBody, "keySingleEmail", typeof(Email), newEmail);

                newEmail.Subject = $"'Итоговой проект' сообщение от {User.Identity.Name}: {newEmail.Subject}";
                newEmail.Message = emailHtmlBody;
                newEmail.SendDateTime = DateTime.Now;

                //Save to DataBase
                await _context.Emails.AddAsync(newEmail);
                await _context.SaveChangesAsync();

                //send
                await _emailSender.SendEmailFromUserToUserAsync(
                    emailAdress,
                    newEmail.Subject,
                    newEmail.Message);

                ViewData["message_color"] = "green";
                ViewData["Message"] = "*Your Message Has Been Sent.";
            }
            else
            {
                _logger.LogInformation($"warning SendEmailAsync by '{User.Identity.Name}' To userId={id}: " +
                    $"incorrect models {DateTime.Now.ToLongTimeString()}, " +
                    $"subj={newEmail.Subject} message={newEmail.Message}");
                
                ViewData["message_color"] = "red";
                ViewData["Message"] = "*Please complete the required fields";
            }

            ModelState.Clear();
            return View();
        }
    }
}
