using MeterReadings.Data;
using MeterReadings.Data.Models;
using MeterReadings.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MeterReadings.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestAccountRepository
    {
        [Test]
        public void TestAddAccountsChecksForExistingAccounts()
        {
            var accounts = new List<Account>
            {
                new Account(1, "Foo", "Test"),
                new Account(2, "Bar", "Test"),
                new Account(3, "Baz", "Test"),
            };
            var accountsQueryable = accounts.AsQueryable();
            var mockSet = TestHelpers.CreateMockDbSet(accountsQueryable);

            var mockContextOptions = new DbContextOptions<AppDbContext>();

            var mockContext = new Mock<AppDbContext>(mockContextOptions);
            mockContext.Setup(c => c.Accounts).Returns(mockSet.Object);

            var newAccounts = new List<Account>()
            {
                new Account(3, "Baz", "Test"),
                new Account(4, "New", "Test"),
                new Account(5, "Account", "Test"),
            };

            var repo = new AccountRepository(mockContext.Object);
            repo.AddAccounts(accounts);

            var expectedAdditions = new List<Account>()
            {
                new Account(4, "New", "Test"),
                new Account(5, "Account", "Test"),
            };

            mockContext.Verify(x => x.Accounts.AddRange(
                It.Is<IEnumerable<Account>>(x => x.All(account => HasAccount(expectedAdditions, account)))),
                Times.Once);
        }

        private bool HasAccount(List<Account> accounts, Account account)
        {
            return accounts.Any(x => x.AccountId == account.AccountId
            && x.FirstName == account.FirstName
            && x.LastName == account.LastName);
        }
    }
}
