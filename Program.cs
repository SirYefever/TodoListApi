using System.Configuration;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<TodoContext>(opt =>
//     opt.UseNpgsql(Configuration.Get));
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins,
            builder => builder.WithOrigins("http://192.168.111.137:8080", "http://192.168.111.137:8081", "http://localhost:8080/")
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });


// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//                       policy  =>
//                       {
//                           policy.WithOrigins("http://192.168.111.137:8080");
//                       });
// });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
