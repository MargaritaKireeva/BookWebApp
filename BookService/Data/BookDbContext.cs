using BookService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MassTransit;

namespace BookService.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }

        public DbSet<Outbox> OutBoxMessages { get; set; }
    }
}
