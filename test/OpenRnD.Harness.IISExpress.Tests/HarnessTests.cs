using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenRnD.Harness.IISExpress.Tests
{
    [TestClass]
    public class HarnessTests
    {
        const int serverPort = 5555;

        private async Task AssertTargetReachable(int serverPort)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"http://localhost:{serverPort}/Target");
            string text = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(expected: "Target", actual: text);
        }

        [TestMethod]
        public async Task CanAcquire_AspNet()
        {
            using (IISExpressHarness harness = new IISExpressHarness("../../../OpenRnD.Harness.IISExpress.Tests.Target", serverPort))
            {
                await AssertTargetReachable(serverPort);
            }
        }

        [TestMethod]
        public async Task CanAcquire_AspNet_x86()
        {
            using (IISExpressHarness harness = new IISExpressHarness("../../../OpenRnD.Harness.IISExpress.Tests.Target", serverPort, IISExpressBitness.x86))
            {
                await AssertTargetReachable(serverPort);
            }
        }

        [TestMethod]
        public async Task CanAcquire_AspNet_x64()
        {
            using (IISExpressHarness harness = new IISExpressHarness("../../../OpenRnD.Harness.IISExpress.Tests.Target", serverPort, IISExpressBitness.x64))
            {
                await AssertTargetReachable(serverPort);
            }
        }

        [TestMethod]
        public async Task CanAcquire_AspNet_ShowWindow()
        {
            using (IISExpressHarness harness = new IISExpressHarness("../../../OpenRnD.Harness.IISExpress.Tests.Target", serverPort, showWindow: true))
            {
                await AssertTargetReachable(serverPort);
            }
        }
    }
}
