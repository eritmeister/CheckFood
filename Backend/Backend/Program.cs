using Microsoft.EntityFrameworkCore;
using WebApi1;

var builder = WebApplication.CreateBuilder(args);

//Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(
    options => 
        options.UseSqlServer(("Server=prices.caxoaki41hpz.us-east-1.rds.amazonaws.com,1433;" +
                              "Database=Products;User ID=admin;Password=Yan31083008")));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();