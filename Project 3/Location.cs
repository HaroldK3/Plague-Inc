namespace Project_3
{
    public class Location
    {
        public string ID { get; private set; }
        public ICollection<Person> People { get; private set; }
        public ICollection<Location> Neighbors { get; private set; }
        public int StartingPopulation { get; private set; }

        public Location()
        {
            ID = string.Empty;
            People = new List<Person>();
            Neighbors = new List<Location>();
        }

        public Location(string iD, ICollection<Person> people, ICollection<Location> neighbors, int startingPopulation)
        {
            ID = iD;
            People = people;
            Neighbors = neighbors;
            StartingPopulation = startingPopulation;
        }

        /// <summary>
        /// Generates a number of locations.
        /// </summary>
        /// <param name="locations">A list for all locations to be stored.</param>
        /// <param name="num">The number of locations to generate.</param>
        /// <param name="populationMean">The starting population mean.</param>
        /// <param name="populationDeviation">The starting population standard deviation.</param>
        /// <param name="quarantineChanceMean">The quarantine chance mean.</param>
        /// <param name="quarantineChanceDeviation">The quarantine chance standard deviation.</param>
        public static void GenerateLocations(ICollection<Location> locations, int num, int populationMean, int populationDeviation, double quarantineChanceMean, double quarantineChanceDeviation)
        {
            Random rand = new Random();

            for (int i = 1; i <= num; i++)
            {
                Location location = new Location();
                location.ID = i.ToString();

                //RNG for StartingPopulation.
                int popMin = Math.Max(0, populationMean - populationDeviation);
                int popMax = populationMean + populationDeviation;

                location.StartingPopulation = rand.Next(popMin, popMax + 1);

                //Generating the location's People.
                Person.GeneratePeople(location.ID, location.People, location.StartingPopulation, quarantineChanceMean, quarantineChanceDeviation);
                locations.Add(location);
            }

            //Loading the locations neighbors.

            foreach (Location location in locations)
            {

                foreach (Location location2 in locations)
                {
                    if (location.Neighbors == null)
                    {
                        location.Neighbors = new List<Location>();
                    }
                    if (location != location2)
                    {
                        if (Math.Abs(int.Parse(location2.ID) - int.Parse(location.ID)) == 1 ||
                            (location.ID == "1" && location2.ID == locations.Count.ToString()) ||
                            (location.ID == locations.Count.ToString() && location2.ID == "1"))
                        {
                            location.Neighbors.Add(location2);
                        }
                    }
                }
            }
        }
    }
}
