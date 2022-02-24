using MeterReadings.Data;
using MeterReadings.Data.Models;
using MeterReadings.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeterReadings.Tests
{
    public class TestMeterReadingRepository
    {
        [Test]
        public void TestAddMeterReadingsCallsContextAddMeterReadings()
        {
            var mockContextOptions = new DbContextOptions<AppDbContext>();
            var mockContext = new Mock<AppDbContext>(mockContextOptions);

            mockContext.Setup(c => c.MeterReadings.AddRange());

            var repo = new MeterReadingRepository(mockContext.Object);

            List<MeterReading> readings = new()
            {
                new(12345, DateTime.Parse("2022-02-22 09:00"), "10000"),
                new(12345, DateTime.Parse("2022-02-23 09:00"), "10010")
            };

            repo.AddMeterReadings(readings);

            mockContext.Verify(x => x.MeterReadings.AddRange(
                It.Is<IEnumerable<MeterReading>>(x => x.All(reading => HasMeterReading(readings, reading)))),
                Times.Once);
        }

        [Test]
        public void TestGetReadingsForAccountReturnsReadingsForAccount()
        {
            List<MeterReading> mockReadings = new()
            {
                new(12345, DateTime.Parse("2022-02-22 09:00"), "10000"),
                new(12345, DateTime.Parse("2022-02-23 09:00"), "10010"),
                new(98765, DateTime.Parse("2022-02-24 09:00"), "10010")
            };

            var readingsQueryable = mockReadings.AsQueryable();
            var mockSet = TestHelpers.CreateMockDbSet(readingsQueryable);

            var mockContextOptions = new DbContextOptions<AppDbContext>();
            var mockContext = new Mock<AppDbContext>(mockContextOptions);

            mockContext.Setup(c => c.MeterReadings).Returns(mockSet.Object);

            var repo = new MeterReadingRepository(mockContext.Object);

            var result = repo.GetReadingsForAccount(12345).ToList();
            var expectedResults = new List<MeterReading>()
            {
                new(12345, DateTime.Parse("2022-02-22 09:00"), "10000"),
                new(12345, DateTime.Parse("2022-02-23 09:00"), "10010"),
            };

            Assert.AreEqual(expectedResults, result);

        }

        private bool HasMeterReading(List<MeterReading> readings, MeterReading reading)
        {
            return readings.Any(x => x.AccountId == reading.AccountId
                && x.MeterReadingDateTime == reading.MeterReadingDateTime
                && x.MeterReadValue == reading.MeterReadValue);
        }
    }
}
