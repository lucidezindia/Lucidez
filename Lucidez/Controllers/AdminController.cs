using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lucidez.Data;
using Lucidez.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lucidez.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Dashboard()
        {
            // summary stats
            var totalLeads = await _db.Leads.CountAsync();
            var unreadContacts = await _db.ContactMessages.CountAsync(m => !m.Handled);
            var totalChats = await _db.ChatMessages.CountAsync();
            var leadsLast24h = await _db.Leads.CountAsync(l => l.CreatedAt >= DateTime.UtcNow.AddHours(-24));
            var chatsLast24h = await _db.ChatMessages.CountAsync(c => c.SentAt >= DateTime.UtcNow.AddHours(-24));

            // recent records (for tables)
            var recentLeads = await _db.Leads.OrderByDescending(l => l.CreatedAt).Take(50).ToListAsync();
            var recentContacts = await _db.ContactMessages.OrderByDescending(m => m.CreatedAt).Take(50).ToListAsync();
            var recentChats = await _db.ChatMessages.OrderByDescending(c => c.SentAt).Take(200).ToListAsync();

            // prepare 7-day series for chart (UTC days)
            var start = DateTime.UtcNow.Date.AddDays(-6); // 7 days including today
            var leadSeries = new List<int>();
            var contactSeries = new List<int>();
            var labels = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                var day = start.AddDays(i);
                labels.Add(day.ToString("MMM dd"));
                leadSeries.Add(await _db.Leads.CountAsync(l => l.CreatedAt >= day && l.CreatedAt < day.AddDays(1)));
                contactSeries.Add(await _db.ContactMessages.CountAsync(m => m.CreatedAt >= day && m.CreatedAt < day.AddDays(1)));
            }

            // pass to view via ViewBag
            ViewBag.TotalLeads = totalLeads;
            ViewBag.UnreadContacts = unreadContacts;
            ViewBag.TotalChats = totalChats;
            ViewBag.LeadsLast24h = leadsLast24h;
            ViewBag.ChatsLast24h = chatsLast24h;

            ViewBag.Labels = labels; // List<string>
            ViewBag.LeadSeries = leadSeries; // List<int>
            ViewBag.ContactSeries = contactSeries; // List<int>

            ViewBag.RecentLeads = recentLeads;
            ViewBag.RecentContacts = recentContacts;

            return View(recentChats); // model = recentChats
        }

        [HttpPost]
        public async Task<IActionResult> MarkHandled(int id)
        {
            var msg = await _db.ContactMessages.FindAsync(id);
            if (msg != null)
            {
                msg.Handled = true;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }
    }
}
