using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    public class WeatherInfo
    {
        public double _temperature;
        public double _humidity;
        public double _pressure;

        public WeatherInfo(double _temperature, double _humidity, double _pressure)
        {
            this._temperature = _temperature;
            this._humidity = _humidity;
            this._pressure = _pressure;
        }
    }
}
