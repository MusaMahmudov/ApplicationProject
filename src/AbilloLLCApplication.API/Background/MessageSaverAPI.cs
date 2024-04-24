using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Business.Tools;
using AbilloLLCApplication.Core.Entities;
using AbilloLLCApplication.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AbilloLLCApplication.API.Background
{
    public class MessageSaverAPI : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageService _messageService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public MessageSaverAPI(IConfiguration configuration, IWebHostEnvironment webHostEnvironment,IServiceProvider serviceProvider, IMessageService messageService)
        {
            _webHostEnviroment = webHostEnvironment;
            _configuration = configuration;
            _messageService = messageService;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        string SecretFilePath = Path.Combine(_webHostEnviroment.WebRootPath,"secret", "");
                        string SecretFilePathToken = Path.Combine(_webHostEnviroment.WebRootPath, "secret", "");
                        var messages = await _messageService.GetMessageAsync(SecretFilePath, SecretFilePathToken);
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        List<Cargo> newCargos = new List<Cargo>(messages.Count);
                        foreach (var message in messages)
                        {
                            if (await dbContext.Cargoes.AnyAsync(c => c.checkId == message.CheckId))
                            {
                                continue;
                            }
                            if (message.PickUpZipcode is not null)
                            {
                                HttpClient client = new HttpClient();
                                HttpResponseMessage response = await client.GetAsync($"empty");
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    string jsonStrinf = await response.Content.ReadAsStringAsync();
                                    JsonDocument jsonDoc = JsonDocument.Parse(jsonStrinf);
                                    JsonElement root = jsonDoc.RootElement;
                                    var results = root.GetProperty("results");
                                    JsonElement pickUpCodeInfo;
                                    try
                                    {
                                        if (results.TryGetProperty(message.PickUpZipcode, out pickUpCodeInfo))
                                        {

                                            JsonElement pickUpLatitudeElement;
                                            JsonElement pickUpLongitudeElement;


                                            if (pickUpCodeInfo[0].TryGetProperty("latitude", out pickUpLatitudeElement) && pickUpCodeInfo[0].TryGetProperty("longitude", out pickUpLongitudeElement))
                                            {
                                                message.Longitude = pickUpLongitudeElement.ToString().Replace(".", ",");
                                                message.Latitude = pickUpLatitudeElement.ToString().Replace(".", ",");


                                            }


                                        }
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }


                                }
                            }
                            else
                            {
                                HttpClient httpClient = new HttpClient();
                                httpClient.DefaultRequestHeaders.Add("X-Api-Key", $"{_configuration["CityApiKey:Key"]}");
                                using var response = httpClient.GetAsync($"");
                                if (response.Result.IsSuccessStatusCode)
                                {
                                    var body = await response.Result.Content.ReadAsStringAsync();
                                    var result = JsonDocument.Parse(body);
                                    var root = result.RootElement;
                                    var latitude = root[0].GetProperty("latitude");
                                    var longitude = root[0].GetProperty("longitude");
                                    message.Latitude = latitude.ToString();
                                    message.Longitude = longitude.ToString();

                                }

                            }

                            Cargo newCargo = new Cargo()
                            {
                                IsTaken = false,
                                checkId = message.CheckId,
                                Miles = message.Miles,
                                Pieces = message.Pieces,
                                Weight = message.Weight,
                                PickUpZipcode = message.PickUpZipcode,
                                PickUpCity = message.PickUpCity,
                                DeliverToZipcode = message.DeliverToZipcode,
                                DeliverToCity = message.DeliverToCity,
                                ToEmail = message.ToEmail,
                                FromEmail = message.FromEmail,
                                Notes = message.Notes,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "Admin",
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = "Admin",
                                Height = message.Height,
                                Length = message.Length,
                                Width = message.Width,
                                Latitude = message.Latitude,
                                Longitude = message.Longitude,

                            };
                            newCargos.Add(newCargo);


                        }
                        if (newCargos.Count > 0)
                        {

                            dbContext.Cargoes.AddRange(newCargos);
                            await dbContext.SaveChangesAsync();
                            var drivers = dbContext.Drivers;
                            foreach (var cargo in newCargos)
                            {
                                double pickUpLatitude;
                                double pickUpLongitude;
                                if (double.TryParse(cargo.Latitude, out pickUpLatitude) && double.TryParse(cargo.Longitude, out pickUpLongitude))
                                {
                                    pickUpLongitude /= 10000;
                                    pickUpLatitude /= 10000;
                                    var driversToSendMessage = new List<Driver>();
                                    foreach (var driver in drivers)
                                    {
                                        if (cargo.Length * cargo.Pieces <= driver.Length && GeoCalculator.CalculateDistance(driver.Latitude ?? 1, driver.Longitude ?? 1, pickUpLatitude, pickUpLongitude) <= 200 && driver.TelegramUserId is not null)
                                        {

                                            string text = $"({cargo.PickUpCity}-{cargo.DeliverToCity}, Miles: {cargo.Miles}, {cargo.Weight} lbs.)\n" +
                                            $"PickUp:  {cargo.PickUpCity} {cargo.PickUpZipcode}\n" +
                                            $"DeliverTo: {cargo.DeliverToCity} {cargo.DeliverToZipcode}\n" +
                                            $"Link: ";

                                            try
                                            {
                                                HttpClient client = new HttpClient();

                                                await client.GetAsync($"");

                                            }
                                            catch (Exception ex)
                                            {
                                                throw new Exception("Exception occured while sending message");
                                            }


                                        }
                                    }

                                }

                            }
                        }

                    }
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);


                }
                catch (Exception ex)
                {

                }


            }
        }
    }
}

