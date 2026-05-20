using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Mappings;
using FluentValidation;
using FluentValidation.AspNetCore;
using TodoApi.Validators;
using TodoApi.Interfaces;
using TodoApi.Repositories;
using TodoApi.Services;
using TodoApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.CommandTimeout(30)));

builder.Services.AddAutoMapper(typeof(TodoMappingProfile));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoDtoValidator>();

builder.Services.AddScoped<ITodoRepository, TodoRepository>();

builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();

