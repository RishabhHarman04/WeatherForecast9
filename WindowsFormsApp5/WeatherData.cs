using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp5.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static WindowsFormsApp5.SettingForm;

namespace WindowsFormsApp5
{
    public partial class WeatherData : Form
    {
        private WeatherService _weatherService;
        private int _panelIndex = 0;
        private int _totalCities = 0;
        private WeatherSettings _settings;

        public WeatherData(WeatherSettings _weathersettings)
        {
            InitializeComponent();
            InitializeFormSize();
            this._settings = _weathersettings;
            this._weatherService = new WeatherService();

            if (_settings != null && _settings._cities != null && _settings._refreshTime != null)
            {
                _totalCities = _settings._cities.Count;
                timer1.Interval = (int)(_settings._refreshTime) * 1000;
            }

            UpdateWeatherCity();

            timer1.Start();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            _panelIndex++;
            if (_panelIndex >= _totalCities)
            {
                _panelIndex = 0;
            }

            UpdateWeatherCity();
        }

        private async Task<string> GetWeatherDataForCity(string _city)
        {
            return await _weatherService.GetWeatherData(_city);
        }

        private async Task RefreshWeatherInfoForCity(string _city)
        {
            string _weatherData = await GetWeatherDataForCity(_city);

            if (_weatherData == null)
            {
                MessageBox.Show(MyStrings.Error);
                return;
            }

            WeatherInfo _weatherInfo = ExtractWeatherData(_weatherData);
            UpdateWeatherDisplay(_city, _weatherInfo._temperature, _weatherInfo._humidity, _weatherInfo._pressure);
        }

        private async void UpdateWeatherCity()
        {
            if (_settings != null && _settings._cities != null && _settings._cities.Any())
            {
                string _city = _settings._cities.ElementAt(_panelIndex);
                await RefreshWeatherInfoForCity(_city);
            }
        }

        private dynamic ParseWeatherData(string _weatherData)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(_weatherData);
        }

        private WeatherInfo CreateWeatherInfo(dynamic _json)
        {
            double _temperature = _json.main.temp;
            double _humidity = _json.main.humidity;
            double _pressure = _json.main.pressure;

            return new WeatherInfo(_temperature, _humidity, _pressure);
        }

        private WeatherInfo ExtractWeatherData(string _weatherData)
        {
            dynamic _json = ParseWeatherData(_weatherData);
            WeatherInfo _weatherInfo = CreateWeatherInfo(_json);

            return _weatherInfo;
        }

        private void UpdateWeatherDisplay(string _city, double _temperature, double _humidity, double _pressure)
        {
            label1.Text = _city;
            lblHumidity.Text = String.Format(MyStrings.HumidityText, _humidity);
            lblAtmosphere.Text = String.Format(MyStrings.AtmoshphereText, _pressure);
            lblTemperature.Text = String.Format(MyStrings.TemperatureText, _temperature);
        }

        private void DisplaySettingForm()
        {
            SettingForm _settingForm = new SettingForm(_settings);
            _settingForm.ShowDialog(this);
            if (_settingForm._weathersettings != null)
            {
                _settings = _settingForm._weathersettings;

                if (_settings._cities != null && _settings._refreshTime != null)
                {
                    _totalCities = _settings._cities.Count;
                    timer1.Interval = (int)(_settings._refreshTime) * 1000;
                }

                UpdateWeatherCity();
                timer1.Start();
            }
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            DisplaySettingForm();
        }

        private void InitializeFormSize()
        {
            this.Width = 700;
            this.Height = 550;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
        }

       
        private void WeatherData_Shown(object sender, EventArgs e)
        {
           
            DisplaySettingForm();

        }
    }
}









