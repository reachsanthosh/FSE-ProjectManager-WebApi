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
    public class TaskServicePerfSpecsTests
    {
        private Counter _counter;

        private readonly TestServer _server;
        private readonly HttpClient _client;
        private const int AcceptableMinAddThroughput = 100;

        private const string AddCounterName = "AddCounter";
        private Counter _addCounter;

        public TaskServicePerfSpecsTests(ITestOutputHelper output)
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

            var taskDetail = new TaskDetail()
            {
                Id = 2,
                ActiveStatus = true,
                Priority = 10,
                Name = "task 2",
                EndDate = DateTime.Now,
                StartDate = DateTime.Now
            };

            var jsonInString = JsonConvert.SerializeObject(taskDetail);

            var response = _client.PostAsync("/api/tasks", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void TaskServicePost()
        {
            var taskDetail = new TaskDetail()
            {
                Id = 1,
                ActiveStatus = true,
                Priority = 10,
                Name = "task 1",
                EndDate = DateTime.Now,
                StartDate = DateTime.Now
            };

            var jsonInString = JsonConvert.SerializeObject(taskDetail);

            var response = _client.PostAsync("/api/tasks", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void TaskServicePut()
        {
            var taskDetail = new TaskDetail()
            {
                Id = 1,
                ActiveStatus = true,
                Priority = 10,
                Name = "task 1",
                EndDate = DateTime.Now,
                StartDate = DateTime.Now
            };

            var jsonInString = JsonConvert.SerializeObject(taskDetail);

            var response = _client.PutAsync("/api/tasks/1", new StringContent(jsonInString, Encoding.UTF8, "application/json")).Result;
            _addCounter.Increment();
        }


        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void TaskServiceDelete()
        {
            var response = _client.DeleteAsync("/api/tasks/2").Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void TaskServiceGetAll()
        {
            var response = _client.GetAsync("/api/tasks").Result;
            _addCounter.Increment();
        }

        [NBenchFact]
        [PerfBenchmark(RunMode = RunMode.Throughput, NumberOfIterations = 3, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion(AddCounterName, MustBe.GreaterThan, AcceptableMinAddThroughput)]
        public void TaskServiceGet()
        {
            var response = _client.GetAsync("/api/tasks/2").Result;
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
