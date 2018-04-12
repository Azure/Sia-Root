﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Sia.Core.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Sia.Core.Tests.Data
{
    [TestClass]
    public class PartialJsonResolverTests
    {
        [TestMethod]
        public void ResolveJsonToString_Method_Resolve_WhenSourceValid_SerializesSourceArgumentObjectToString()
        {
            var input = new TestHasJsonDataObject
            {
                Data = new JsonSerializationTestObject()
            };
            var expectedResultDataValue = JsonSerializationTestObject.ExpectedSerialization();
            var objectUnderTest = new ResolveJsonToString<TestHasJsonDataObject, TestHasJsonDataString>();


            var result = objectUnderTest.Resolve(input, null, null, null);

            Assert.AreEqual(expectedResultDataValue, result, false, CultureInfo.InvariantCulture);

        }

        [TestMethod]
        public void ResolveStringToJson_Method_Resolve_WhenSourceValid_SerializesSourceArgumentStringToObject()
        {
            var input = new TestHasJsonDataString()
            {
                Data = JsonSerializationTestObject.ExpectedSerialization()
            };
            var expectedResult = new JsonSerializationTestObject();
            var objectUnderTest = new ResolveStringToJson<TestHasJsonDataString, TestHasJsonDataObject>();


            var result = objectUnderTest.Resolve(input, null, null, null);


            Assert.AreEqual(expectedResult.a, ExtractPropertyFromResult(result, "a"));
            Assert.AreEqual(expectedResult.b, ExtractPropertyFromResult(result, "b"));
        }

        [TestMethod]
        public void ResolveJsonToString_Method_Resolve_WhenSourceDataNull_ReturnsNull()
        {
            var input = new TestHasJsonDataObject
            {
                Data = null
            };
            var expectedResultDataValue = JsonSerializationTestObject.ExpectedSerialization();
            var objectUnderTest = new ResolveJsonToString<TestHasJsonDataObject, TestHasJsonDataString>();


            var result = objectUnderTest.Resolve(input, null, null, null);


            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ResolveStringToJson_Method_Resolve_WhenSourceDataNull_ReturnsNull()
        {
            var input = new TestHasJsonDataString()
            {
                Data = null
            };
            var expectedResult = new JsonSerializationTestObject();
            var objectUnderTest = new ResolveStringToJson<TestHasJsonDataString, TestHasJsonDataObject>();


            var result = objectUnderTest.Resolve(input, null, null, null);


            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ResolveJsonToString_Method_Resolve_WhenSourceNull_ReturnsNull()
        {
            TestHasJsonDataObject input = null;
            var expectedResultDataValue = JsonSerializationTestObject.ExpectedSerialization();
            var objectUnderTest = new ResolveJsonToString<TestHasJsonDataObject, TestHasJsonDataString>();


            var result = objectUnderTest.Resolve(input, null, null, null);


            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ResolveStringToJson_Method_Resolve_WhenSourceNull_ReturnsNull()
        {
            TestHasJsonDataString input = null;
            var expectedResult = new JsonSerializationTestObject();
            var objectUnderTest = new ResolveStringToJson<TestHasJsonDataString, TestHasJsonDataObject>();


            var result = objectUnderTest.Resolve(input, null, null, null);


            Assert.AreEqual(null, result);
        }

        private static JToken ExtractPropertyFromResult(object result, string propName) => ((JObject)result).Property(propName).Value;
    }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    internal class JsonSerializationTestObject : IEquatable<JsonSerializationTestObject>

    {
        public string a { get; set; } = "ValueOfA";
        public int b { get; set; } = 1;

        public static string ExpectedSerialization()
            => "{\"a\":\"ValueOfA\",\"b\":1}";

        public static bool operator ==(JsonSerializationTestObject left, JsonSerializationTestObject right)
        {
            if ((object)left == null || (object)right == null)
            {
                return Object.Equals(left, right);
            }

            return left.Equals(right);
        }

        public static bool operator !=(JsonSerializationTestObject left, JsonSerializationTestObject right)
        {
            if ((object)left == null || (object)right == null)
            {
                return !Object.Equals(left, right);
            }

            return !left.Equals(right);
        }

        public bool Equals(JsonSerializationTestObject other)
            => other != null && a == other.a && b == other.b;

        public override bool Equals(Object other)
        {
            if (other == null)
            {
                return false;
            }

            var castOther = other as JsonSerializationTestObject;

            return castOther != null &&
                a == castOther.a &&
                b == castOther.b;
        }
    }
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

    internal class TestHasJsonDataString : IJsonDataString
    {
        public string Data { get; set; }
    }

    internal class TestHasJsonDataObject : IJsonDataObject
    {
        public object Data { get; set; }
    }
}
