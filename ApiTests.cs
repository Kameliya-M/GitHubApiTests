using System.Text.Json;
using RestSharp;
using System.Net;
using RestSharp.Authenticators;
using RestSharp.Serializers;

namespace GitHubApiTests
{
    public class ApiTests
    {
        private RestClient client;
        private const string baseUrl = "https://api.github.com";
        private const string partialUrl = "repos/Kameliya-M/Postman/issues";
        private const string username = "Kameliya-M";
        private const string password = "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";

        [SetUp] 
        public void SetUp()
        { 
            this.client = new RestClient(baseUrl);
            this.client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        [Test]
        public void Test_GetSingleIssue()
        {
            //var client = new RestClient("https://api.github.com");
            var request = new RestRequest($"{partialUrl}/1", Method.Get);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(issue.title, Is.EqualTo("First issue"));
            Assert.That(issue.number, Is.EqualTo(1));
        }
        [Test]
        public void Test_GetSingleIssueWithLabels()
        {
            //var client = new RestClient("https://api.github.com");
            var request = new RestRequest($"{partialUrl}/1", Method.Get);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            for (int i = 0; i < issue.labels.Count; i++)
            {
                Assert.That(issue.labels[i].name, Is.Not.Empty);
            }




        }
        [Test]
        public void Test_GetAllIssues()
        {
            //var client = new RestClient("https://api.github.com");
            var request = new RestRequest(partialUrl, Method.Get);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            foreach(var issue in issues)
            {
                Assert.That(issue.title, Is.Not.Empty);
                Assert.That(issue.number, Is.GreaterThan(0));
            }
           
        }
        [Test]
        public void Test_CreateNewIssue()
        {
            //Arrange
            var request = new RestRequest(partialUrl, Method.Post);
            var issueBody = new
            {
                title = "Test issue from RestSharp " + DateTime.Now.Ticks,
                body = "some body for my issue",
                labels = new string[] { "bug", "critical", "release" }
            };
            
            request.AddBody(issueBody);

            //Act

            var response = this.client.Execute(request);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            //Assert

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo(issueBody.title));
            Assert.That(issue.body, Is.EqualTo(issueBody.body));
        }
        [Test]
        public void Test_EditIssue() 
        {
            var request = new RestRequest($"{partialUrl}/2", Method.Patch);
            request.AddJsonBody(new
            {
                title = "Edited title with RestSharp"
            }
                );
            var response = client.Execute(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.Greater(issue.id, 0);
            Assert.Greater(issue.number, 0);
            Assert.That(issue.title.Contains("Edited title with RestSharp"));

          }
        [Test]

        public void Test_DeleteIssue()
        {

        }

        [TestCase("US", "90210", "United States")]
        [TestCase("BG", "1000", "Bulgaria")]
        [TestCase("DE", "01067", "Germany")]
        public void Test_ZippopotamusDDT(string countryCode, string zipCode, string expectedCountry) 
        {
          var restClient = new RestClient("https://api.zippopotam.us");
          var request = new RestRequest(countryCode + "/" + zipCode, Method.Get);

          var response = restClient.Execute(request, Method.Get);
          Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var location = JsonSerializer.Deserialize<Locations>(response.Content);

            Assert.That(location.country, Is.EqualTo(expectedCountry));


        }

    }
}