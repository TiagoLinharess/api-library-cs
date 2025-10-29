using api_library_cs.Communications.Request;
using api_library_cs.Communications.Response;
using api_library_cs.Data;
using api_library_cs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_library_cs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseBookJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public IActionResult create([FromBody] RequestBookJson request)
        {
            Book book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Genre = request.Genre,
                Price = request.Price,
                Stock = request.Stock
            };

            var validationResult = ValidateBook(book);

            if (validationResult != null)
            {
                return validationResult;
            }

            var normalizedTitle = book.Title.Trim().ToLowerInvariant();
            var normalizedAuthor = book.Author.Trim().ToLowerInvariant();

            var exists = _context.Books
                .AsNoTracking()
                .Any(b => b.Title.Trim().ToLower() == normalizedTitle && b.Author.Trim().ToLower() == normalizedAuthor);

            if (exists)
            {
                return Conflict("A book with the same title and author already exists.");
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            ResponseBookJson response = new ResponseBookJson
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                Price = book.Price,
                Stock = book.Stock,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt
            };

            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ResponseBookJson>), StatusCodes.Status200OK)]
        public IActionResult read()
        {
            var books = _context.Books
                .Select(book => new ResponseBookJson
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    Price = book.Price,
                    Stock = book.Stock,
                    CreatedAt = book.CreatedAt,
                    UpdatedAt = book.UpdatedAt
                })
                .ToList();

            return Ok(books);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseBookJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult read([FromRoute] Guid id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            return Ok(book);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult update([FromRoute] Guid id, [FromBody] RequestBookJson request)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            var updatedCandidate = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Genre = request.Genre,
                Price = request.Price,
                Stock = request.Stock
            };

            var validationResult = ValidateBook(updatedCandidate);

            if (validationResult != null)
            {
                return validationResult;
            }

            book.Title = updatedCandidate.Title;
            book.Author = updatedCandidate.Author;
            book.Genre = updatedCandidate.Genre;
            book.Price = updatedCandidate.Price;
            book.Stock = updatedCandidate.Stock;
            book.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult update([FromRoute] Guid id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            return NoContent();
        }

        private IActionResult? ValidateBook(Book book)
        {
            if (!book.IsTitleOrAuthorValid())
            {
                return BadRequest("Title and Author must be between 2 and 120 characters long.");
            }

            if (!book.IsGenreValid())
            {
                return BadRequest("Genre must be one of the following: ficção, romance, mistério.");
            }

            if (!book.IsPriceValid())
            {
                return BadRequest("Price must be a non-negative value.");
            }

            if (!book.IsStockValid())
            {
                return BadRequest("Stock must be a non-negative integer.");
            }

            return null;
        }
    }
}
