using AngleSharp.Html.Dom;
using Restaurants.IntegrationTests.Infrastructure;
using Restaurants.IntegrationTests.Mocks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Restaurants.IntegrationTests
{
    public class RestaurantsIntegrationTests : TestServerFixture, IClassFixture<MockRestaurantsApiClient>
    {
        [Fact]
        public async Task ShowsListOfResturantsWithImagesForGivenPostCode()
        {
            //Arrange
            var formValues = new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("PostCode","SE19")
            };
            var defaultPage = await Client.GetAsync("Home");

            // Act

            defaultPage.EnsureSuccessStatusCode();
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            var response = await Client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='postcode-search']"),
                formValues);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var resultsContent = await HtmlHelpers.GetDocumentAsync(response);
            var resultsList = resultsContent.QuerySelector("table[id='search-results']");
            Assert.NotNull(resultsList);
            var resultRows = resultsList.GetElementsByTagName("tr");
            // including the header row
            Assert.Equal(11, resultRows.Length);

            var resultsImages = resultsList.GetElementsByTagName("img");
            Assert.Equal("R1", ((IHtmlImageElement)resultsImages.First()).AlternativeText);
        }

        [Fact]
        public async Task ShowsListOfResturantsWithImagesForGivenGeoLocation()
        {
            // Act
            var response = await Client.GetAsync("?latitude=59.123&longitude=0.123");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await HtmlHelpers.GetDocumentAsync(response);
            var resultsList = content.QuerySelector("table[id='search-results']");
            Assert.NotNull(resultsList);
            var resultRows = resultsList.GetElementsByTagName("tr");
            // including the header row
            Assert.Equal(11, resultRows.Length);

            var resultsImages = resultsList.GetElementsByTagName("img");
            Assert.Equal("R1", ((IHtmlImageElement)resultsImages.First()).AlternativeText);
        }

    }
}
