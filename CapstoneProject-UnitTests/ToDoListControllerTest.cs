namespace CapstoneProject_UnitTests
{
    public class ToDoListControllerTest
    {
        [Fact]
        public void CreateMethod_Should_Return_ToDoList()
        {
            var temp = new ToDoListController(null);

            var result = temp.Create();

            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.IsType<ToDoList>(viewResult.Model);
        }

        [Fact]
        public async Task CreateMethod_Should_Return_InvalidModelState_For_Invalid_Data()
        {
            var temp = new ToDoListController(null);

            temp.ModelState.AddModelError("Name", "Error");
            var result = await temp.Create(It.IsAny<ToDoList>());

            Assert.IsType<ViewResult>(result);
            Assert.False(temp.ModelState.IsValid);
        }
    }
}
