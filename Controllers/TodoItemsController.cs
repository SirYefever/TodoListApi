using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// using System.Web.Http;
using System.Web.Http.Cors;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace TodoApi.Controllers
{
    [System.Web.Mvc.Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private long _todoIterator = 0;
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet("GetTodoList")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoList()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("GetTodo/{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpGet("GetTodoCounter")]
        public ActionResult<string> GetTodoCounter(){
            return JsonConvert.SerializeObject(_context.TodoItems.Count());
        }

        [HttpGet("GetDoneCounter")]
        public ActionResult<string> GetDoneCounter(){
            var doneCounter = 0;
            foreach (TodoItem todo in _context.TodoItems) {
                if (todo.IsComplete) {
                    doneCounter++;
                }
            }
            var doneCounterJson = JsonConvert.SerializeObject(doneCounter);
            return doneCounterJson;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ChangeTodoName/{id}")]
        // public async Task<IActionResult> RedactTodoItem(long id, TodoItemModel todoModel) {
        public async Task<IActionResult> RedactTodoItem(long id, TodoItemModel todoModel) {
            // var isCompleteStatus = _context.TodoItems.Where(o => o.Id == id).Select(o => o.IsComplete).FirstOrDefault(); // Правильно находит, что видно в отладке.
            var redactedTodo = new TodoItem(id, todoModel.Name, todoModel.IsComplete); 
            _context.Entry(redactedTodo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("ChangeCompleteStatus/{id}")]
        public async Task<ActionResult<TodoItem>> ChangeCompleteStatus(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.IsComplete = !todoItem.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return todoItem;
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddTodo")]
        public async Task<ActionResult<TodoItem>> AddTodo(TodoItemModel todoModel)
        {
            var todoItem = new TodoItem(_todoIterator, todoModel.Name, todoModel.IsComplete);
            _todoIterator++;
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // return CreatedAtAction(nameof(GetTodoItem), todoItem);
            return todoItem;
        }


        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        [HttpPut("LoadTodoList")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> LoadTodoList(List<TodoItemModel> itemModels)
        {
            _context.TodoItems.RemoveRange(_context.TodoItems);
            foreach (var item in itemModels)
            {
                await AddTodo(item);
            }
            await _context.SaveChangesAsync();
            return await _context.TodoItems.ToListAsync();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("DeleteTodo/{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }
    }
}
