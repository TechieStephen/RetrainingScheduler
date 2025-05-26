# 📅 Retraining Scheduler – NewGlobe Coding Challenge (C#)

## 🔍 Overview

This is a command-line scheduling tool developed for the NewGlobe Full Stack Engineer coding challenge. It creates an optimized schedule for elective training sessions by organizing talks into multiple **tracks** (each with a **morning session**, **afternoon session**, and a **sharing session**). The program supports both default and user-inputted talks, handles time formatting, and ensures constraints are strictly followed.

---

## ✅ Features

- 🔁 **Backtracking algorithm** for optimal session fitting (maximizing utilization).
- ⏰ Sessions handled with precise **start times** and **AM/PM formatting**.
- 🧪 Unit-tested using **xUnit** for reliability.
- 🧹 Clean validation to prevent invalid titles and inputs.
- 💬 Interactive console application for flexible user input.
- 📋 Supports talks defined in minutes or as `"lightning"` (5-minute sessions).
- 📚 Produces clearly formatted schedules, grouped by track.

---

## 🗂️ Project Structure

RetrainingScheduler/
│
├── Models/
│   ├── Talk.cs            # Model for training session (title, duration)
│   └── Track.cs           # Model for a single track (morning + afternoon + sharing session)
│
├── Helpers.cs             # Validation, time formatting, schedule formatting
├── Schedular.cs           # Core logic (backtracking, greedy fitting, track generation)
├── Program.cs             # CLI entry point for user interaction and output
│
├── RetrainingSchedular.Test/
│   ├── HelpersTest.cs     # Unit tests for Helpers
│   └── SchedularTest.cs   # Unit tests for Schedular logic


## ▶️ Running the Project

**Prerequisites:**

1.  Ensure you have **.NET 8.0 SDK or later** installed on your system. You can check your installed SDKs by running `dotnet --list-sdks` in your command line. If you don't have it, download it from the official .NET website.

**Running in Visual Studio:**

1.  **Open the Solution:** Launch Visual Studio and open the solution file (`.sln`) of your `RetrainingSchedular` project.
2.  **Set as Startup Project (Optional but Recommended):** In the Solution Explorer (View > Solution Explorer), right-click on the `RetrainingSchedular` project and select **"Set as Startup Project"**. This ensures that this project is the one that runs when you start debugging.
3.  **Run with Debugging:** Press **F5** or click the **"Start Debugging"** button (the green play arrow with "RetrainingSchedular" next to it in the toolbar). This will build and run your application with the debugger attached, allowing you to set breakpoints, inspect variables, etc.
4.  **Run without Debugging:** Press **Ctrl + F5** or click the **"Start without Debugging"** button (the green play arrow in the toolbar). This will build and run your application without attaching the debugger.

**Running from the Command Line (CLI):**

1.  **Open Terminal/Command Prompt:** Open a terminal (on macOS/Linux) or command prompt (on Windows).
2.  **Navigate to Project Root:** Use the `cd` command to navigate to the root directory of your `RetrainingSchedular` project. This is the directory that contains the `.csproj` file for your application.
3.  **Run the Application:** Execute the following command:

    ```bash
    dotnet run --project RetrainingSchedular
    ```

    * `dotnet`: The .NET CLI command.
    * `run`: A command that builds and runs your project.
    * `--project RetrainingSchedular`: Specifies the project file to run. Make sure `"RetrainingSchedular"` matches the name of your `.csproj` file (without the `.csproj` extension).

**After Running:**

Whether you run from Visual Studio or the command line, the application will start, and you should see the following options in your console:


Select 1 to proceed with default talks.

Select 2 to manually enter your own list of talks:

Format: Title, Duration

Use lightning for 5-minute talks.

Type done to finish input.


## 🧠 Scheduling Logic
Morning Session: 9:00 AM – 12:00 PM (180 minutes)

Afternoon Session: 1:00 PM – 4:00–5:00 PM (max 240 minutes)

Sharing Session: Begins after 4 PM, no later than 5 PM (ends at 5:30 PM)

Backtracking is used to find the best combination of talks that fills each session without exceeding the limits.

## 📝 Assumptions
All talk titles are free of numbers (validated).

"lightning" talks are interpreted as 5 minutes.

Lunch is fixed at 12:00 PM.

Talks are only scheduled once across all tracks.

## 💡 Future Improvements
Export schedules to PDF or CSV.

Add web-based UI using Blazor or ASP.NET Core.

Support fixed-time talks (e.g., keynote always at 9 AM).

Enable user-defined track/session durations via config.

## 👨‍💻 Author
Eduke Ohien Stephen
Submitted for the NewGlobe Full Stack Engineer Coding Challenge.
Built with ❤️ using C# and .NET.


## 📌 Future Enhancements
- Lunch Timing: Could be dynamic (based on end of morning session), though fixed at 12PM is acceptable per spec.
- Scalability: For large dataset maybe fine turn the backtracking algorithm or use dynamic programming approuch
- Configurable Constraints (optional): Hardcoded constants like 180/240 minutes could be refactored to config parameters. (not really a big deal though)