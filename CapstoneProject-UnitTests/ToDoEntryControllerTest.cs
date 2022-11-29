namespace CapstoneProject_UnitTests
{
    /// <summary>
    /// Test of ToDoEntry controller.
    /// </summary>
    public class ToDoEntryControllerTest
    {
        /// <summary>
        /// Test of Create action[HttpGet].
        /// Create method should return viewresult with ToDoEntry Model
        /// </summary>
        [Fact]
        public void CreateMethod_Should_Return_ToDoEntry()
        {
            var temp = new ToDoEntryController(null);

            var result = temp.Create(0);

            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.IsType<ToDoEntry>(viewResult.Model);
        }

        /// <summary>
        /// Test of Create action[HttpPost] for invalid filled forms.
        /// </summary>
        [Fact]
        public async Task CreateMethod_Should_Return_ViewResult_For_Invalid_Data()
        {
            var temp = new ToDoEntryController(null);
            temp.ModelState.AddModelError("Name", "Error");

            var toDoEntry = new ToDoEntry() { CreatedDate = DateTime.Now };
            var result = await temp.Create(toDoEntry);

            Assert.IsType<ViewResult>(result);
            Assert.False(temp.ModelState.IsValid);
        }

        /// <summary>
        /// Test of Create action[HttpPost] for invalid filled DueDate.
        /// If DueDate is already passed than it is invalid.
        /// </summary>
        [Fact]
        public async Task CreateMethod_Should_Return_ViewResult_For_Invalid_DueDate()
        {
            var temp = new ToDoEntryController(null);

            DateTime now = DateTime.Now;
            DateTime past = DateTime.Now.AddDays(-1);
            ToDoEntry toDoEntry = new ToDoEntry() { DueDate = past };

            var result = await temp.Create(toDoEntry);
            Assert.IsType<ViewResult>(result);
        }
    }
}
