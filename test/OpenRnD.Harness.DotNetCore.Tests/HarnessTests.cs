using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net.Http;

namespace OpenRnD.Harness.DotNetCore.Tests
{
    [TestClass]
    public class HarnessTests
    {
        [TestMethod]
        public async Task CanAcquire_DotNetCore()
        {
            int serverPort = 5555;

            using (DotNetCoreHarness harness = new DotNetCoreHarness("../../../OpenRnD.Harness.DotNetCore.Tests.Target", serverPort, "Development"))
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"http://localhost:{serverPort}/Target.txt");
                string text = await response.Content.ReadAsStringAsync();

                Assert.AreEqual(expected: "Target", actual: text);
            }
        }
    }
}
