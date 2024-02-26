using Microsoft.Extensions.Options;
using System.Text;

namespace dEV4
{
    public class Books
    {
        public List<Book> books { get; set; }
    }
    public class Book
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Year { get; set; }
    }
    public class BooksMiddleware
    {
        private readonly RequestDelegate _next;
        public Books books { get; }

        public BooksMiddleware(RequestDelegate next, IOptions<Books> options)
        {
            _next = next;
            books = options.Value;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/Library/Books")
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (Book book in books.books)
                {
                    stringBuilder.Append($"<p>Name: {book.Name}</li>");
                    stringBuilder.Append($"<p>Country: {book.Country}</li>");
                    stringBuilder.Append($"<p>Year: {book.Year}</li>");

                    stringBuilder.Append("</ul>");
                    stringBuilder.Append("<hr>");
                }
                await context.Response.WriteAsync(stringBuilder.ToString());
            }
            await _next.Invoke(context);
        }
    }
}