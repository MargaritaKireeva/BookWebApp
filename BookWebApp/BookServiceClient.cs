using BookService.Models;

namespace BookWebApp
{
    public class BookServiceClient
    {
        private readonly HttpClient _client;

        public BookServiceClient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("HttpBookService");
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var response = await _client.GetAsync("api/Books");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Book>>();
        }
        public async Task<Book> GetBookAsync(int id)
        {
            var response = await _client.GetAsync($"api/Books/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Book>();
        }
    }
}
