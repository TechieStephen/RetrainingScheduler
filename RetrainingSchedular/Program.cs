// See https://aka.ms/new-console-template for more information
using RetrainingSchedular;
using RetrainingSchedular.Models;

Console.WriteLine("---------------------------------------------------------");
Console.WriteLine("NewGlobe Retraining Scheduler 1.0");
Console.WriteLine("----------------------------------------------------------");


Console.WriteLine();
Console.WriteLine("Hi, How would you like to procced?");
Console.WriteLine("1. Proceed with Default Talks");
Console.WriteLine("2. Enter new Talks");



List<Talk> talks =
[
    new Talk("Organising Parents for Academy Improvements", 60),
    new Talk("Teaching Innovations in the Pipeline", 45),
    new Talk("Teacher Computer Hacks", 30),
    new Talk("Making Your Academy Beautiful", 45),
    new Talk("Academy Tech Field Repair", 45),
    new Talk("Sync Hard", 5), // Assuming "lightning" means 5 minutes
    new Talk("Unusual Recruiting", 5), // Assuming "lightning" means 5 minutes
    new Talk("Parent Teacher Conferences", 60),
    new Talk("Managing Your Dire Allowance", 45),
    new Talk("Customer Care", 30),
    new Talk("AIMs – 'Managing Up'", 30),
    new Talk("Dealing with Problem Teachers", 45),
    new Talk("Hiring the Right Cook", 60),
    new Talk("Government Policy Changes and New Globe", 60),
    new Talk("Adjusting to Relocation", 45),
    new Talk("Public Works in Your Community", 30),
    new Talk("Talking To Parents About Billing", 30),
    new Talk("So They Say You're a Devil Worshipper", 60),
    new Talk("Two-Streams or Not Two-Streams", 30),
    new Talk("Piped Water", 30)
];

while (true)
{
    Console.Write("PLEASE ENTER A CHOICE (1 or 2): ");
    var choice = Console.ReadLine();
    if (choice == "1")
        break; // break the loop and proceed with default talks

    else if (choice == "2")
    {
        Console.WriteLine("Please enter your talks in the format: Title, Duration (in minutes or 'lightning')");
        Console.WriteLine("e.g Teaching Innovations, 45");
        Console.WriteLine("e.g Fun Teaching Tricks, lightning");
        Console.WriteLine("Type 'done' when you are finished.");

        List<Talk> newTalks = new List<Talk>();

        while (true)
        {
            var input = Console.ReadLine();
            if (input?.ToLower() == "done")
                break; // exit the loop if user is done entering talks

            var parts = input?.Split(','); // split input by comma
            if (parts?.Length == 2)
            {
                string title = parts[0].Trim(); // get the title and trim whitespace
                string durationPart = parts[1].Trim().ToLower(); // get the duration part, trim whitespace, and convert to lowercase

                if (!Helpers.IsValidTitle(title)) // check if the title is valid
                {
                    Console.WriteLine("Invalid title. Titles should not contain numbers.");
                    continue;
                }

                int duration;
                if (durationPart == "lightning") // check for 'lightning' duration
                {
                    duration = 5;
                }
                else if (!int.TryParse(durationPart, out duration))
                {
                    Console.WriteLine("Invalid duration. Please enter a number (e.g., 45) or 'lightning' for duration");
                    continue;
                }

                newTalks.Add(new Talk(title, duration)); // create a new Talk object and add it to the list
            }
            else
                Console.WriteLine("Invalid input. Please enter in the format: Title, Duration (e.g., 'Talk Title, 30' or 'Talk Title, lightning')");
        }

        if (newTalks.Count > 0)
            talks = newTalks;

        break; // Exit the loop after processing custom talks
    }
    else
        Console.WriteLine("Invalid choice. Please enter 1 or 2.");
}



// Create an instance of the Schedular class with the talks list
Schedular sc = new Schedular(talks);
//// Schedule the tracks using the ScheduleTracks method
var tracks = sc.ScheduleTracks();



int trackNum = 1;
foreach (var track in tracks)
{
    //Get the morning and afternoon schedules with respective time stamp using Helpers.CalculateStartTimes method
    var morningSchedule = Helpers.CalculateStartTimes(track.MorningTalks, new TimeSpan(9, 0, 0));
    var afternoonSchedule = Helpers.CalculateStartTimes(track.AfternoonTalks, new TimeSpan(13, 0, 0));

    //print track number
    Console.WriteLine();
    Console.WriteLine($"Track: {trackNum}");
    Console.WriteLine();

    // print display table header
    FormartPrint("Time", "Session Name", "Duration");
    Console.WriteLine(new string('-', 7) + "-|-" + new string('-', 50) + "-|-" + new string('-', 7));

    // loops print morning session time
    foreach (var session in morningSchedule)
        FormartPrint(session.Time, session.Talk.Title, session.Talk.Duration == 5 ? "lightning" : session.Talk.Duration + "Mins");

    // print lunch time
    Console.WriteLine("{0,-7} | {1,-50} | {2,-7}", "12:00PM", "Lunch", "");

    // loops print afternoon session time
    foreach (var session in afternoonSchedule)
        FormartPrint(session.Time, session.Talk.Title, session.Talk.Duration == 5 ? "lightning" : session.Talk.Duration + "Mins");

    // print sharing session time
    FormartPrint(Helpers.FormatTime(track.SharingTimeStart), "Sharing Session", "");


    Console.WriteLine();
    Console.WriteLine();

    trackNum++;
}



/// <summary>
/// Prints a formatted line containing the session time, name, and duration.
/// </summary>
/// <param name="time">The time of the session (e.g., "10:00").</param>
/// <param name="sessionName">The name or description of the session.</param>
/// <param name="duration">The duration of the session (e.g., "1h").</param>
static void FormartPrint(string time, string sessionName, string duration)
{
    Console.WriteLine("{0,-7} | {1,-50} | {2,-7}", time, sessionName, duration);
}

