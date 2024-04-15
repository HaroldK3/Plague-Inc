using Project_3;

//Sim setup
Configuration config = new Configuration();
ICollection<Location> locations = new List<Location>();
Location.GenerateLocations(locations, config.NumOfLocations, config.MeanPop, config.PopDeviation, config.QuarantineChanceMean, config.QuarantineChanceDeviation);
int hourClock = 0;
Random rand = new Random();

//Picking ground zero person.
int randLocation = rand.Next(locations.Count + 1);
foreach (Location location in locations)
{
    if (Convert.ToInt32(location.ID) == randLocation)
    {
        int randPerson = rand.Next(location.People.Count());
        int counter2 = 0;
        foreach (Person person in location.People)
        {
            if (counter2 == randPerson)
            {
                person.InfectedStatus(true, config.DiseaseDuration);
            }
            counter2++;
        }
    }
}

foreach (Location location in locations)
{
    Console.WriteLine(location.StartingPopulation);
}

/// <summary>
/// Creates the csv file and makes a header
/// </summary>
string filePath = "..\\..\\..\\..\\Project 3\\csv files\\sim_log.csv";

using (StreamWriter header = new StreamWriter(filePath))
{
    header.WriteLine("Hour,Highest Infected,Highest Spread,Alive,Dead,Currently Infected,Currently Quarantined");
}

//Sim Start
for (int hour = 0; hour < config.SimDuration; hour++)
{
    foreach (Location location in locations)
    {
        List<Person> list = new List<Person>(location.People);

        for (int a = 0; a < location.People.Count; a++)
        {
            Person person = list[a];

            //Quarantine stuff
            if (person.InfectedCount == 1 && !person.IsDead)
            {
                double quarantineRand = rand.NextDouble() * 100;
                if (quarantineRand <= person.QuarantineChance && !person.HasNaturalImmunity)
                {
                    person.QuarantineStatus(true);
                }
            }

            //Travel stuff
            if (person.TravelStartTime == hourClock && !person.IsDead)
            {
                person.TravelStatus(true);
            }
            if (person.TravelEndTime == hourClock && !person.IsDead)
            {
                person.TravelStatus(false);
            }
            if (person.IsTraveling && !person.IsQuarantined && !person.IsDead)
            {
                int travelDest = rand.Next(0, 2);

                //Setting location IDs
                int currentLocationID = Convert.ToInt32(location.ID);
                int neightborOne = currentLocationID - 1;
                if (neightborOne == 0)
                {
                    neightborOne = locations.Count;
                }
                int neightborTwo = currentLocationID + 1;
                if (currentLocationID == locations.Count)
                {
                    neightborTwo = 1;
                }

                //Setting the travelDest
                if (travelDest == 0)
                {
                    travelDest = neightborOne;
                }
                else
                {
                    travelDest = neightborTwo;
                }

                //Moving person
                foreach (Location neightbor in location.Neighbors)
                {
                    if (Convert.ToInt32(neightbor.ID) == travelDest)
                    {
                        neightbor.People.Add(person);
                        location.People.Remove(person);
                    }
                }
            }

            //Infected stuff
            if (person.IsInfected && !person.IsDead && !person.HasNaturalImmunity)
            {
                //Spreading
                double spreadRand = rand.NextDouble() * 100;
                if (spreadRand <= config.SpreadChance)
                {
                    person.IncreaseSpreadCount();
                    //RNG for who to infect
                    int randPerson = rand.Next(0, location.People.Count);
                    int counter = 0;
                    foreach (Person person2 in location.People)
                    {
                        if (counter == randPerson)
                        {
                            person2.InfectedStatus(true, config.DiseaseDuration);
                        }
                        counter++;
                    }
                }

                person.DecreaseDiseaseDuration();
                if (person.DiseaseDurationRemaining == 0)
                {
                    person.InfectedStatus(false);
                }

                //Death stuff
                double deathRand = rand.NextDouble() * 100;
                if (deathRand <= config.DeathChance)
                {
                    person.Kill();
                }
            }
        }
    }

    //Increment hourClock
    if (hourClock == 24)
    {
        hourClock = 0;
    }
    else
    {
        hourClock++;
    }



    //Ending sim early if all people are dead or there is no more infected
    bool atLeastOneInfected = false;
    bool atLeastOnePerson = false;
    foreach (Location locationss in locations)
    {
        foreach (Person person in locationss.People)
        {
            if (!person.IsDead)
            {
                atLeastOnePerson = true;
            }
            if (person.IsInfected)
            {
                atLeastOneInfected = true;
            }
        }
    }
    if (atLeastOneInfected == false && atLeastOnePerson == false)
    {
        Console.WriteLine("All have died.\n");
        break;
    }
    //Simulation spreadsheet report stuff
    /// <summary>
    /// Algorithms for the different counts.
    /// </summary>
    int highestInfected = 0;
    int alivePeople = 0;
    int deadPeople = 0;
    int highestSpread = 0;
    int currentlyInfected = 0;
    int currentlyQuarantined = 0;
    Person personWhosSpreadMost = new Person();
    Person personWhosInfectedMost = new Person();
    foreach (Location location in locations)
    {
        foreach (Person person in location.People)
        {
            //gets the person who's infected most
            if (person.GetInfectedCount() > highestInfected)
            {
                highestInfected = person.GetInfectedCount();
                personWhosInfectedMost = person;
            }

            //gets person who's spread most
            if (person.GetSpreadCount() > highestSpread)
            {
                highestSpread = person.GetSpreadCount();
                personWhosSpreadMost = person;
            }

            //counts people alive
            if (!person.IsDead)
            {
                alivePeople++;
            }
            //and dead
            else
            {
                deadPeople++;
            }

            //gets the # of people currently infected
            if (person.GetIfInfected())
            {
                currentlyInfected++;
            }
            //gets the # of people currently quarantined
            if (person.GetIfQuarantined())
            {
                currentlyQuarantined++;
            }
        }

        /// <summary>
        /// Updates The CSV file as the sim runs.
        /// </summary>
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{hourClock},{highestInfected}    {personWhosInfectedMost.ID},{highestSpread}    {personWhosSpreadMost.ID},{alivePeople}," +
                $"{deadPeople},{currentlyInfected},{currentlyQuarantined}");
        }

        highestInfected = 0;
        alivePeople = 0;
        deadPeople = 0;
        highestSpread = 0;
        currentlyInfected = 0;
        currentlyQuarantined = 0;
        personWhosSpreadMost = new Person();
        personWhosInfectedMost = new Person();
    }
}
