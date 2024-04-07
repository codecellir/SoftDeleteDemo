using Microsoft.EntityFrameworkCore;
using SoftDeleteDemo.Data;
using SoftDeleteDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SoftDeleteInterceptor>();


builder.Services.AddDbContext<AppDbContext>(
    (sp, option) => option
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>())
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



var bookGroup = app.MapGroup("/api/book");

bookGroup.MapPost("", async (AppDbContext dbContext, BookDTO book) =>
{
    var entity = new Book
    {
        Name = book.Name,
    };
    dbContext.Books.Add(entity);
    await dbContext.SaveChangesAsync();

    return Results.Ok(entity.Id);
});

bookGroup.MapGet("/{bookId}", async (AppDbContext dbContext, int bookId) =>
{
    var book = dbContext.Books.AsNoTracking().SingleOrDefault(_ => _.Id == bookId);

    if (book is null)
        return Results.NotFound();

    return Results.Ok(new BookDTO
    {
        Id = book.Id,
        Name = book.Name,
    });
});
bookGroup.MapGet("", async (AppDbContext dbContext) =>
{
    var books = await dbContext.Books.AsNoTracking().ToListAsync();

    return Results.Ok(books.Select(_ => new BookDTO
    {
        Id = _.Id,
        Name = _.Name,
    }));
});
bookGroup.MapDelete("/{bookId}", async (AppDbContext dbContext, int bookId) =>
{
    var book = dbContext.Books.AsNoTracking().SingleOrDefault(_ => _.Id == bookId);

    if (book is null)
        return Results.BadRequest();

    dbContext.Books.Remove(book);
    await dbContext.SaveChangesAsync();

    return Results.Ok();
});

app.Run();

