using CapstoneProject.DBContext;
using CapstoneProject.DomainModels;
using CapstoneProject.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.Controllers
{
    public class ToDoEntryController : Controller
    {
        private readonly CapStoneDbContext _context;

        public ToDoEntryController(CapStoneDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int toDoListId)
        {
            ViewBag.ToDoListId = toDoListId;
            ViewBag.ToDoListName = _context.ToDoLists.Find(toDoListId)?.Name;

            var temp = _context.ToDoEntries.Where(s => s.ToDoListId == toDoListId).ToList();
            return View(temp);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.Id = id;
            ToDoEntry toDoEntry = new ToDoEntry();
            return View(toDoEntry);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name, Description, DueDate, CreatedDate, Status, ToDoListId")] ToDoEntry tdeObj)
        {
            if (tdeObj.DueDate < DateTime.Now)
            {
                if (TempData != null)
                    TempData["DueDate"] = "DueDate is Wrong !";

                return View(tdeObj);
            }

            if (ModelState.IsValid)
            {
                tdeObj.CreatedDate = DateTime.Now;
                tdeObj.ToDoList = _context.ToDoLists.Find(tdeObj.ToDoListId);

                _context.ToDoEntries.Add(tdeObj);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "Node Added Successfully !";

                return RedirectToAction("Index", new {toDoListId = tdeObj.ToDoListId});
            }

            return View(tdeObj);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tdeFromDb = _context.ToDoEntries.Find(id);

            if (tdeFromDb == null)
            {
                return NotFound();
            }

            return View(tdeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ToDoEntry tdeObj)
        {
            if (tdeObj.DueDate < DateTime.Now)
            {
                TempData["DueDate"] = "DueDate is Wrong";
                return View(tdeObj);
            }

            if (ModelState.IsValid)
            {
                _context.ToDoEntries.Find(tdeObj.Id).Name = tdeObj.Name;
                _context.ToDoEntries.Find(tdeObj.Id).Description = tdeObj.Description;
                _context.ToDoEntries.Find(tdeObj.Id).DueDate = tdeObj.DueDate;
                _context.ToDoEntries.Find(tdeObj.Id).Status = tdeObj.Status;
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "Node Updated Successfully !";
                return RedirectToAction("Index", new {toDoListId = tdeObj.ToDoListId});
            }

            return View(tdeObj);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteRecord = _context.ToDoEntries.Find(id);
            if (deleteRecord == null)
            {
                return NotFound();
            }

            _context.ToDoEntries.Remove(deleteRecord);
            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "Node Deleted Successfully !";
            return RedirectToAction("Index", new {toDoListId = deleteRecord.ToDoListId});
        }
    }
}
