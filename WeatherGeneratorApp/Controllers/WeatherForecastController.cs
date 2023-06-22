using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Text.Json;
using WeatherGeneratorApp.Models;
using WeatherGeneratorApp.OpenWeatherMap_Model;


namespace WeatherGeneratorApp.Controllers
{
    public class WeatherForecastController : Controller
    {

        public IActionResult Index()
        {
            ResultViewModel model = new ResultViewModel();  
            return View(model);  
        }
        public IActionResult Details()
        {
            ResultViewModel result = new ResultViewModel();
            return View(result);    
        }

        public async Task<IActionResult> GetWeather(string City)

        {
            ResultViewModel result = new ResultViewModel(); 
            string apiId = "6ce86757580c07a55e4677e54d3a6393";
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, apiId);
            string url1 = string.Format("http://api.openweathermap.org/geo/1.0/direct?q={0}&appid={1}", City, apiId);
            using var httpClient = new HttpClient();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url1);
            if(responseMessage.IsSuccessStatusCode)
            {
                string ResponseContent = await responseMessage.Content.ReadAsStringAsync();
                var DeserializeData = JsonSerializer.Deserialize<RootObject>(ResponseContent);
                result.Country = DeserializeData.sys.Country;
                result.City = DeserializeData.name;
                result.Lat = Convert.ToString(DeserializeData.coord.Lat);
                result.Lon = Convert.ToString(DeserializeData.coord.Lon);
                //result.Description = DeserializeData.weathers[0].Description;
                result.Humidity = Convert.ToString(DeserializeData.main.Humidity);
                result.Temp = Convert.ToInt64(DeserializeData.main.Temp);
                result.TempMax = Convert.ToInt64(DeserializeData.main.Temp_Max);
                result.TempMin = Convert.ToInt64(DeserializeData.main.Temp_Min);
                //result.WeatherIcon = DeserializeData.weathers[0].Icon;
                result.TempFeelsLike = Convert.ToInt64(DeserializeData.main.TempFeelsLike);

            }
            var jsonString = JsonSerializer.Serialize(result);
            return View("Details", result);
            
        }
        

        
    }
}
