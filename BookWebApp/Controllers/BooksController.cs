using Microsoft.AspNetCore.Mvc;

namespace BookWebApp.Controllers
{
    [ApiController]
    /*[Route("[controller]")]*/
    public class BooksController : Controller
    {
        private readonly BookServiceClient _bookService;

        public BooksController(BookServiceClient bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Books()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }
    }
}
