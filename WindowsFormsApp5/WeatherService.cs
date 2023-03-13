using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp5.WeatherData;
using static WindowsFormsApp5.SettingForm;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public class WeatherService
    {
        private HttpClient _httpClient = new HttpClient();
        private string _apiKey = MyStrings.APIKEY;
        public async Task<string> GetWeatherData(string _city)
        {
            try
            {
                string _url = String.Format(MyStrings.WeatherAPIUrl,_city, _apiKey);
                var _response = await _httpClient.GetAsync(_url);
                _response.EnsureSuccessStatusCode();
                var _content = await _response.Content.ReadAsStringAsync();
                return _content;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(String.Format(MyStrings.HttpExceptionMessage,_city, ex.Message));
                return null;
            }
        }

      
    }
}
