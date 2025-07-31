using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo_list.Data;
using todo_list.Models;
using todo_list.Services;

namespace todo_list.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _todoService.GetAllAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,StartDate,EndDate")] TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                await _todoService.CreateAsync(todoItem);
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        public async Task<IActionResult> Get(int? id)
        {
            if (id == null) return NotFound();
            var todoItem = await _todoService.GetByIdAsync(id.Value);
            if (todoItem == null) return NotFound();

            return View(todoItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsCompleted,StartDate,EndDate")] TodoItem todoItem)
        {
            if (id != todoItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _todoService.UpdateAsync(todoItem);

                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var todoItem = await _todoService.GetByIdAsync(id.Value);
            if (todoItem == null) return NotFound();

            return View(todoItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _todoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleIsCompleted(int id)
        {
            await _todoService.ToggleIsCompletedAsync(id);

            return RedirectToAction(nameof(Index));
        }
    } 
}