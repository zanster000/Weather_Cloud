using System;
using System.Net.Http;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Weather_Cloud.Controllers;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Weather_Cloud_Test
{
    public class WeatherForecastControllerTest
    {

        public static string response = @"{""by"":""default"",""valid_key"":false,""results"":{""temp"":20,""date"":""11/04/2022"",""time"":""09:41"",""condition_code"":""28"",""description"":""Temponublado"",""currently"":""dia"",""cid"":"""",""city"":""SãoPaulo,SP"",""img_id"":""28"",""humidity"":81,""wind_speedy"":""5.66km/h"",""sunrise"":""06:18am"",""sunset"":""05:56pm"",""condition_slug"":""cloudly_day"",""city_name"":""SãoPaulo"",""forecast"":[{""date"":""11/04"",""weekday"":""Seg"",""max"":28,""min"":17,""description"":""Temponublado"",""condition"":""cloudly_day""},{""date"":""12/04"",""weekday"":""Ter"",""max"":29,""min"":19,""description"":""Chuvasesparsas"",""condition"":""rain""},{""date"":""13/04"",""weekday"":""Qua"",""max"":26,""min"":19,""description"":""Chuva"",""condition"":""rain""},{""date"":""14/04"",""weekday"":""Qui"",""max"":24,""min"":18,""description"":""Chuvasesparsas"",""condition"":""rain""},{""date"":""15/04"",""weekday"":""Sex"",""max"":17,""min"":14,""description"":""Chuvasesparsas"",""condition"":""rain""},{""date"":""16/04"",""weekday"":""Sáb"",""max"":18,""min"":13,""description"":""Chuvasesparsas"",""condition"":""rain""},{""date"":""17/04"",""weekday"":""Dom"",""max"":20,""min"":13,""description"":""Chuvasesparsas"",""condition"":""rain""},{""date"":""18/04"",""weekday"":""Seg"",""max"":23,""min"":13,""description"":""Tempolimpo"",""condition"":""clear_day""},{""date"":""19/04"",""weekday"":""Ter"",""max"":23,""min"":14,""description"":""Tempolimpo"",""condition"":""clear_day""},{""date"":""20/04"",""weekday"":""Qua"",""max"":25,""min"":15,""description"":""Tempolimpo"",""condition"":""clear_day""}]},""execution_time"":0.0,""from_cache"":true}";


        private readonly Mock<HttpMessageHandler> _mockMessageHandler;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<WeatherForecastController>> _mockLogger;

        public WeatherForecastControllerTest()
        {
            this._mockHttpClientFactory = new Mock<IHttpClientFactory>();
            this._mockMessageHandler = new Mock<HttpMessageHandler>();
            this._mockLogger = new Mock<ILogger<WeatherForecastController>>();
        }

        [Fact]
        public async Task Test1()
        {

            var responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response)
            };

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(_mockMessageHandler.Object)
            {
                BaseAddress = new Uri(configuration.GetValue<String>("HG_API:url")),
            };

            _mockHttpClientFactory.Setup(h => h.CreateClient(It.IsAny<String>())).Returns(httpClient);

            var controller = new WeatherForecastController(_mockLogger.Object, _mockHttpClientFactory.Object);
            var result = await controller.Get();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
