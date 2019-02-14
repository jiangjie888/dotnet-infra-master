using System.Threading.Tasks;
using WebApp.Web.Host.Controllers;
using Shouldly;
using Xunit;

namespace WebApp.Web.Host.Tests.Controllers
{
    public class HomeController_Tests: WebAppWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
