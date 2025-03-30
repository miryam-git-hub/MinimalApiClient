using TodoApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql")));



builder.Services.AddDbContext<ToDoDbContext>(options => 
    options.UseMySql(connectionString, 
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0 -mysql")));


 builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));
builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseCors("MyPolicy");

app.MapControllers();
//===========
app.MapGet("/", () => "Hello World!");

app.MapGet("/item", async (ToDoDbContext db) => 
{
    var items = await db.Items.ToListAsync();
    foreach (var i in items)
    {
        Console.WriteLine(i); // הדפס את הפריט
    }
    return Results.Ok(items); // תיקון של 'resulte' ל-'Results'
});

//===========
app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    if (await db.Items.FindAsync(id) is Item Id)
    {
        db.Items.Remove(Id);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

//===========
app.MapPut("/item/{id}", async (int id,[FromBody] Item update, ToDoDbContext db) =>
{
    Console.WriteLine("setCompleted", update);

    var item = await db.Items.FindAsync(id);

    if (item is null) return Results.NotFound();
    // item.Name=update.Name;
   item.IsComplete = update.IsComplete;
    // item.IsComplete = true;
    
    await db.SaveChangesAsync();
    return Results.Text($"עודכן בהצלחה {item.Name} {item.IsComplete}");

    // return Results.NoContent();
});


//===========
app.MapPost("/item",async ( Item InputItem,ToDoDbContext db)=>{
var newItem = new Item();
newItem.Name=InputItem.Name;
newItem.IsComplete=InputItem.IsComplete;
db.Items.Add(newItem);
  await db.SaveChangesAsync();
return Results.Text($"נוסף בהצלחה{newItem.Id}");

   // return Results.NoContent();
});

app.Run();
