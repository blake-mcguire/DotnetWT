using DotnetAPI.Data;

var builder = WebApplication.CreateBuilder(args); //this will build the web server that the application will be running on.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((options) =>
    {
        options.AddPolicy("DevCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000") // allow these origins
                    .AllowAnyMethod() // allow any HTTP method (GET, POST, PUT, DELETE, etc.)
                    .AllowAnyHeader() // allow any HTTP header
                    .AllowCredentials();// allow credentials (cookies, authorization headers, etc.)
            });
        options.AddPolicy("ProdCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("https://myProductionSite.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });


//essentially, this is a way to add a service to the dependency injection container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
 
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();
app.Run();

