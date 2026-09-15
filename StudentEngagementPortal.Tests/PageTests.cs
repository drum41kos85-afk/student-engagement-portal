using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace StudentEngagementPortal.Tests
{
    public class PageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PageTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Events")]
        [InlineData("/Account/Login")]
        [InlineData("/Account/Register")]
        public async Task PublicPages_LoadSuccessfully(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AdminPanel_RedirectsToLogin_WhenNotAuthenticated()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var response = await client.GetAsync("/Admin");

            // Not logged in -> should redirect (302) to the login page, not return the page directly
            Assert.Equal(System.Net.HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Account/Login", response.Headers.Location?.ToString() ?? "");
        }

        [Fact]
        public async Task MyMessages_RedirectsToLogin_WhenNotAuthenticated()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var response = await client.GetAsync("/Messages");

            Assert.Equal(System.Net.HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Account/Login", response.Headers.Location?.ToString() ?? "");
        }
    }
}