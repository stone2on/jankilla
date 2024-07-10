using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Jankilla.Driver.MitsubishiMxComponent.Test
{
    [TestClass]
    public class IntTagTest
    {
        private IntTag _tag;

        [TestInitialize]
        public void Setup()
        {
            _tag = new IntTag() { Name = "TestIntTag", Address = "D01" };
        }

        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            Assert.AreEqual("TestIntTag", _tag.Name);
            Assert.AreEqual("D01", _tag.Address);
            Assert.AreEqual(4, _tag.ByteSize);
            Assert.AreEqual(EDirection.In, _tag.Direction);
        }

        [TestMethod]
        public void Read_ShouldSetValue()
        {
            short[] buffer = new short[] { 0, 0, 1, 0 }; 
            _tag.Read(buffer, 2);
            Assert.AreEqual(1, _tag.IntValue);
        }

        [TestMethod]
        public void CompareByteArrays_ShouldReturnTrueForEqualArrays()
        {
            byte[] array1 = { 1, 2, 3, 4 };
            byte[] array2 = { 1, 2, 3, 4 };

            bool result = Tag.CompareByteArrays(array1, array2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CompareByteArrays_ShouldReturnFalseForDifferentArrays()
        {
            byte[] array1 = { 1, 2, 3, 4 };
            byte[] array2 = { 4, 3, 2, 1 };

            bool result = Tag.CompareByteArrays(array1, array2);

            Assert.IsFalse(result);
        }
    }
}
