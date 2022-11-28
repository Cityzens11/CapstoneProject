using CapstoneProject.DomainModels;
using CapstoneProject.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly CapStoneDbContext _context;
        public ToDoListController(CapStoneDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<ToDoList> objCatlist = _context.ToDoLists;
            return View(objCatlist);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ToDoList toDoList = new ToDoList();
            return View(toDoList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoList tdlObj)
        {
            if (ModelState.IsValid)
            {
                tdlObj.CreatedDate = DateTime.Now;
                _context.ToDoLists.Add(tdlObj);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "List Added Successfully !";
                return RedirectToAction("Index");
            }

            return View(tdlObj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var tdlFromDb = _context.ToDoLists.Find(id);

            if (tdlFromDb == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "ToDoEntry", new {toDoListId = tdlFromDb.Id});
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteRecord = await _context.ToDoLists.FindAsync(id);
            if (deleteRecord == null)
            {
                return NotFound();
            }
            _context.ToDoLists.Remove(deleteRecord);
            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "List Deleted Successfully !";
            return RedirectToAction("Index");
        }
    }
}
