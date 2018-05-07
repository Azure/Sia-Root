using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Core.Tests.Validation
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
        public void Null_StaticMethod_WhenObjectIsNull_AndCalledFromConstructor_ThrowsArgumentNullException()
        {
#pragma warning disable CA1806 // Do not ignore method results
            new ThrowIfTester(ThrowIfTester.Null());
#pragma warning restore CA1806 // Do not ignore method results

            //expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Null_StaticMethod_WhenObjectIsNull_AndCalledFromNonConstructor_ThrowsNullReferenceException()
        {
            ThrowIfTester.TestMe(ThrowIfTester.Null());

            //expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullOrWhiteSpace_StaticMethod_WhenInputIsNull_AndCalledFromConstructor_ThrowsArgumentException()
        {
#pragma warning disable IDE0022 // Use expression body for methods
            new ThrowIfTester((string)null);
#pragma warning restore IDE0022 // Use expression body for methods

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void NullOrWhiteSpace_StaticMethod_WhenInputIsNull_AndCalledFromNonConstructor_ThrowsArgumentException()
        {
            ThrowIfTester.TestMe((string)null);

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void NullOrWhiteSpace_StaticMethod_WhenInputIsOnlyWhitespace_ThrowsANullReferenceException()
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

    internal class ThrowIfTester
    {
        internal static ThrowIfTester Null()
            => null;
        internal static ThrowIfTester ValidObject()
            => new ThrowIfTester();
        private ThrowIfTester()
        {

        }
        internal ThrowIfTester(string arg)
        {
            ThrowIf.NullOrWhiteSpace(arg, nameof(arg));
        }

        internal ThrowIfTester(ThrowIfTester otherArg)
        {
            ThrowIf.Null(otherArg, nameof(otherArg));
        }

        internal static void TestMe(string arg)
        {
            ThrowIf.NullOrWhiteSpace(arg, nameof(arg));
        }

        internal static void TestMe(ThrowIfTester otherArg)
        {
            ThrowIf.Null(otherArg, nameof(otherArg));
        }
    }
}
