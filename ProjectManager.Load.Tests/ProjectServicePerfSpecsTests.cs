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
using System;

namespace ProjectManager.Load.Tests
{
    public class ProjectServicePerfSpecsTests
    {
        private Counter _counter;

        private readonly TestServer _server;
        private readonly HttpClient _client;
        private const int AcceptableMinAddThroughput = 100;

        private const string AddCounterName = "AddCounter";
        private Counter _addCounter;

        public ProjectServicePerfSpecsTests(ITestOutputHelper output)
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

            var project = new Project() { ProjectId = 2, ActiveStatus = true, Priority = 10, ProjectName ="project2", EndDate = DateTime.Now, 
             StartDate = DateTime.Now };

            var jsonInString = JsonConvert.SerializeObject(project);

            var response = _client.PostAsync("/api/projects", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void ProjectServicePost()
        {
            var project = new Project()
            {
                ProjectId = 1,
                ActiveStatus = true,
                Priority = 10,
                ProjectName = "project1",
                EndDate = DateTime.Now,
                StartDate = DateTime.Now
            };

            var jsonInString = JsonConvert.SerializeObject(project);

            var response = _client.PostAsync("/api/projects", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void ProjectServicePut()
        {
            var project = new Project()
            {
                ProjectId = 1,
                ActiveStatus = true,
                Priority = 10,
                ProjectName = "project1",
                EndDate = DateTime.Now,
                StartDate = DateTime.Now
            };

            var jsonInString = JsonConvert.SerializeObject(project);

            var response = _client.PutAsync("/api/projects/1", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void ProjectServiceDelete()
        {
            var response = _client.DeleteAsync("/api/projects/2").Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void ProjectServiceGetAll()
        {
            var response = _client.GetAsync("/api/projects").Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void ProjectServiceGet()
        {
            var response = _client.GetAsync("/api/projects/2").Result;
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
