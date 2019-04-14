using NUnit.Framework;
using System;

namespace DynamicMethodGeneration.Tests
{
    [TestFixture]
    public class GuardTests
    {
        [Test]
        public void CanBindInstance_ShouldThrow_WhenIsStaticTrue()
        {
            var isStatic = true;

            Assert.That(
                () => Guard.CanBindInstance(typeof(int), typeof(int), isStatic),
                Throws.InvalidOperationException
            );
        }

        [Test]
        public void CanBindInstance_ShouldNotThrow_WhenIsStaticFalse()
        {
            var isStatic = false;

            Assert.That(
                () => Guard.CanBindInstance(typeof(int), typeof(int), isStatic),
                Throws.Nothing
            );
        }

        [TestCase(typeof(int), typeof(long))]
        [TestCase(typeof(A), typeof(B))]
        [TestCase(typeof(AA), typeof(A))]
        public void CanBindInstance_ShouldThrow_WhenUnassignableType(Type declaring, Type instance)
        {
            Assert.That(
                () => Guard.CanBindInstance(declaring, instance, false),
                Throws.InvalidOperationException
            );
        }

        [TestCase(typeof(int), typeof(int))]
        [TestCase(typeof(A), typeof(A))]
        [TestCase(typeof(A), typeof(AA))]
        public void CanBindInstance_ShouldNotThrow_WhenAssignableType(Type declaring, Type instance)
        {
            Assert.That(
                () => Guard.CanBindInstance(declaring, instance, false),
                Throws.Nothing
            );
        }

        private class A { }
        private class B { }
        private class AA : A { }
    }
}
