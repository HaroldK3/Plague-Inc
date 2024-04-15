namespace Project_3
{
    public class Person
    {
        public string ID { get; private set; }
        public int TravelStartTime { get; private set; }
        public int TravelEndTime { get; private set; }
        public bool IsTraveling { get; private set; }
        public bool IsInfected { get; private set; }
        public int InfectedCount { get; private set; }
        public int InfectedSpreadCount { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsQuarantined { get; private set; }
        public double QuarantineChance { get; private set; }
        public int DiseaseDurationRemaining { get; private set; }
        public bool HasNaturalImmunity { get; private set; }

        public Person()
        {
            ID = string.Empty;
        }

        public Person(string iD, int travelStartTime, int travelEndTime, bool isTraveling, bool isInfected, int infectedCount,
            int infectedSpreadCount, bool isDead, bool isQuarantined, double quarantineChance, int diseaseDurationRemaining, bool hasNaturalImmunity)
        {
            ID = iD;
            TravelStartTime = travelStartTime;
            TravelEndTime = travelEndTime;
            IsTraveling = isTraveling;
            IsInfected = isInfected;
            InfectedCount = infectedCount;
            InfectedSpreadCount = infectedSpreadCount;
            IsDead = isDead;
            IsQuarantined = isQuarantined;
            QuarantineChance = quarantineChance;
            DiseaseDurationRemaining = diseaseDurationRemaining;
            HasNaturalImmunity = hasNaturalImmunity;
        }

        /// <summary>
        /// Generates a number of person objects for a location.
        /// </summary>
        /// <param name="locationID">The location's ID.</param>
        /// <param name="people">The location's people collection to add the person objects to.</param>
        /// <param name="population">The population of the location.</param>
        /// <param name="quarantineChanceMean">The quarantine chance mean.</param>
        /// <param name="quarantineChanceDeviation">The quarantine chance standard deviation.</param>
        public static void GeneratePeople(string locationID, ICollection<Person> people, int population, double quarantineChanceMean, double quarantineChanceDeviation)
        {
            if (people == null)
            {
                people = new List<Person>();
            }

            for (int i = 1; i <= population; i++)
            {
                Person person = new Person();
                person.ID = $"{locationID}{i}";

                //RNG for TravelStartTime and TravelEndTime on a 24-hour clock.
                Random rand = new Random();
                person.TravelStartTime = rand.Next(1, 25);
                int plusTimeToEndTravel = rand.Next(1, 25);
                if (person.TravelStartTime + plusTimeToEndTravel > 24)
                {
                    int diffCount = 24 - person.TravelStartTime;
                    plusTimeToEndTravel -= diffCount;
                    person.TravelEndTime = plusTimeToEndTravel;
                }
                else
                {
                    person.TravelEndTime = person.TravelStartTime + plusTimeToEndTravel;
                }

                //Zeroing properties
                person.IsTraveling = false;
                person.IsInfected = false;
                person.InfectedCount = 0;
                person.InfectedSpreadCount = 0;
                person.IsDead = false;
                person.IsQuarantined = false;

                //RNG for Immunity Chance
                Random random = new Random();
                bool HasNaturalImm = random.NextDouble() < .05;
                person.HasNaturalImmunity = HasNaturalImm;

                if (person.HasNaturalImmunity)
                {
                    person.IsInfected = false;
                }

                //RNG for QuarantineChance
                double quarantineChanceMin = quarantineChanceMean - quarantineChanceDeviation;
                if (quarantineChanceMin < 0)
                {
                    quarantineChanceMin = 0;
                }

                double quarantineChanceMax = quarantineChanceMean + quarantineChanceDeviation;
                if (quarantineChanceMax > 100)
                {
                    quarantineChanceMax = 100;
                }

                do
                {
                    person.QuarantineChance = rand.NextDouble() * 100;
                } while (person.QuarantineChance < quarantineChanceMin || person.QuarantineChance > quarantineChanceMax);
                people.Add(person);
            }
        }

        /// <summary>
        /// A setter to set IsInfected.
        /// </summary>
        /// <param name="status">bool flag.</param>
        /// <param name="diseaseDuration">Disease duration from config.</param>
        public void InfectedStatus(bool status, int diseaseDuration = 0)
        {
            IsInfected = status;
            if (status)
            {
                InfectedCount++;
                DiseaseDurationRemaining = diseaseDuration;
            }
            else
            {
                DiseaseDurationRemaining = diseaseDuration;
            }
        }

        /// <summary>
        /// A setter to set IsDead to true.
        /// </summary>
        public void Kill()
        {
            IsDead = true;
        }

        /// <summary>
        /// A setter to set IsQuarantined.
        /// </summary>
        /// <param name="status">bool flag.</param>
        public void QuarantineStatus(bool status)
        {
            IsQuarantined = status;
        }

        /// <summary>
        /// A setter for IsTraveling.
        /// </summary>
        /// <param name="status">bool flag.</param>
        public void TravelStatus(bool status)
        {
            IsTraveling = status;
        }

        /// <summary>
        /// Decreases the DiseaseDurationRemaining by 1.
        /// </summary>
        public void DecreaseDiseaseDuration()
        {
            DiseaseDurationRemaining--;
        }

        /// <summary>
        /// Increase InfectedSpreadCount by 1.
        /// </summary>
        public void IncreaseSpreadCount()
        {
            if (!HasNaturalImmunity && IsInfected)
            {
                InfectedSpreadCount++;
            }
        }

        /// <summary>
        /// Returns Spread Count.
        /// </summary>
        public int GetSpreadCount()
        {
            return InfectedSpreadCount;
        }

        /// <summary>
        /// Returns how many times infected.
        /// </summary>
        public int GetInfectedCount()
        {
            return InfectedCount;
        }

        /// <summary>
        /// Returns If infected.
        /// </summary>
        public bool GetIfInfected()
        {
            return IsInfected;
        }

        /// <summary>
        /// Returns if quarantined.
        /// </summary>
        public bool GetIfQuarantined()
        {
            return IsQuarantined;
        }
    }
}
