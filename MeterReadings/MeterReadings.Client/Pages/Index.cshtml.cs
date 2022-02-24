using MeterReadings.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace MeterReadings.Client.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile? Upload { get; set; }

        public UploadResponse? UploadResults { get; set; }

        public async Task<IActionResult> OnPostAsync(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        var apiUrl = Environment.GetEnvironmentVariable("API_URL")!;
                        client.BaseAddress = new Uri(apiUrl);

                        byte[] data;
                        using (var br = new BinaryReader(upload.OpenReadStream()))
                            data = br.ReadBytes((int)upload.OpenReadStream().Length);

                        ByteArrayContent bytes = new ByteArrayContent(data);


                        MultipartFormDataContent multiContent = new MultipartFormDataContent();

                        multiContent.Add(bytes, "readings", upload.FileName);

                        var response = await client.PostAsync("api/meter-reading-uploads", multiContent);
                        var responseJson = await response.Content.ReadAsStringAsync();

                        UploadResults = JsonSerializer.Deserialize<UploadResponse>(responseJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    catch (Exception ex)
                    {
                        return Page(); // 500 is generic server error
                    }
                }
            }
            return Page();
        }
    }
}