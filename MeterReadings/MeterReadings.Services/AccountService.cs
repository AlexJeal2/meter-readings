using System.Text.Json;

namespace MeterReadings.Services
{
    public class AccountService
    {
        public void SeedDatabase()
        {
            var csv = new List<string[]>(); // or, List<YourClass>
            var lines = System.IO.File.ReadAllLines(@"Test_Accounts.csv");
            foreach (string line in lines)
                csv.Add(line.Split(',')); // or, populate YourClass          
            var s = JsonSerializer.Serialize(csv);
            Console.WriteLine(s);
        }
    }
}