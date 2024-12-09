using BookService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWebApp.Pages
{
    public class BooksModel : PageModel
    {
        public IEnumerable<Book> Books { get; private set; }
        private readonly BookServiceClient _bookService;
        /*        public int Id { get; private set; }
                public string Title { get; private set; }
                public string Author { get; private set; }
                public decimal Price { get; private set; }
                public int Quantity { get; private set; }*/
        public BooksModel(BookServiceClient bookService)
        {
            _bookService = bookService;
        }
        public async Task OnGet()
        {
            Books = await _bookService.GetAllBooksAsync();
            /*            Id = id;    
                        Title = title;
                        Author = author;
                        Price = price;
                        Quantity = quantity;*/
        }
    }
}
