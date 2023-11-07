using Lab1Problem;

namespace Lab1_UnitTest
{
    public class Tests
    {
        BankAccount account{get;set;} 

        [SetUp]
        public void Setup()
        {
            account = new BankAccount();
        }

        [Test]
        public void Test_100_19()
        {
            account.Balance = 100;
            account.NumberOfCheck = 19;

            double expect_result = 26.89;
            var result = account.totalServiceFee();
            //Assert.AreEqual(expect_result, result, 0.01);
            Assert.That(result, Is.EqualTo(expect_result).Within(0.01));
            Assert.That(result, Is.TypeOf<double>());
        }

        [Test]
        public void Test_100_20()
        {
            account.Balance = 100;
            account.NumberOfCheck = 20;

            double expect_result = 26.60;
            var result = account.totalServiceFee();

            Assert.That(result, Is.EqualTo(expect_result).Within(0.01));
            Assert.That(result, Is.TypeOf<double>());

        }

        [Test]
        public void Test_100_40()
        {
            account.Balance = 100;
            account.NumberOfCheck = 40;

            double expect_result = 27.40;
            var result = account.totalServiceFee();

            Assert.That(result, Is.EqualTo(expect_result).Within(0.01));
            Assert.That(result, Is.TypeOf<double>());

        }

        [Test]
        public void Test2_100_60()
        {
            account.Balance = 100;
            account.NumberOfCheck = 60;

            double expect_result = 31.00;
            var result = account.totalServiceFee();

            Assert.That(result, Is.EqualTo(expect_result).Within(0.01));
            Assert.That(result, Is.TypeOf<double>());

        }

        [Test]
        public void Test_1000_19()
        {
            account.Balance = 1000;
            account.NumberOfCheck = 19;

            double expect_result = 11.89;
            var result = account.totalServiceFee();
            Assert.That(result, Is.EqualTo(expect_result).Within(0.01));
            Assert.That(result, Is.TypeOf<double>());

        }

        [Test]
        public void Test_1000_20()
        {
            account.Balance = 1000;
            account.NumberOfCheck = 20;

            double expect_result = 11.6;
            var result = account.totalServiceFee();

            Assert.That(result, Is.EqualTo(expect_result).Within(0.01));
            Assert.That(result, Is.TypeOf<double>());

        }

        [Test]
        public void Test_1000_40()
        {
            account.Balance = 1000;
            account.NumberOfCheck = 40;

            double expect_result = 12.4;
            var result = account.totalServiceFee();

            Assert.That(result, Is.EqualTo(expect_result).Within(0.01));
            Assert.That(result, Is.TypeOf<double>());

        }

        [Test]
        public void Test_1000_60()
        {
            account.Balance = 1000;
            account.NumberOfCheck = 60;

            double expect_result = 16.00;
            var result = account.totalServiceFee();

            Assert.That(result, Is.EqualTo(expect_result).Within(0.01));
            Assert.That(result, Is.TypeOf<double>());

        }
    }
}