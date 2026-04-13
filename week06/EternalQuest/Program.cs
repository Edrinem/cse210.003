// ============================================================
// Eternal Quest — C# Program
// ============================================================
// EXCEEDING REQUIREMENTS:
//   1. Leveling system: the player gains a titled rank (e.g.,
//      "Seasoned Adventurer") based on total score thresholds.
//      The current level and title are shown in the header.
//   2. Negative goals: a new derived class (NegativeGoal) lets
//      users track bad habits and LOSE points each time they
//      record a slip, encouraging accountability.
//   3. Bonus streak for Eternal goals: every 7 consecutive
//      recordings of an EternalGoal earns a +50 streak bonus,
//      rewarding consistency.
//   4. All four goal types are saved/loaded from a plain-text
//      file using a type-tag system (no external libraries).
// ============================================================

using System;
using System.Collections.Generic;
using System.IO;

// ── Base class ────────────────────────────────────────────────
abstract class Goal
{
    private string _name;
    private string _description;
    private int    _pointValue;

    public string Name        => _name;
    public string Description => _description;
    public int    PointValue  => _pointValue;

    protected Goal(string name, string description, int pointValue)
    {
        _name        = name;
        _description = description;
        _pointValue  = pointValue;
    }

    // Returns points earned (may be negative). Updates internal state.
    public abstract int RecordEvent();

    // True when the goal is finished and should show [X].
    public abstract bool IsComplete { get; }

    // One-line status shown in the list view.
    public abstract string GetStatus();

    // Serialise to a single line of text.
    public abstract string Serialise();

    // Shared helper for derived classes to access point value during save.
    protected int RawPointValue => _pointValue;
}

// ── SimpleGoal ────────────────────────────────────────────────
class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int pointValue)
        : base(name, description, pointValue)
    {
        _isComplete = false;
    }

    // Restore from file
    public SimpleGoal(string name, string description, int pointValue, bool complete)
        : base(name, description, pointValue)
    {
        _isComplete = complete;
    }

    public override bool IsComplete => _isComplete;

    public override int RecordEvent()
    {
        if (_isComplete)
        {
            Console.WriteLine("  (This goal is already complete!)");
            return 0;
        }
        _isComplete = true;
        Console.WriteLine($"  +{RawPointValue} points! Goal complete!");
        return RawPointValue;
    }

    public override string GetStatus()
    {
        string check = _isComplete ? "[X]" : "[ ]";
        return $"{check} {Name}  ({Description})  +{RawPointValue} pts";
    }

    public override string Serialise() =>
        $"SIMPLE|{Name}|{Description}|{RawPointValue}|{_isComplete}";
}

// ── EternalGoal ───────────────────────────────────────────────
class EternalGoal : Goal
{
    private int _timesRecorded;
    private const int StreakBonus     = 50;
    private const int StreakThreshold = 7;

    public EternalGoal(string name, string description, int pointValue)
        : base(name, description, pointValue)
    {
        _timesRecorded = 0;
    }

    public EternalGoal(string name, string description, int pointValue, int timesRecorded)
        : base(name, description, pointValue)
    {
        _timesRecorded = timesRecorded;
    }

    public override bool IsComplete => false; // never complete

    public override int RecordEvent()
    {
        _timesRecorded++;
        int earned = RawPointValue;

        if (_timesRecorded % StreakThreshold == 0)
        {
            earned += StreakBonus;
            Console.WriteLine($"  +{RawPointValue} points! 🔥 {StreakThreshold}-time streak bonus +{StreakBonus}! Total: +{earned}");
        }
        else
        {
            Console.WriteLine($"  +{earned} points!");
        }
        return earned;
    }

    public override string GetStatus() =>
        $"[ ] {Name}  ({Description})  +{RawPointValue} pts  [recorded {_timesRecorded}×]";

    public override string Serialise() =>
        $"ETERNAL|{Name}|{Description}|{RawPointValue}|{_timesRecorded}";
}

// ── ChecklistGoal ─────────────────────────────────────────────
class ChecklistGoal : Goal
{
    private int  _timesCompleted;
    private int  _requiredCount;
    private int  _bonusPoints;

    public ChecklistGoal(string name, string description, int pointValue, int requiredCount, int bonusPoints)
        : base(name, description, pointValue)
    {
        _requiredCount  = requiredCount;
        _bonusPoints    = bonusPoints;
        _timesCompleted = 0;
    }

    public ChecklistGoal(string name, string description, int pointValue,
                         int requiredCount, int bonusPoints, int timesCompleted)
        : base(name, description, pointValue)
    {
        _requiredCount  = requiredCount;
        _bonusPoints    = bonusPoints;
        _timesCompleted = timesCompleted;
    }

    public override bool IsComplete => _timesCompleted >= _requiredCount;

    public override int RecordEvent()
    {
        if (IsComplete)
        {
            Console.WriteLine("  (Checklist already complete!)");
            return 0;
        }

        _timesCompleted++;
        int earned = RawPointValue;

        if (_timesCompleted >= _requiredCount)
        {
            earned += _bonusPoints;
            Console.WriteLine($"  +{RawPointValue} points + BONUS +{_bonusPoints} = +{earned}! Checklist complete!");
        }
        else
        {
            Console.WriteLine($"  +{earned} points! ({_timesCompleted}/{_requiredCount} done)");
        }
        return earned;
    }

    public override string GetStatus()
    {
        string check = IsComplete ? "[X]" : "[ ]";
        return $"{check} {Name}  ({Description})  +{RawPointValue} pts  [Completed {_timesCompleted}/{_requiredCount}]  bonus: +{_bonusPoints}";
    }

    public override string Serialise() =>
        $"CHECKLIST|{Name}|{Description}|{RawPointValue}|{_requiredCount}|{_bonusPoints}|{_timesCompleted}";
}

// ── NegativeGoal (exceeds requirements) ──────────────────────
class NegativeGoal : Goal
{
    private int _slipCount;

    public NegativeGoal(string name, string description, int pointValue)
        : base(name, description, pointValue)
    {
        _slipCount = 0;
    }

    public NegativeGoal(string name, string description, int pointValue, int slipCount)
        : base(name, description, pointValue)
    {
        _slipCount = slipCount;
    }

    public override bool IsComplete => false; // accountability goal — never "done"

    public override int RecordEvent()
    {
        _slipCount++;
        Console.WriteLine($"  -{RawPointValue} points. Stay strong — you've slipped {_slipCount} time(s).");
        return -RawPointValue;
    }

    public override string GetStatus() =>
        $"[ ] {Name}  ({Description})  -{RawPointValue} pts per slip  [slipped {_slipCount}×]";

    public override string Serialise() =>
        $"NEGATIVE|{Name}|{Description}|{RawPointValue}|{_slipCount}";
}

// ── Player & level system ─────────────────────────────────────
class Player
{
    private int    _score;
    private string _name;

    private static readonly (int threshold, string title)[] Levels =
    {
        (0,     "Wandering Apprentice"),
        (500,   "Novice Quester"),
        (1200,  "Seasoned Adventurer"),
        (2500,  "Brave Champion"),
        (5000,  "Legendary Knight"),
        (10000, "Mythic Hero"),
        (20000, "Eternal Guardian"),
        (50000, "Cosmic Legend"),
    };

    public int    Score => _score;
    public string Name  => _name;

    public Player(string name) { _name = name; _score = 0; }
    public Player(string name, int score) { _name = name; _score = score; }

    public void AddPoints(int pts)
    {
        int prevLevel = CurrentLevel();
        _score = Math.Max(0, _score + pts);
        int newLevel = CurrentLevel();
        if (newLevel > prevLevel)
            Console.WriteLine($"\n  *** LEVEL UP! You are now a {CurrentTitle()}! ***\n");
    }

    public int CurrentLevel()
    {
        int lvl = 0;
        for (int i = 0; i < Levels.Length; i++)
            if (_score >= Levels[i].threshold) lvl = i;
        return lvl + 1;
    }

    public string CurrentTitle()
    {
        string title = Levels[0].title;
        for (int i = 0; i < Levels.Length; i++)
            if (_score >= Levels[i].threshold) title = Levels[i].title;
        return title;
    }
}

// ── Main Program ──────────────────────────────────────────────
class Program
{
    private static List<Goal> _goals  = new List<Goal>();
    private static Player     _player = new Player("Hero");
    // Save file always lives next to the executable so relative-path issues can't cause lost data.
    private static readonly string SaveFile =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "eternal_quest_save.txt");

    static void Main(string[] args)
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(name)) name = "Hero";
        _player = new Player(name);

        bool running = true;
        while (running)
        {
            PrintHeader();
            Console.WriteLine("  1. View goals");
            Console.WriteLine("  2. Add new goal");
            Console.WriteLine("  3. Record goal event");
            Console.WriteLine("  4. Save progress");
            Console.WriteLine("  5. Load progress");
            Console.WriteLine("  6. Quit");
            Console.Write("\nChoice: ");
            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1": ViewGoals();      break;
                case "2": AddGoal();        break;
                case "3": RecordGoal();     break;
                case "4": SaveProgress();   break;
                case "5": LoadProgress();   break;
                case "6": running = false;  break;
                default:
                    Console.WriteLine("  Invalid choice, try again.\n");
                    break;
            }
        }
        Console.WriteLine("Farewell, adventurer. Your quest continues...");
    }

    static void PrintHeader()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║           ⚔  ETERNAL QUEST  ⚔            ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        Console.WriteLine($"  Player : {_player.Name}");
        Console.WriteLine($"  Level  : {_player.CurrentLevel()} — {_player.CurrentTitle()}");
        Console.WriteLine($"  Score  : {_player.Score} XP");
        Console.WriteLine();
    }

    static void ViewGoals()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("  (No goals yet. Add some!)\n");
            Pause();
            return;
        }
        Console.WriteLine("\n── Your Goals ─────────────────────────────");
        for (int i = 0; i < _goals.Count; i++)
            Console.WriteLine($"  {i + 1}. {_goals[i].GetStatus()}");
        Console.WriteLine();
        Pause();
    }

    static void AddGoal()
    {
        Console.WriteLine("\n  Goal type:");
        Console.WriteLine("    1. Simple (one-time completion)");
        Console.WriteLine("    2. Eternal (repeating, never finished)");
        Console.WriteLine("    3. Checklist (N completions required)");
        Console.WriteLine("    4. Negative (lose points for bad habit)");
        Console.Write("  Type: ");
        string typeChoice = Console.ReadLine()?.Trim();

        Console.Write("  Name: ");
        string gName = Console.ReadLine()?.Trim() ?? "Unnamed";

        Console.Write("  Short description: ");
        string gDesc = Console.ReadLine()?.Trim() ?? "";

        Console.Write("  Points per event: ");
        int pts = int.TryParse(Console.ReadLine(), out int p) ? p : 100;

        switch (typeChoice)
        {
            case "1":
                _goals.Add(new SimpleGoal(gName, gDesc, pts));
                break;
            case "2":
                _goals.Add(new EternalGoal(gName, gDesc, pts));
                break;
            case "3":
                Console.Write("  Required completions: ");
                int req = int.TryParse(Console.ReadLine(), out int r) ? r : 10;
                Console.Write("  Completion bonus points: ");
                int bon = int.TryParse(Console.ReadLine(), out int b) ? b : 500;
                _goals.Add(new ChecklistGoal(gName, gDesc, pts, req, bon));
                break;
            case "4":
                _goals.Add(new NegativeGoal(gName, gDesc, pts));
                break;
            default:
                Console.WriteLine("  Invalid type.");
                break;
        }
        Console.WriteLine("  Goal added!\n");
        Pause();
    }

    static void RecordGoal()
    {
        if (_goals.Count == 0) { Console.WriteLine("  No goals to record.\n"); Pause(); return; }

        Console.WriteLine("\n── Record a Goal Event ────────────────────");
        for (int i = 0; i < _goals.Count; i++)
            Console.WriteLine($"  {i + 1}. {_goals[i].Name}");

        Console.Write("  Select goal #: ");
        if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > _goals.Count)
        {
            Console.WriteLine("  Invalid selection.\n");
            Pause();
            return;
        }

        int earned = _goals[idx - 1].RecordEvent();
        _player.AddPoints(earned);
        Console.WriteLine($"  Score is now: {_player.Score} XP\n");
        Pause();
    }

    static void SaveProgress()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(SaveFile))
            {
                sw.WriteLine($"PLAYER|{_player.Name}|{_player.Score}");
                foreach (Goal g in _goals)
                    sw.WriteLine(g.Serialise());
            }
            Console.WriteLine($"  Progress saved!\n  File: {SaveFile}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ERROR saving: {ex.Message}\n");
        }
        Pause();
    }

    static void LoadProgress()
    {
        if (!File.Exists(SaveFile))
        {
            Console.WriteLine($"  No save file found at:\n  {SaveFile}\n");
            Pause();
            return;
        }

        _goals.Clear();
        string[] lines = File.ReadAllLines(SaveFile);

        foreach (string line in lines)
        {
            string[] parts = line.Split('|');
            switch (parts[0])
            {
                case "PLAYER":
                    _player = new Player(parts[1], int.Parse(parts[2]));
                    break;
                case "SIMPLE":
                    _goals.Add(new SimpleGoal(parts[1], parts[2], int.Parse(parts[3]), bool.Parse(parts[4])));
                    break;
                case "ETERNAL":
                    _goals.Add(new EternalGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4])));
                    break;
                case "CHECKLIST":
                    _goals.Add(new ChecklistGoal(parts[1], parts[2], int.Parse(parts[3]),
                                                 int.Parse(parts[4]), int.Parse(parts[5]), int.Parse(parts[6])));
                    break;
                case "NEGATIVE":
                    _goals.Add(new NegativeGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4])));
                    break;
            }
        }
        Console.WriteLine($"  Progress loaded! Welcome back, {_player.Name}.\n");
        Pause();
    }

    static void Pause()
    {
        Console.Write("  Press Enter to continue...");
        Console.ReadLine();
    }
}