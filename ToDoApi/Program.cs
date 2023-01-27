
using Microsoft.EntityFrameworkCore;
using ToDoApi;
using ToDoApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(op => op.AddPolicy("TodoPolicy", c=>
{
	c.AllowAnyHeader()
	.AllowAnyMethod()
	.AllowAnyOrigin();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors("TodoPolicy");
app.MapGroup("/todolist/v1")
	.MapTodosApi()
	.WithTags("Todo Endpoints");

app.Run();

