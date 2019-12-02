using InterviewProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void APIClientIntegrationTest1()
        {
            var task = Execute();
            task.Wait();
            var result = task.Result;

            Assert.IsTrue(result.Count() > 0);
        }

        private async Task<IEnumerable<string>> Execute()
        {
            var client = new APIClient();
            var task = await client.GetLocationList("san");
            return task;
        }

        [TestMethod]
        public void APIClientIntegrationTest2()
        {
            var task = Execute2();
            task.Wait();
            var result = task.Result;
            Assert.IsTrue(result.HasValue);
        }

        private async Task<int?> Execute2()
        {
            var client = new APIClient();
            var task = await client.GetWhereOnEarthId("San Francisco");
            return task;
        }

    }
}
