using GitHubApiTests;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;

namespace RestSharpDemoProject
{
    internal class Program
    {
        public void Main(string[] args)
        {
            RestClient client = new RestClient("https://api.github.com");

            client.Authenticator = new HttpBasicAuthenticator("Kameliya-M", "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");

            RestRequest request = new RestRequest("/repos/{user}/{repoName}/issues", Method.Post);
            var issueBody = new
            {
                title = "Test issue from RestSharp " + DateTime.Now.Ticks,
                body = "some body for my issue",
                labels = new string[] {"bug", "critical", "release"}
            };

            request.AddBody(issueBody);
            request.AddUrlSegment("user", "Kameliya-M");
            request.AddUrlSegment("repoName", "Postman");
            //request.AddUrlSegment("id", "1");

            var response = client.Execute(request);

            
            Console.WriteLine("Status Code: " + response.StatusCode);

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Console.WriteLine("Issue name: " + issue.title);
            Console.WriteLine("Issue number: " + issue.number);
            //var labels = JsonSerializer.Deserialize<List<Labels>>(response.Content!);

            //Console.WriteLine("labels: " + labels);

            //foreach (var label in labels)
            //{
            //    Console.WriteLine("Label name: " + label.name);
            //    Console.WriteLine("Label id: " + label.id);
            //}

            //foreach (var issue in issues)
            //{
            //    Console.WriteLine("Issue name: " + issue.title);
            //    Console.WriteLine("Issue number: " + issue.number);
            //}


            //Console.WriteLine("Response: " + response.Content);

        }
    }

   
}