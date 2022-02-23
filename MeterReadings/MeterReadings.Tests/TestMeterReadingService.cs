using AutoMapper;
using MeterReadings.Data.Repositories;
using MeterReadings.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace MeterReadings.Tests
{
    public class TestMeterReadingService
    {
        private Mock<IMeterReadingRepository> mockMeterReadingRepo = new Mock<IMeterReadingRepository>();
        private Mock<IMapper> mockMapper = new Mock<IMapper>();
        private Mock<ILogger<MeterReadingService>> mockLogger = new Mock<ILogger<MeterReadingService>>();

        [Test]
        public void TestCreateMeterReadingFromCsvCreatesMeterReading()
        {
            var meterReadingService = new MeterReadingService(mockMeterReadingRepo.Object, mockLogger.Object);

            var rowData = "12345,08/02/2022 09:30,00123";

            var result = meterReadingService.CreateReadingFromCsvData(rowData);
            var expectedResult = new MeterReadingInputModel(12345, new DateTime(2022, 02, 08, 9, 30, 0), "00123");

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TestValidateMeterReadingIsValidWhenMeterReadValueIsValid()
        {
            var meterReadingService = new MeterReadingService(mockMeterReadingRepo.Object, mockLogger.Object);
            var rowData = "12345,08/02/2022 09:30,00123";
            var reading = meterReadingService.CreateReadingFromCsvData(rowData);

            var isValid = meterReadingService.ReadingIsValid(reading);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void TestValidateMeterReadingRaisesInvalidErrorWhenReadValueIsEmpty()
        {
            var meterReadingService = new MeterReadingService(mockMeterReadingRepo.Object, mockLogger.Object);
            var rowData = "12345,08/02/2022 09:30,";
            var reading = meterReadingService.CreateReadingFromCsvData(rowData);

            var isValid = meterReadingService.ReadingIsValid(reading);

            Assert.IsFalse(isValid);
            Assert.AreEqual(1, reading.ValidationErrors.Count);
            Assert.AreEqual("MeterReadValue must be in the format 'NNNNN'", reading.ValidationErrors[0]);
        }
    }
}
