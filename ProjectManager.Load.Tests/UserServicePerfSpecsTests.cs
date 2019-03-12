using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NBench;
using ProjectManager.Model;
//using ProjectManager.Service;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Pro.NBench.xUnit.XunitExtensions;
using Xunit.Abstractions;
using System.Diagnostics;

namespace ProjectManager.Load.Tests
{
    public class UserServicePerfSpecs
    {
        private Counter _counter;

        private readonly TestServer _server;
        private readonly HttpClient _client;
        private const int AcceptableMinAddThroughput = 100;

        private const string AddCounterName = "AddCounter";      
        private Counter _addCounter;       

        public UserServicePerfSpecs(ITestOutputHelper output)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new XunitTraceListener(output));

            _server = new TestServer(WebHost.CreateDefaultBuilder()
            .UseStartup<TestStartup>()
            .UseEnvironment("Development"));
            _client = _server.CreateClient();
        }

        [PerfSetup]
#pragma warning disable xUnit1013 // Public method should be marked as test
        public void Setup(BenchmarkContext context)
#pragma warning restore xUnit1013 // Public method should be marked as test
        {
            _addCounter = context.GetCounter(AddCounterName);
            
            var user = new User() { EmployeeId = 2, FirstName = "First", LastName = "last", UserId = 2 };
            var jsonInString = JsonConvert.SerializeObject(user);

            var response = _client.PostAsync("/api/users", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void UserServicePost()
        {
            var user = new User() { EmployeeId = 1, FirstName = "First", LastName = "last", UserId = 1 };
            var jsonInString = JsonConvert.SerializeObject(user);

            var response = _client.PostAsync("/api/users", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void UserServicePut()
        {
            var user = new User() { EmployeeId = 1, FirstName = "First", LastName = "last", UserId = 1 };
            var jsonInString = JsonConvert.SerializeObject(user);

            var response = _client.PutAsync("/api/users/1", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void UserServiceDelete()
        {         
            var response = _client.DeleteAsync("/api/users/2").Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void UserServiceGetAll()
        {
            var response = _client.GetAsync("/api/users").Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void UserServiceGet()
        {
            var response = _client.GetAsync("/api/users/2").Result;
            _addCounter.Increment();
        }

        [PerfCleanup]
#pragma warning disable xUnit1013 // Public method should be marked as test
        public void Cleanup(BenchmarkContext context)
#pragma warning restore xUnit1013 // Public method should be marked as test
        {
          
        }

    }
}

