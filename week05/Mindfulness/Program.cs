// Mindfulness Program - C# Implementation
// Exceeds requirements by:
// 1. Tracking a log of completed activities (count per type) saved to mindfulness_log.txt
// 2. Ensuring reflection questions are not repeated until all have been used in a session
// 3. Animated breathing text that grows/shrinks to show breath pace

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

// ─────────────────────────────────────────────
//  BASE CLASS
// ─────────────────────────────────────────────
abstract class Activity
{
    protected string _name;
    protected string _description;
    protected int _duration; // seconds

    protected Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    // Shared starting message — asks user for duration, pauses before beginning
    public void DisplayStartMessage()
    {
        Console.Clear();
        Console.WriteLine($"=== {_name} Activity ===\n");
        Console.WriteLine(_description);
        Console.WriteLine();
        Console.Write("How many seconds would you like? ");
        while (!int.TryParse(Console.ReadLine(), out _duration) || _duration < 5)
        {
            Console.Write("Please enter a number >= 5: ");
        }
        Console.WriteLine("\nGet ready to begin...");
        ShowSpinner(3);
    }

    // Shared ending message — tells user what they completed and for how long
    public void DisplayEndMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Well done! Great job!");
        ShowSpinner(2);
        Console.WriteLine($"\nYou completed the {_name} activity for {_duration} seconds.");
        ShowSpinner(3);
    }

    // Shows an animated spinner for a given number of seconds
    protected void ShowSpinner(int seconds)
    {
        string[] frames = { "|", "/", "-", "\\" };
        int total = seconds * 10; // 10 ticks per second → 100ms each
        for (int i = 0; i < total; i++)
        {
            Console.Write($"\r  {frames[i % frames.Length]} ");
            Thread.Sleep(100);
        }
        Console.Write("\r     \r"); // clear spinner line
    }

    // Shows a numeric countdown (e.g. 5... 4... 3...)
    protected void ShowCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"\r  {i}... ");
            Thread.Sleep(1000);
        }
        Console.Write("\r        \r");
    }

    // Each subclass implements its own core loop
    public abstract void Run();
}

// ─────────────────────────────────────────────
//  BREATHING ACTIVITY
// ─────────────────────────────────────────────
class BreathingActivity : Activity
{
    public BreathingActivity() : base(
        "Breathing",
        "This activity will help you relax by walking you through breathing in and out slowly.\nClear your mind and focus on your breathing.")
    { }

    public override void Run()
    {
        DisplayStartMessage();

        int elapsed = 0;
        bool inhale = true;
        int breathSeconds = 4;

        while (elapsed < _duration)
        {
            string action = inhale ? "Breathe in..." : "Breathe out...";
            Console.WriteLine($"\n  {action}");
            AnimateBreath(breathSeconds, inhale);
            elapsed += breathSeconds;
            inhale = !inhale;
        }

        DisplayEndMessage();
    }

    // Extra credit: animated text that grows then slows to mimic breath pace
    private void AnimateBreath(int seconds, bool growing)
    {
        string[] sizes = { "·", "• ", "●  ", "◉   ", "⬤    " };
        int steps = sizes.Length;
        int msPerStep = (seconds * 1000) / (steps * 2);

        // Expand phase
        for (int i = 0; i < steps; i++)
        {
            Console.Write($"\r  {sizes[i]}        ");
            Thread.Sleep(msPerStep + (growing ? i * 20 : 0)); // slows near end of inhale
        }
        // Contract phase
        for (int i = steps - 1; i >= 0; i--)
        {
            Console.Write($"\r  {sizes[i]}        ");
            Thread.Sleep(msPerStep);
        }
        Console.Write("\r              \r");
    }
}

// ─────────────────────────────────────────────
//  REFLECTION ACTIVITY
// ─────────────────────────────────────────────
class ReflectionActivity : Activity
{
    private static readonly List<string> _prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private static readonly List<string> _questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity() : base(
        "Reflection",
        "This activity will help you reflect on times in your life when you have shown\nstrength and resilience. This will help you recognize the power you have\nand how you can use it in other aspects of your life.")
    { }

    public override void Run()
    {
        DisplayStartMessage();

        // Pick a random prompt
        var rng = new Random();
        string prompt = _prompts[rng.Next(_prompts.Count)];
        Console.WriteLine($"\n  {prompt}\n");
        Console.WriteLine("  Reflect on this experience...");
        ShowSpinner(5);

        // Extra credit: shuffle questions so none repeat until all used
        var shuffled = new List<string>(_questions);
        Shuffle(shuffled, rng);
        int qIndex = 0;

        int elapsed = 5;
        int pauseSeconds = 8;

        while (elapsed < _duration)
        {
            if (qIndex >= shuffled.Count)
            {
                // All used — reshuffle for another round
                Shuffle(shuffled, rng);
                qIndex = 0;
            }
            Console.WriteLine($"\n  > {shuffled[qIndex++]}");
            ShowSpinner(Math.Min(pauseSeconds, _duration - elapsed));
            elapsed += pauseSeconds;
        }

        DisplayEndMessage();
    }

    private void Shuffle<T>(List<T> list, Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}

// ─────────────────────────────────────────────
//  LISTING ACTIVITY
// ─────────────────────────────────────────────
class ListingActivity : Activity
{
    private static readonly List<string> _prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt grateful this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity() : base(
        "Listing",
        "This activity will help you reflect on the good things in your life\nby having you list as many things as you can in a certain area.")
    { }

    public override void Run()
    {
        DisplayStartMessage();

        var rng = new Random();
        string prompt = _prompts[rng.Next(_prompts.Count)];
        Console.WriteLine($"\n  {prompt}");
        Console.WriteLine("\n  You have a few seconds to think before you start listing...");
        ShowCountdown(5);

        Console.WriteLine("  Start listing! Press Enter after each item.\n");

        var items = new List<string>();
        DateTime end = DateTime.Now.AddSeconds(_duration - 5);

        // Let user type until time is up
        while (DateTime.Now < end)
        {
            Console.Write("  > ");
            // Non-blocking read with timeout check
            string entry = TimedReadLine(end - DateTime.Now);
            if (entry != null && entry.Trim() != "")
                items.Add(entry.Trim());
        }

        Console.WriteLine($"\n  You listed {items.Count} item{(items.Count == 1 ? "" : "s")}!");
        DisplayEndMessage();
    }

    // Reads a line from console, returning null if the deadline passes
    private string TimedReadLine(TimeSpan timeout)
    {
        string result = null;
        var thread = new Thread(() => { result = Console.ReadLine(); });
        thread.IsBackground = true;
        thread.Start();
        thread.Join(timeout < TimeSpan.Zero ? TimeSpan.Zero : timeout);
        return result;
    }
}

// ─────────────────────────────────────────────
//  ACTIVITY LOG (Extra Credit)
// ─────────────────────────────────────────────
class ActivityLog
{
    private const string LogFile = "mindfulness_log.txt";
    private Dictionary<string, int> _counts = new Dictionary<string, int>();

    public ActivityLog()
    {
        Load();
    }

    public void Record(string activityName)
    {
        if (!_counts.ContainsKey(activityName))
            _counts[activityName] = 0;
        _counts[activityName]++;
        Save();
    }

    public void Display()
    {
        Console.WriteLine("\n  --- Activity Log ---");
        if (_counts.Count == 0)
        {
            Console.WriteLine("  No activities recorded yet.");
            return;
        }
        foreach (var entry in _counts)
            Console.WriteLine($"  {entry.Key,-12}: {entry.Value} time{(entry.Value == 1 ? "" : "s")}");
        Console.WriteLine();
    }

    private void Save()
    {
        using var w = new StreamWriter(LogFile);
        foreach (var kv in _counts)
            w.WriteLine($"{kv.Key}:{kv.Value}");
    }

    private void Load()
    {
        if (!File.Exists(LogFile)) return;
        foreach (var line in File.ReadAllLines(LogFile))
        {
            var parts = line.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                _counts[parts[0]] = count;
        }
    }
}

// ─────────────────────────────────────────────
//  PROGRAM ENTRY POINT
// ─────────────────────────────────────────────
class Program
{
    static void Main()
    {
        var log = new ActivityLog();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Mindfulness App ===\n");
            Console.WriteLine("  1. Breathing Activity");
            Console.WriteLine("  2. Reflection Activity");
            Console.WriteLine("  3. Listing Activity");
            Console.WriteLine("  4. View Activity Log");
            Console.WriteLine("  5. Quit\n");
            Console.Write("Choose an activity: ");

            string choice = Console.ReadLine();

            Activity activity = null;

            switch (choice)
            {
                case "1": activity = new BreathingActivity(); break;
                case "2": activity = new ReflectionActivity(); break;
                case "3": activity = new ListingActivity(); break;
                case "4":
                    log.Display();
                    Console.WriteLine("  Press Enter to continue...");
                    Console.ReadLine();
                    continue;
                case "5":
                    Console.WriteLine("\nTake care. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    Thread.Sleep(1000);
                    continue;
            }

            activity.Run();
            log.Record(activity.GetType().Name.Replace("Activity", ""));

            Console.WriteLine("\n  Press Enter to return to the menu...");
            Console.ReadLine();
        }
    }
}