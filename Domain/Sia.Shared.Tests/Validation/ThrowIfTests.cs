using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Tests.Validation
{
    [TestClass]
    public class ThrowIfTests
    {
        [TestMethod]
        public void Null_StaticMethod_WhenObjectIsNotNull_ReturnsObject()
        {
            var input = new Object();

            var result = ThrowIf.Null(input, nameof(input));

            Assert.AreSame(input, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_StaticMethod_WhenObjectIsNull_ThrowsArgumentNullException()
        {
            object input = null;

            var result = ThrowIf.Null(input, nameof(input));

            //expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullOrWhiteSpace_StaticMethod_WhenInputIsNull_ThrowsArgumentException()
        {
            string input = null;

            var result = ThrowIf.NullOrWhiteSpace(input, nameof(input));

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullOrWhiteSpace_StaticMethod_WhenInputIsOnlyWhitespace_ThrowsArgumentException()
        {
            string input = "    ";

            var result = ThrowIf.NullOrWhiteSpace(input, nameof(input));

            //Expect exception
        }

        [TestMethod]
        public void NullOrWhiteSpace_StaticMethod_WhenInputStringWithAnyNonWhitespace_ReturnsString()
        {
            string input = " .  ";

            var result = ThrowIf.NullOrWhiteSpace(input, nameof(input));

            Assert.AreSame(input, result);
        }
    }
}
