using Microsoft.EntityFrameworkCore;
using LibraryAPI; // ???

var builder = WebApplication.CreateBuilder(args);

// builder.Service is dependency injection container.
// LibraryContext is database class
// AddDbContext adds your database context to the dependency injection container,
// allowing you to inject it into your controllers or other services.
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite("Data Source=library.db")); // 

var app = builder.Build();

//List<Student> students = new List<Student>(); // List to hold students in memory
//int nextId = 1;

// similar to @GetMapping which return students
app.MapGet("/students", async (LibraryContext db) =>
{
    return await db.Students.ToListAsync();
});

app.MapPost("/students", async (LibraryContext db, Student newStudent) =>
{
    db.Students.Add(newStudent);
    await db.SaveChangesAsync();
    return Results.Created($"/students/{newStudent.Id}", newStudent);
});

app.MapGet("/students/{id}", async (LibraryContext db, int id) =>
{
    Student student = await db.Students.FindAsync(id);

    if (student == null)
    {
        return Results.NotFound("Student not found");
    }

    return Results.Ok(student);
});

app.MapPost("/students/{id}", async (LibraryContext db, int id, Student updatedStudent) =>
{
    Student student = await db.Students.FindAsync(id); // Change tracker is tracking the fetched object,
                                                       // so any changes made to it will be automatically detected and saved
                                                       // to the database when SaveChangesAsync is called.

    if (student == null)
    {
        return Results.NotFound("Student not found");
    }

    student.Name = updatedStudent.Name;
    student.Email = updatedStudent.Email;
    await db.SaveChangesAsync();
    return Results.Ok(student);
});

app.MapDelete("/students/{id}", async (LibraryContext db, int id) =>
{
    Student student = await db.Students.FindAsync(id);

    if(student == null)
    {
        return Results.NotFound("Student not found");
    }

    //students.Remove(student);
    db.Students.Remove(student);
    await db.SaveChangesAsync();
    return Results.Ok($"Student with id: {id} removed from database");
});

//app.MapGet("/students/random", () =>
//{
//    Student student = students.Find(student => student.Id == 1);

//    if( student == null)
//    {
//        Results.NotFound("Students not found");
//    }

//    return Results.Ok(student.Name);
//});

app.Run();