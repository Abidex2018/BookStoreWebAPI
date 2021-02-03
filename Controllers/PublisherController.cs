using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly BookStoresDBContext _context;

        public PublisherController(BookStoresDBContext context)
        {
            _context = context;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublisher()
        {
            return await _context.Publishers.ToListAsync();
        }
      
        
        [HttpGet("PostPublisherDetails")]
        public async Task<ActionResult<Publisher>> PostPublisherDetails()
        {
            var publisher = new Publisher();

            publisher.City = "New York City";
            publisher.Country = "USA";
            publisher.State = "NY";
            publisher.PublisherName = "Harper & Brothers";

            var book1= new Book();

            book1.Title = "Good Night moon -1";
            book1.PublishedDate = DateTime.Now;

            var book2 = new Book();

            book2.Title = "Good Night moon -2";
            book2.PublishedDate = DateTime.Now;

            var sale1 = new Sale();

            sale1.Quantity = 2;
            sale1.StoreId = "8042";
            sale1.OrderDate = DateTime.Now;
            sale1.OrderNum = "XYZ";
            sale1.PayTerms = "Nat 30";

            var sale2 = new Sale();

            sale2.Quantity = 2;
            sale2.StoreId = "7131";
            sale2.OrderDate = DateTime.Now;
            sale2.OrderNum = "QA879.1";
            sale2.PayTerms = "Nat 20";

            book1.Sales.Add(sale1);
            book2.Sales.Add(sale2);


            publisher.Books.Add(book1);
            publisher.Books.Add(book2);

            _context.Publishers.Add(publisher);
            _context.SaveChanges();

            var publishers = _context.Publishers
                .Include(pub => pub.Books)
                .ThenInclude(book => book.Sales)
                .Include(pub => pub.Users)
                .ThenInclude(user => user.Job)
                .FirstOrDefault(x => x.PubId == publisher.PubId);

            if (publishers == null)
            {
                return NotFound();
            }

            return publisher;
        }


        // GET: api/Publishers/5
        [HttpGet("GetPublisherDetails/{id}")]
        public async Task<ActionResult<Publisher>> GetPublisherDetails(int id)
        {
            var publisher = await _context.Publishers.SingleAsync(pub => pub.PubId == id);

            _context.Entry(publisher)
                .Collection(pub=>pub.Users)
                .Query()
                .Where(user=>user.EmailAddress.Contains("karin"))
                .Load();

            _context.Entry(publisher)
                .Collection(pub=>pub.Books)
                .Query()
                .Include(book=>book.Sales)
                .Load();

           // var user = _context.Users.SingleAsync(usr => usr.UserId == 1);


           return publisher;
        }



        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Publishers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publisher>> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return publisher;
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.PubId == id);
        }
    }
}
