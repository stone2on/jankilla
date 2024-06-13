using Jankilla.Core.Contracts.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Jankilla.Driver.MitsubishiMxComponent.Test
{
    [TestClass]
    public class BlockTest
    {
        [TestMethod]
        public void _01_StringTagTest()
        {
            var device = new MitsubishiMxComponentDevice() { ID = Guid.NewGuid() };
            
            var block = new MitsubishiMxComponentBlock("B1", 1, "D0", 100) { ID = Guid.NewGuid() };
            device.AddBlock(block);

            var s1 = new StringTag("S_DAT01", "D0000", Core.Contracts.Tags.Base.EDirection.In, 10) { ID = Guid.NewGuid() };
            var s2 = new StringTag("S_DAT02", "D90", Core.Contracts.Tags.Base.EDirection.In, 10) { ID = Guid.NewGuid() };

            block.AddTag(s1);
            block.AddTag(s2);

        }
    }
}
