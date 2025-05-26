// See https://aka.ms/new-console-template for more information
using RetrainingSchedular;
using RetrainingSchedular.Models;

Console.Clear();
ShowWelcomeMessage();
List<Talk> talks = GetTalksFromUser();

// Create an instance of the Schedular class with the talks list
Schedular sc = new Schedular(talks);
// Schedule the tracks using the ScheduleTracks method
var tracks = sc.ScheduleTracks();

// initailize output lines to store the formatted schedule used for printing and downloading
List<string> outputLines = new();
outputLines.Add("NewGlobe Retraining Scheduler");

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

    outputLines.Add("");  // Blank line before track
    outputLines.Add($"Track: {trackNum}");
    outputLines.Add("");

    // print display table header
    FormatPrint("Time", "Session Name", "Duration");
    var columnLines = new string('-', 7) + "-|-" + new string('-', 50) + "-|-" + new string('-', 7);
    Console.WriteLine(columnLines);
    outputLines.Add(columnLines);


    // loops print morning session time
    foreach (var session in morningSchedule)
        outputLines.Add(FormatPrint(session.Time, session.Talk.Title, session.Talk.Duration == 5 ? "lightning" : session.Talk.Duration + "Mins"));

    // print lunch time
    outputLines.Add(FormatPrint("12:00PM", "Lunch", ""));

    // loops print afternoon session time
    foreach (var session in afternoonSchedule)
        outputLines.Add(FormatPrint(session.Time, session.Talk.Title, session.Talk.Duration == 5 ? "lightning" : session.Talk.Duration + "Mins"));

    // print sharing session time
    outputLines.Add(FormatPrint(Helpers.FormatTime(track.SharingTimeStart), "Sharing Session", ""));


    Console.WriteLine();
    Console.WriteLine();
    outputLines.Add("");
    outputLines.Add("");

    trackNum++;
}


// Print the final output to the console if the user wants to see it
AskToDownloadSchedule(outputLines);


/// <summary>
/// Displays a welcome message and prompts the user with options to proceed.
/// </summary>
static void ShowWelcomeMessage()
{
    Console.WriteLine("---------------------------------------------------------");
    Console.WriteLine("NewGlobe Retraining Scheduler 1.0");
    Console.WriteLine("----------------------------------------------------------");
    Console.WriteLine();
    Console.WriteLine("Hi, How would you like to proceed?");
    Console.WriteLine("1. Proceed with Default Talks");
    Console.WriteLine("2. Enter new Talks");
}


// <summary>
/// Prompts the user to choose between using a default list of talks or entering new ones manually.
/// </summary>
/// <returns>
/// A list of <see cref="Talk"/> objects representing the user's selected or entered talks.
/// </returns>
/// <remarks>
/// If the user selects option 1, a predefined list of talks is returned.
/// If the user selects option 2, the user is prompted to enter talk titles and durations.
/// Durations can be in minutes or specified as 'lightning' (which is treated as 5 minutes).
/// </remarks>
static List<Talk> GetTalksFromUser()
{
    List<Talk> defaultTalks =
    [
        new Talk("Organising Parents for Academy Improvements", 60),
                new Talk("Teaching Innovations in the Pipeline", 45),
                new Talk("Teacher Computer Hacks", 30),
                new Talk("Making Your Academy Beautiful", 45),
                new Talk("Academy Tech Field Repair", 45),
                new Talk("Sync Hard", 5),
                new Talk("Unusual Recruiting", 5),
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
        {
            return defaultTalks;
        }
        else if (choice == "2")
        {
            Console.WriteLine("Please enter your talks in the format: Title, Duration (in minutes or 'lightning')");
            Console.WriteLine("e.g Teaching Innovations, 45");
            Console.WriteLine("e.g Fun Teaching Tricks, lightning");
            Console.WriteLine("Type 'done' when you are finished.");

            List<Talk> newTalks = new();

            while (true)
            {
                var input = Console.ReadLine();
                if (input?.ToLower() == "done") break;

                var parts = input?.Split(',');
                if (parts?.Length == 2)
                {
                    string title = parts[0].Trim();
                    string durationPart = parts[1].Trim().ToLower();

                    if (!Helpers.IsValidTitle(title))
                    {
                        Console.WriteLine("Invalid title. Titles should not contain numbers.");
                        continue;
                    }

                    int duration;
                    if (durationPart == "lightning")
                    {
                        duration = 5;
                    }
                    else if (!int.TryParse(durationPart, out duration))
                    {
                        Console.WriteLine("Invalid duration. Please enter a number (e.g., 45) or 'lightning'");
                        continue;
                    }

                    newTalks.Add(new Talk(title, duration));
                }
                else
                {
                    Console.WriteLine("Invalid input. Use format: Title, Duration (e.g., 'Talk Title, 30')");
                }
            }

            return newTalks.Count > 0 ? newTalks : defaultTalks;
        }
        else
        {
            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
        }
    }
}


/// <summary>
/// Prompts the user to choose whether to download the schedule as a text file.
/// If the user selects "1" for Yes, the method calls <see cref="DownloadToTextFile(List{string})"/>
/// to save the provided schedule lines to the user's Downloads folder.
/// </summary>
/// <param name="outputLines">A list of formatted strings representing the schedule to be written to the file.</param>
static void AskToDownloadSchedule(List<string> outputLines)
{
    Console.WriteLine("Would you like to download the schedule as a text file?");
    Console.WriteLine("1: Yes");
    Console.WriteLine("2: No");

    Console.Write("Enter your choice: ");
    var input = Console.ReadLine()?.Trim();

    if (input == "1")
        Helpers.DownloadToTextFile(outputLines);
    else
        Console.WriteLine("Download skipped.");
}


/// <summary>
/// Prints a formatted line containing the session time, name, and duration.
/// </summary>
/// <param name="time">The time of the session (e.g., "10:00").</param>
/// <param name="sessionName">The name or description of the session.</param>
/// <param name="duration">The duration of the session (e.g., "1h").</param>
/// <returns>A formatted string representing a session row, also printed to console.</returns>
static string FormatPrint(string time, string sessionName, string duration)
{
    var line = string.Format("{0,-7} | {1,-50} | {2,-7}", time, sessionName, duration);
    Console.WriteLine(line);
    return line;
}

