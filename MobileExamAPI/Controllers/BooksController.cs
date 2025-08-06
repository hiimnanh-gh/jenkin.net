using Microsoft.AspNetCore.Mvc;
using MobileExamAPI.Models;

namespace MobileExamAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Books.ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return Ok(book);
        }
    }
}
