using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsumingRestAPI
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string countriesEndpoint = "https://restcountries.eu/rest/v2/all";
        private static int _countryGiniPlace;
        private static string _lowestGiniCountry;
        private static string _regionWithMostTimezones;
        private static int _amountOfTimezonesInRegion;

        static void Main(string[] args)
        {
            Country[] countries = GetCountries(countriesEndpoint).GetAwaiter().GetResult();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" Country Facts");
            Console.WriteLine();
            Console.ResetColor();

            Random rnd = new Random(); // random to populate fake answer - you can remove this once you use real values


            List<Country> countryList = countries.ToList<Country>(); //convert array to List

            var OrderedCountryListDESC = new List<Country>(countries.OrderByDescending(c => c.Gini)); //order by gini descending 

            foreach (Country country in OrderedCountryListDESC)
            {
                if (country.Name.Equals("South Africa") && country.Gini != null)
                {
                    _countryGiniPlace = OrderedCountryListDESC.IndexOf(country) + 1;
                }

                int min = (int)OrderedCountryListDESC.Min(c => c.Gini); //check the minimun gini value

                if (country.Gini == min)
                {
                    _lowestGiniCountry = country.Name;
                }
            }

            var regions = OrderedCountryListDESC.GroupBy(c => c.Region); //group countries by region

            Dictionary<Enum, int> dict = new Dictionary<Enum, int>(); //dictionary for key-value iterations

            foreach (var timezone in regions)
            {
                dict.Add(timezone.Key, timezone.Count());
            }

            foreach (var item in dict)
            {
                var maxValue = dict.Values.Max(); //get the max value in the dictionary
                if (item.Value == maxValue)
                {
                    _regionWithMostTimezones = item.Key.ToString();
                    _amountOfTimezonesInRegion = maxValue;
                }

            }
            dict.Clear(); //clear dictionary for next reusability


            int southAfricanGiniPlace = _countryGiniPlace; 
            Console.WriteLine($"1. South Africa's Gini coefficient is the {GetOrdinal(southAfricanGiniPlace)} highest");


            string lowestGiniCountry = _lowestGiniCountry;
            Console.WriteLine($"2. {lowestGiniCountry} has the lowest Gini Coefficient");

            string regionWithMostTimezones = _regionWithMostTimezones; 
            int amountOfTimezonesInRegion = _amountOfTimezonesInRegion; 
            Console.WriteLine($"3. {regionWithMostTimezones} is the region that spans most timezones at {amountOfTimezonesInRegion} timezones");

            /*
             * HINT: Count occurances of each currency in all countries (check `Country.Currencies`)
             * Find the name of the currency with most occurances and return it's name (`Currency.Name`) also return the number of occurances found for that currency          
             */
            string mostPopularCurrency = "ExampleCurrency"; // Use correct value
            int numCountriesUsedByCurrency = rnd.Next(1, 10); // Use correct value
            Console.WriteLine($"4. {mostPopularCurrency} is the most popular currency and is used in {numCountriesUsedByCurrency} countries");

            /*
             * HINT: Count the number of occurances of each language (`Country.Languages`) and sort then in descending occurances count (i.e. most populat first)
             * Once done return the names of the top three languages (`Language.Name`)
             */
            string[] mostPopularLanguages = { "ExampleLanguage1", "ExampleLanguage2", "ExampleLanguage3" }; // Use correct values
            Console.WriteLine($"5. The top three popular languages are {mostPopularLanguages[0]}, {mostPopularLanguages[1]} and {mostPopularLanguages[2]}");

            /*
             * HINT: Each country has an array of Bordering countries `Country.Borders`, The array has a list of alpha3 codes of each bordering country `Country.alpha3Code`
             * Sum up the population of each country (`Country.Population`) along with all of its bordering countries's population. Sort this list according to the combined population descending
             * Find the country with the highest combined (with bordering countries) population the return that country's name (`Country.Name`), the number of it's Bordering countries (`Country.Borders.length`) and the combined population
             * Be wary of null values           
             */
            string countryWithBorderingCountries = "ExampleCountry"; // Use correct value
            int numberOfBorderingCountries = rnd.Next(1, 10); // Use correct value
            int combinedPopulation = rnd.Next(1000000, 10000000); // Use correct value
            Console.WriteLine($"6. {countryWithBorderingCountries} and it's {numberOfBorderingCountries} bordering countries has the highest combined population of {combinedPopulation}");

            /*
             * HINT: Population density is calculated as (population size)/area, i.e. `Country.Population/Country.Area`
             * Calculate the population density of each country and sort by that value to find the lowest density
             * Return the name of that country (`Country.Name`) and its calculated density.
             * Be wary of null values when doing calculations           
             */
            string lowPopDensityName = "ExampleCountry"; // Use correct value
            double lowPopDensity = rnd.NextDouble() * 100; // Use correct value
            Console.WriteLine($"7. {lowPopDensityName} has the lowest population density of {lowPopDensity}");

            /*
             * HINT: Population density is calculated as (population size)/area, i.e. `Country.Population/Country.Area`
             * Calculate the population density of each country and sort by that value to find the highest density
             * Return the name of that country (`Country.Name`) and its calculated density.
             * Be wary of any null values when doing calculations. Consider reusing work from above related question           
             */
            string highPopDensityName = "ExampleCountry"; // Use correct value
            double highPopDensity = rnd.NextDouble() * 100; // Use correct value
            Console.WriteLine($"8. {highPopDensityName} has the highest population density of {highPopDensity}");

            /*
             * HINT: Group by subregion `Country.Subregion` and sum up the area (`Country.Area`) of all countries per subregion
             * Sort the subregions by the combined area to find the maximum (or just find the maximum)
             * Return the name of the subregion
             * Be wary of any null values when summing up the area           
             */
            string largestAreaSubregion = "ExampleSubRegion"; // Use correct value
            Console.WriteLine($"9. {largestAreaSubregion} is the subregion that covers the most area");

            /*
             * HINT: Group by regional blocks (`Country.RegionalBlocks`). For each regional block, average out the gini coefficient (`Country.Gini`) of all member countries
             * Sort the regional blocks by the average country gini coefficient to find the lowest (or find the lowest without sorting)
             * Return the name of the regional block (`RegionalBloc.Name`) along with the calculated average gini coefficient
             */
            string mostEqualRegionalBlock = "ExampleRegionalBlock"; // Use correct value
            double lowestRegionalBlockGini = rnd.NextDouble() * 10; // Use correct value
            Console.WriteLine($"10. {mostEqualRegionalBlock} is the regional block with the lowest average Gini coefficient of {lowestRegionalBlockGini}");

            Console.WriteLine("-----------------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            //Use this section to debug first, Lol
             

            List<String> currenciesInCountires = new List<string>();

            foreach (Country country in OrderedCountryListDESC)
            {
                
               foreach(Currency currency in country.Currencies) //currenty property inside the country class
                {
                    
                    currenciesInCountires.Add(currency.Name); //add all currenies to list
                    //Console.WriteLine("{0} is used in {1}", currency.Name, country.Name);
                    var countriesx = country.Currencies.Distinct().OrderByDescending(c => c.Name);
                    Console.WriteLine(countriesx);
                }

            }

            foreach (var item in currenciesInCountires)
            {
               // Console.WriteLine(item);
            }
            //Console.WriteLine(currenciesInCountires.Count());
            Console.ReadLine();






        }




        /// <summary>
        /// Gets the countries from a specified endpiny
        /// </summary>
        /// <returns>The countries.</returns>
        /// <param name="path">Path endpoint for the API.</param>
        static async Task<Country[]> GetCountries(string path)
        {
            Country[] countries = null;
            //TODO get data from endpoint and convert it to a typed array using Country.FromJson

            HttpResponseMessage response = await client.GetAsync(path);
            response.EnsureSuccessStatusCode();// throws exception if not succesful


            string result = await response.Content.ReadAsStringAsync();

            countries = Country.FromJson(result);
            return countries;
        }

        /// <summary>
        /// Gets the ordinal value of a number (e.g. 1 to 1st)
        /// </summary>
        /// <returns>The ordinal.</returns>
        /// <param name="num">Number.</param>
        public static string GetOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }
    }
}
