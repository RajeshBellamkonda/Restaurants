using AngleSharp.Dom;
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
            var resultList = resultsContent.QuerySelector("table[id='search-results'] > tbody");
            var resultRows = resultList.GetElementsByTagName("tr");
            Assert.NotNull(resultRows);
            Assert.Equal(10, resultRows.Length);
            var firstRowData = resultRows.First().GetElementsByTagName("td");
            // check the logo
            Assert.Contains("http://logo1", ((IHtmlImageElement)((IHtmlTableDataCellElement)firstRowData[0]).Children.First()).Source);
            // check the Restaurant Name
            Assert.Contains("R1", ((IHtmlTableDataCellElement)firstRowData[1]).TextContent);
            //check the RatingStars
            Assert.Contains("3", ((IHtmlTableDataCellElement)firstRowData[2]).TextContent);
            // check the cuisines
            Assert.Contains("C1", ((IHtmlTableDataCellElement)firstRowData[5]).Children[0].TextContent);
            Assert.Contains("C2", ((IHtmlTableDataCellElement)firstRowData[5]).Children[1].TextContent);

        }

        [Fact]
        public async Task ShowsListOfResturantsWithImagesForGivenGeoLocation()
        {
            // Act
            var response = await Client.GetAsync("?latitude=59.123&longitude=0.123");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var resultsContent = await HtmlHelpers.GetDocumentAsync(response);
            var resultList = resultsContent.QuerySelector("table[id='search-results'] > tbody");
            var resultRows = resultList.GetElementsByTagName("tr");
            Assert.NotNull(resultRows);
            Assert.Equal(10, resultRows.Length);
            var firstRowData = resultRows.First().GetElementsByTagName("td");
            // check the logo
            Assert.Contains("http://logo1", ((IHtmlImageElement)((IHtmlTableDataCellElement)firstRowData[0]).Children.First()).Source);
            // check the Restaurant Name
            Assert.Contains("R1", ((IHtmlTableDataCellElement)firstRowData[1]).TextContent);
            //check the RatingStars
            Assert.Contains("3", ((IHtmlTableDataCellElement)firstRowData[2]).TextContent);
            // check the cuisines
            Assert.Contains("C1", ((IHtmlTableDataCellElement)firstRowData[5]).Children[0].TextContent);
            Assert.Contains("C2", ((IHtmlTableDataCellElement)firstRowData[5]).Children[1].TextContent);
        }


    }
}
