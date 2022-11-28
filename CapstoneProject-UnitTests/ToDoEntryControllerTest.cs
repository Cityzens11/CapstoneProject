namespace CapstoneProject_UnitTests
{
    public class ToDoEntryControllerTest
    {
        [Fact]
        public void CreateMethod_Should_Return_ToDoEntry()
        {
            var temp = new ToDoEntryController(null);

            var result = temp.Create(0);

            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.IsType<ToDoEntry>(viewResult.Model);
        }

        [Fact]
        public async Task CreateMethod_Should_Return_InvalidModelState_For_Invalid_Data()
        {
            var temp = new ToDoEntryController(null);
            temp.ModelState.AddModelError("Name", "Error");

            var toDoEntry = new ToDoEntry() { CreatedDate = DateTime.Now };
            var result = await temp.Create(toDoEntry);

            Assert.IsType<ViewResult>(result);
            Assert.False(temp.ModelState.IsValid);
        }

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
