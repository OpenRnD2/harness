using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenRnD.Harness.IISExpress.Tests
{
    [TestClass]
    public class CSProjUtilitiesTests
    {
        [TestMethod]
        public void CSProjUtilities_CanRetrieveIISUrl()
        {
            string iisUrl = CSProjUtilities.GetIISUrl("../../../OpenRnD.Harness.IISExpress.Tests.Target/OpenRnD.Harness.IISExpress.Tests.Target.csproj");

            Assert.AreEqual("http://localhost:21956/", iisUrl);
        }
    }
}
