using LibraryAPI; // ???

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Student> students = new List<Student>(); // List to hold students in memory
int nextId = 1;

// similar to @GetMapping which return students
app.MapGet("/students", () =>
{
    return students;
});

app.MapPost("/students", (Student newStudent) =>
{
    newStudent.Id = nextId;
    nextId++;
    students.Add(newStudent);
    return Results.Created($"/students/{newStudent.Id}", newStudent);
});

app.MapGet("/students/{id}", (int id) =>
{
    Student student = students.Find(s => s.Id == id);

    if (student == null)
    {
        return Results.NotFound("Student not found");
    }

    return Results.Ok(student);
});

app.MapPost("/students/{id}", (int id, Student updatedStudent) =>
{
    Student student = students.Find(s => s.Id == id);

    if(student == null)
    {
        Results.NotFound("Student not found");
    }

    student.Name = updatedStudent.Name;
    student.Email = updatedStudent.Email;
    return Results.Ok(student);
});

app.MapDelete("/students/{id}", (int id) =>
{
    Student student = students.Find(s => s.Id == id);

    if(student == null)
    {
        Results.NotFound("Student not found");
    }

    students.Remove(student);
    return Results.Ok($"Student with id: {id} removed from database");
});

app.MapGet("/students/random", () =>
{
    Student student = students.Find(student => student.Id == 1);

    if( student == null)
    {
        Results.NotFound("Students not found");
    }

    return Results.Ok(student.Name);
});

app.Run();