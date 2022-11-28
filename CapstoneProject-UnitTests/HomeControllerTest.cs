namespace CapstoneProject_UnitTests
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexMethod_Should_Return_The_Link_To_ToDoList_Index_Action()
        {
            var temp = new HomeController(null);

            var result = temp.Index();

            Assert.IsType<RedirectToActionResult>(result);

            var redirectResult = (RedirectToActionResult)result;
            Assert.NotNull(redirectResult);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("ToDoList", redirectResult.ControllerName);
        }
    }
}
