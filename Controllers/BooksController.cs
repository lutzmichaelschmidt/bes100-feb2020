using LibraryApi.Domain;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class BooksController : Controller
    {
        LibraryDataContext Context;

        public BooksController(LibraryDataContext context)
        {
            Context = context;
        }

        [HttpPost("books")]
        public async Task<ActionResult<GetABookResponse>> AddABook([FromBody] PostBookRequest bookToAdd)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Title = bookToAdd.Title,
                Author = bookToAdd.Author,
                Genre = bookToAdd.Genre,
                InInventory = true,
                NumberOfPages = bookToAdd.NumberOfPages
            };

            Context.Books.Add(book);
            await Context.SaveChangesAsync();

            var response = new GetABookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                NumberOfPages = book.NumberOfPages
            };

            return CreatedAtRoute("books#getabook", new { id = book.Id }, response);

        }

        [HttpGet("books/{id:int}", Name ="books#getabook")]
        public async Task<ActionResult<GetABookResponse>> GetABook(int id)
        {
           
            var response = await GetBooksInInventory()
                .Where(b => b.Id == id)
                .Select(b => new GetABookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Author,
                    NumberOfPages= b.NumberOfPages
                }).SingleOrDefaultAsync();

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("books")]
        public async Task<ActionResult<GetBooksResponse>> GetAllBooks([FromQuery]string genre="all")
        {
            var response = new GetBooksResponse();

            var data = GetBooksInInventory();
            
            if(genre != "all")
            {
                data = data.Where(b => b.Genre == genre);
            }

            response.Data = await data
                .Select(b => new BookSummaryItem
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author
                }).ToListAsync();
            response.Genre = genre;
            return Ok(response);
        }

        private IQueryable<Book> GetBooksInInventory()
        {
            return Context.Books.Where(b => b.InInventory);
        }
    }
}
