using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi;

public static class TodoEndpoints
{
	public static RouteGroupBuilder MapTodosApi(this RouteGroupBuilder group)
	{

		group.MapGet("/", GetAllTodos);
		group.MapGet("/{id}", GetTodo);
		group.MapPost("/", CreateTodo);
		group.MapPut("/{id}", UpdateTodo);
		group.MapDelete("/{id}", DeleteTodo);

		return group;
	}
		static async Task<IResult> GetAllTodos(TodoDb db)
		{
			return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
		}

		static async Task<IResult> GetTodo(int id, TodoDb db)
		{
			return await db.Todos.FindAsync(id)
				is Todo todo
					? TypedResults.Ok(new TodoItemDTO(todo))
					: TypedResults.NotFound();
		}

		static async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, TodoDb db)
		{
			var todoItem = new Todo
			{
				IsComplete = todoItemDTO.IsComplete,
				Name = todoItemDTO.Name
			};

			db.Todos.Add(todoItem);
			await db.SaveChangesAsync();

			return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
		}

		static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, TodoDb db)
		{
			var todo = await db.Todos.FindAsync(id);

			if (todo is null) return TypedResults.NotFound();

			//todo.Name = todoItemDTO.Name;
			todo.IsComplete = todoItemDTO.IsComplete;

			await db.SaveChangesAsync();

			return TypedResults.NoContent();
		}

		static async Task<IResult> DeleteTodo(int id, TodoDb db)
		{
			if (await db.Todos.FindAsync(id) is Todo todo)
			{
				db.Todos.Remove(todo);
				await db.SaveChangesAsync();
				return TypedResults.Ok(todo);
			}

			return TypedResults.NotFound();
		}
	}

