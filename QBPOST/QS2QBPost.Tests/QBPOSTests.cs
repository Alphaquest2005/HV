namespace QS2QBPost.Tests
{
    
    using System;
    using NUnit.Framework;
    using RMSDataAccessLayer;

    [TestFixture]
    public class QBPOSTests
    {
        private QS2QBPost.QBPOS _testClass;

        [SetUp]
        public void SetUp()
        {
            _testClass = new QBPOS();
        }

        [Test]
        public void CanCallGetOrAddCustomerQuery()
        {
            var patient = new Patient();
            var QBCompanyFile = "TestValue1666452039";
            var result = QBPOS.GetOrAddCustomerQuery(patient, QBCompanyFile);
            Assert.Fail("Create or modify test");
        }

        //[Test]
        //public void CannotCallGetOrAddCustomerQueryWithNullPatient()
        //{
        //    Assert.Throws<ArgumentNullException>(() => QBPOS.GetOrAddCustomerQuery(default(Patient), "TestValue332058110"));
        //}

        //[TestCase(null)]
        //[TestCase("")]
        //[TestCase("   ")]
        //public void CannotCallGetOrAddCustomerQueryWithInvalidQBCompanyFile(string value)
        //{
        //    Assert.Throws<ArgumentNullException>(() => QBPOS.GetOrAddCustomerQuery(new Patient(), value));
        //}
    }
}