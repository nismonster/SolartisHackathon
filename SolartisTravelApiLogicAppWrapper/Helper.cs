using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SolartisTravelApiLogicAppWrapper
{
    public sealed class Helper
    {
        private static Helper instance = null;
        private static readonly object padlock = new object();

        List<string> _city = new List<string>();
        List<string> _stateFull = new List<string>();
        List<string> _stateCd = new List<string>();


        List<string> _destCity = new List<string>();
        List<string> _destCountry = new List<string>();
        Helper()
        {
            if (_city.Count == 0)
            {
                using (var reader = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}\popular_cities.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (values.Length > 1)
                        {
                            _city.Add(values[0]);
                            _stateFull.Add(values[1]);
                            _stateCd.Add(values[2]);
                        }
                    }
                }
                using (var reader = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}\cities_countries.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (values.Length > 1)
                        {
                            _destCity.Add(values[0]);
                            _destCountry.Add(values[1]);
                        }
                    }
                }
            }
        }

        public static Helper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Helper();
                    }
                    return instance;
                }
            }
        }

        public string ExtractStateCodeFromLocation(string incLocation)
        {
            incLocation = incLocation.IndexOf(',') > -1 ? incLocation.Substring(0, incLocation.IndexOf(',')) : incLocation;
            return !string.IsNullOrWhiteSpace(incLocation) && _city.FindIndex(x => x.Equals(incLocation, StringComparison.OrdinalIgnoreCase)) > -1 ?
                _stateCd[_city.FindIndex(x => x.Equals(incLocation, StringComparison.OrdinalIgnoreCase))] :
                !string.IsNullOrWhiteSpace(incLocation) && _stateFull.FindIndex(x => x.Equals(incLocation, StringComparison.OrdinalIgnoreCase)) > -1 ?
                _stateCd[_stateFull.FindIndex(x => x.Equals(incLocation, StringComparison.OrdinalIgnoreCase))] :
                incLocation;
        }


        public string ExtractCountryFromLocation(string incLocation)
        {
            incLocation = incLocation.IndexOf(',') > -1 ? incLocation.Substring(0, incLocation.IndexOf(',')) : incLocation;
            return !string.IsNullOrWhiteSpace(incLocation) && _destCity.FindIndex(x => x.Equals(incLocation, StringComparison.OrdinalIgnoreCase)) > -1 ?
                _destCountry[_destCity.FindIndex(x => x.Equals(incLocation, StringComparison.OrdinalIgnoreCase))] :
                incLocation;
        }
    }
}
