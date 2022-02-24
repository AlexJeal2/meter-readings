using AutoMapper;
using MeterReadings.Data.Models;
using MeterReadings.Data.Repositories;
using MeterReadings.Models;
using MeterReadings.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace MeterReadings.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestMeterReadingService
    {
        private Mock<IMeterReadingRepository> mockMeterReadingRepo = new();
        private Mock<IAccountRepository> mockAccountRepo = new();
        private Mock<IMapper> mockMapper = new();
        private Mock<ILogger<MeterReadingService>> mockLogger = new();

        private MeterReadingService CreateMeterReadingServiceFromMocks()
        {
            return new MeterReadingService(mockMeterReadingRepo.Object, mockAccountRepo.Object, mockMapper.Object, mockLogger.Object);
        }

        [SetUp]
        public void Setup()
        {
            //Reset the mocks for each test
            mockMeterReadingRepo = new();
            mockAccountRepo = new();
            mockMapper = new();
            mockLogger = new();
        }

        [Test]
        public void TestCreateMeterReadingFromCsvCreatesMeterReading()
        {
            var meterReadingService = CreateMeterReadingServiceFromMocks();

            var rowData = "12345,08/02/2022 09:30,00123";

            var result = meterReadingService.CreateReadingFromCsvData(rowData);
            var expectedResult = new MeterReadingDTO(12345, new DateTime(2022, 02, 08, 9, 30, 0), "00123");

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TestValidateMeterReadingIsValidWhenMeterReadValueIsValid()
        {
            var meterReadingService = CreateMeterReadingServiceFromMocks();
            var rowData = "12345,08/02/2022 09:30,00123";
            var reading = meterReadingService.CreateReadingFromCsvData(rowData);

            var response = new AddMeterReadingsResultDTO();
            meterReadingService.ValidateReading(reading, response);

            Assert.IsTrue(reading.IsValid);
        }

        [Test]
        public void TestValidateMeterReadingRaisesInvalidErrorWhenReadValueIsEmpty()
        {
            var meterReadingService = CreateMeterReadingServiceFromMocks();
            var rowData = "12345,08/02/2022 09:30,";
            var reading = meterReadingService.CreateReadingFromCsvData(rowData);

            var response = new AddMeterReadingsResultDTO();
            meterReadingService.ValidateReading(reading, response);

            Assert.IsFalse(reading.IsValid);
            Assert.AreEqual(1, reading.ValidationErrors.Count);
            Assert.AreEqual("MeterReadValue must be in the format 'NNNNN'", reading.ValidationErrors[0]);
        }

        [Test]
        public void GetGetMeterReadingFromCsvReturnsExpectedResults()
        {
            var meterReadingService = CreateMeterReadingServiceFromMocks();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("AccountId,MeterReadingDateTime,MeterReadValue");
            //Valid
            csvBuilder.AppendLine("12345,08/02/2022 09:30,00123");
            //Duplicate
            csvBuilder.AppendLine("12345,09/02/2022 09:30,00200");
            //Invalid
            csvBuilder.AppendLine("12345,10/02/2022 09:30,");
            //Duplicate
            csvBuilder.AppendLine("12345,09/02/2022 09:30,00200");

            var rowData = csvBuilder.ToString();
            var readings = meterReadingService.GetValidFormatMeterReadingsFromCsv(rowData);

            Assert.AreEqual(1, readings.ValidReadings.Count);
            Assert.AreEqual(2, readings.InvalidReadings.Count);
        }

        [Test]
        public void TestGroupMeterReadingsByCustomer()
        {
            var meterReadingService = CreateMeterReadingServiceFromMocks();

            var readings = new AddMeterReadingsResultDTO();
            var validReadings = new List<MeterReadingDTO>()
            {
                new MeterReadingDTO(12345, DateTime.Parse("2022-02-23 10:00"), "00010"),
                new MeterReadingDTO(12345, DateTime.Parse("2022-02-24 09:00"), "000015"),
                new MeterReadingDTO(12345, DateTime.Parse("2022-02-23 09:00"), "00005"),
                new MeterReadingDTO(98765, DateTime.Parse("2022-02-22 09:00"), "00140"),
                new MeterReadingDTO(98765, DateTime.Parse("2022-02-24 09:00"), "00150"),
            };

            readings.ValidReadings = new HashSet<MeterReadingDTO>(validReadings);

            var result = meterReadingService.GroupReadingsByCustomer(readings);

            var expectedResult = new Dictionary<int, List<MeterReadingDTO>>()
            {
                { 12345, new List<MeterReadingDTO>()
                        {
                            new MeterReadingDTO(12345, DateTime.Parse("2022-02-23 09:00"), "00005"),
                            new MeterReadingDTO(12345, DateTime.Parse("2022-02-23 10:00"), "00010"),
                            new MeterReadingDTO(12345, DateTime.Parse("2022-02-24 09:00"), "000015"),
                        }
                },
                { 98765, new List<MeterReadingDTO>()
                        {
                           new MeterReadingDTO(98765, DateTime.Parse("2022-02-22 09:00"), "00140"),
                           new MeterReadingDTO(98765, DateTime.Parse("2022-02-24 09:00"), "00150"),
                        }
                }
            };

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TestValidateAccountExistsWithInvalidAccountIdAppliesValidationCorrectly()
        {
            var meterReadingService = CreateMeterReadingServiceFromMocks();
            var readings = new AddMeterReadingsResultDTO();
            var validReadings = new List<MeterReadingDTO>()
            {
                new MeterReadingDTO(98765, DateTime.Parse("2022-02-22 09:00"), "00140"),
                new MeterReadingDTO(98765, DateTime.Parse("2022-02-24 09:00"), "00150"),
            };

            readings.ValidReadings = new HashSet<MeterReadingDTO>(validReadings);

            var mockMissingAccountIds = new HashSet<int>() { 98765 };

            meterReadingService.ValidateAccountExists(mockMissingAccountIds, 98765, validReadings, readings);

            Assert.AreEqual(0, readings.ValidReadings.Count);
            Assert.AreEqual(2, readings.InvalidReadings.Count);
            Assert.AreEqual("Account not found for AccountId 98765", readings.InvalidReadings[0].ValidationErrors[0]);
        }

        [Test]
        public void TestValidateNoLaterDateExistsWhenLaterDateExists()
        {
            List<MeterReading> mockMeterReadings = new()
            {
                new MeterReading(12345, DateTime.Parse("2022-02-22 09:00"), "00100"),
                new MeterReading(12345, DateTime.Parse("2022-02-24 09:00"), "00100")
            };

            mockMeterReadingRepo.Setup(x => x.GetReadingsForAccount(It.IsAny<int>())).Returns(mockMeterReadings.AsQueryable());

            var meterReadingService = CreateMeterReadingServiceFromMocks();
            //meterReadingService.

        }
    }
}
