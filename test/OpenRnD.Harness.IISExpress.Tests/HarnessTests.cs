using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenRnD.Harness.IISExpress.Tests
{
    [TestClass]
    public class HarnessTests
    {
        [TestMethod]
        public async Task CanAcquire_AspNet()
        {
            int serverPort = 5555;

            using (IISExpressHarness harness = new IISExpressHarness("../../../OpenRnD.Harness.IISExpress.Tests.Target", serverPort))
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"http://localhost:{serverPort}/Target");
                string text = await response.Content.ReadAsStringAsync();

                Assert.AreEqual(expected: "Target", actual: text);
            }
        }

        [TestMethod]
        public async Task CanAcquire_AspNet_x86()
        {
            int serverPort = 5555;

            using (IISExpressHarness harness = new IISExpressHarness("../../../OpenRnD.Harness.IISExpress.Tests.Target", serverPort, IISExpressBitness.x64))
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"http://localhost:{serverPort}/Target");
                string text = await response.Content.ReadAsStringAsync();

                Assert.AreEqual(expected: "Target", actual: text);
            }
        }

        [TestMethod]
        public async Task CanAcquire_AspNet_x64()
        {
            int serverPort = 5555;

            using (IISExpressHarness harness = new IISExpressHarness("../../../OpenRnD.Harness.IISExpress.Tests.Target", serverPort, IISExpressBitness.x64))
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"http://localhost:{serverPort}/Target");
                string text = await response.Content.ReadAsStringAsync();

                Assert.AreEqual(expected: "Target", actual: text);
            }
        }
    }
}
