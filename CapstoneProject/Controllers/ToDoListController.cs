using CapstoneProject.DomainModels;
using CapstoneProject.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers
{
    /// <summary>
    /// A base class(controller) for Capstone project that works with ToDoLists.
    /// </summary>
    public class ToDoListController : Controller
    {
        private readonly CapStoneDbContext _context;

        /// <summary>
        /// Gets a reference to existing DataBase in a controller.
        /// </summary>
        public ToDoListController(CapStoneDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets Data from DbContext and Displays all ToDoLists in a DataBase.
        /// </summary>
        /// <returns>The created <see cref="ViewResult"/> with all ToDoLists.</returns>
        public IActionResult Index()
        {
            IEnumerable<ToDoList> objCatlist = _context.ToDoLists;
            return View(objCatlist);
        }

        /// <summary>
        /// Get type of Create method.
        /// Displays view with form to create a ToDoList.
        /// </summary>
        /// <returns>The created <see cref="ViewResult"/> with forms to fill in.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            ToDoList toDoList = new ToDoList();
            return View(toDoList);
        }

        /// <summary>
        /// Post type of Create method.
        /// Gets a filled form of a ToDoList and add it to DataBase.
        /// </summary>
        /// <param name="tdlObj">ToDoList to add to DataBase.</param>
        /// <returns>If <paramref name="tdlObj"> is valid   ---> The created <see cref="RedirectToActionResult"/> to Index action of the same controller.</returns>
        /// <returns>If <paramref name="tdlObj"> is invalid ---> The created <see cref="ViewResult"/> of the same page.</returns>
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

        /// <summary>
        /// Edit an existing ToDoList in DataBase.
        /// </summary>
        /// <param name="id">Id of a ToDoList to edit in DataBase.</param>
        /// <returns>If <paramref name="id"> is valid   ---> The created <see cref="RedirectToActionResult"/> to Index action of the same controller.</returns>
        /// <returns>If <paramref name="id"> is invalid ---> The created <see cref="NotFoundResult"/> page.</returns>
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

        /// <summary>
        /// Delete an existing ToDoList.
        /// </summary>
        /// <param name="id">Id of a ToDoList to delete from DatatBase.</param>
        /// <returns>If <paramref name="id"> is valid   ---> The created <see cref="RedirectToActionResult"/> to Index action of the same controller.</returns>
        /// <returns>If <paramref name="id"> is invalid ---> The created <see cref="NotFoundResult"/> page.</returns>
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
