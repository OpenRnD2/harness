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
        public async Task CanAcquireTarget()
        {
            int serverPort = 5555;

            using (IISExpressHarness harness = new IISExpressHarness("../../../OpenRnD.Harness.IISExpress.Tests.Target", serverPort))
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"http://localhost:{serverPort}/Target.txt");
                string text = await response.Content.ReadAsStringAsync();

                Assert.AreEqual(expected: "Target", actual: text);
            }
        }
    }
}
