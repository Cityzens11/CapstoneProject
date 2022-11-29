using CapstoneProject.DBContext;
using CapstoneProject.DomainModels;
using CapstoneProject.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.Controllers
{
    /// <summary>
    /// Base class(controller) for working with ToDoEntries of a ToDoList.
    /// </summary>
    public class ToDoEntryController : Controller
    {
        private readonly CapStoneDbContext _context;

        /// <summary>
        /// Gets a reference to existing DataBase in a controller.
        /// </summary>
        public ToDoEntryController(CapStoneDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets Data from DbContext and Displays all ToDoEntries of
        /// a given ToDoList <paramref name="toDoListId"/> in a DataBase.
        /// </summary>
        /// <param name="toDoListId">Id of a ToDoList, entries of which to display.</param>
        /// <returns>The created <see cref="ViewResult"/> with all ToDoEnrties of given ToDoList.</returns>
        public IActionResult Index(int toDoListId)
        {
            ViewBag.ToDoListId = toDoListId;
            ViewBag.ToDoListName = _context.ToDoLists.Find(toDoListId)?.Name;

            var temp = _context.ToDoEntries.Where(s => s.ToDoListId == toDoListId).ToList();
            return View(temp);
        }

        /// <summary>
        /// Get type of Create method.
        /// Displays view with form to create a ToDoEntry.
        /// </summary>
        /// <param name="id">Id of a ToDoList, where to create a ToDoEntry.</param>
        /// <returns>The created <see cref="ViewResult"/> with forms to fill in.</returns>
        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.Id = id;
            ToDoEntry toDoEntry = new ToDoEntry();
            return View(toDoEntry);
        }

        /// <summary>
        /// Post type of Create method.
        /// Gets a filled form of a ToDoEntry and add it to a DataBase.
        /// </summary>
        /// <param name="tdeObj">ToDoEntry to add to a DataBase.</param>
        /// <returns>If <paramref name="tdeObj"> is valid   ---> The created <see cref="RedirectToActionResult"/> to Index action of the same controller.</returns>
        /// <returns>If <paramref name="tdeObj"> is invalid ---> The created <see cref="ViewResult"/> of the same page.</returns>
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

        /// <summary>
        /// Get type of Edit method.
        /// Displays view with form to edit a ToDoEntry.
        /// </summary>
        /// <param name="id">Id of a ToDoEntry to edit in DataBase.</param>
        /// <returns>If <paramref name="id"> is valid   ---> The created <see cref="ViewResult"/> with form to fill in.</returns>
        /// <returns>If <paramref name="id"> is invalid ---> The created <see cref="NotFoundResult"/> page.</returns>
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

        /// <summary>
        /// Post type of Edit method.
        /// Gets a edited form of a ToDoEntry and update it in a DataBase.
        /// </summary>
        /// <param name="id">Id of a ToDoEntry to edit in DataBase.</param>
        /// <returns>If <paramref name="id"> is valid   ---> The created <see cref="RedirectToActionResult"/> to Index action of the same controller.</returns>
        /// <returns>If <paramref name="id"> is invalid ---> The created <see cref="ViewResult"/> of the same page.</returns>
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

        /// <summary>
        /// Delete an existing ToDoEntry.
        /// </summary>
        /// <param name="id">Id of a ToDoEntry to delete from DatatBase.</param>
        /// <returns>If <paramref name="id"> is valid   ---> The created <see cref="RedirectToActionResult"/> to Index action of the same controller.</returns>
        /// <returns>If <paramref name="id"> is invalid ---> The created <see cref="NotFoundResult"/> page.</returns>
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
