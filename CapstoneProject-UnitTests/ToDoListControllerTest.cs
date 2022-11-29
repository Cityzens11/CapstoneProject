namespace CapstoneProject_UnitTests
{
    /// <summary>
    /// Test of ToDoList controller.
    /// </summary>
    public class ToDoListControllerTest
    {
        /// <summary>
        /// Test of Create action[HttpGet].
        /// Create method should return viewresult with ToDoList Model
        /// </summary>
        [Fact]
        public void CreateMethod_Should_Return_ToDoList()
        {
            var temp = new ToDoListController(null);

            var result = temp.Create();

            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.IsType<ToDoList>(viewResult.Model);
        }

        /// <summary>
        /// Test of Create action[HttpPost] for invalid filled forms.
        /// </summary>
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
