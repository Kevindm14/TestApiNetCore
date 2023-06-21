using Microsoft.AspNetCore.Mvc;
using TestApi.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TestApi.Models.DTO;

namespace TestApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _dbContext;

        public BookController(IMapper mapper, ApplicationDBContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            try
            {
                var books = await _dbContext.Books.ToListAsync();

                var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(books);

                return Ok(booksDTO);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var book = await _dbContext.Books.FindAsync(id);
                if (book == null) { return NotFound(); }

                var bookDTO = _mapper.Map<BookDTO>(book);
                return Ok(bookDTO);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookDTO bookDTO)
        {
            try
            {
                var book = _mapper.Map<Book>(bookDTO);
                book.CreatedAt = DateTime.Now;

                _dbContext.Add(book);
                await _dbContext.SaveChangesAsync();

                var bookDto = _mapper.Map<BookDTO>(book);

                return CreatedAtAction("Get", new { id = bookDto.Id }, bookDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, BookDTO bookDTO)
        {
            try
            {
                var book = _mapper.Map<Book>(bookDTO);
                if (id != book.Id) { return BadRequest(); }

                var bookItem = await _dbContext.Books.FindAsync(id);
                if (book == null) { return NotFound(); }

                bookItem!.Title = book.Title;
                bookItem!.Description = book.Description;
                bookItem!.Author = book.Author;

                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var book = await _dbContext.Books.FindAsync(id);
                if (book == null) { return NotFound(); }

                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
