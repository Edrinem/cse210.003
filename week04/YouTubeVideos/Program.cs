// ── Video 1 ────────────────────────────────────────────────────
var video1 = new Video("How to Bake Sourdough Bread", "BreadCraft", 742);
video1.AddComment(new Comment("Sarah N.",  "Finally a recipe that actually works!"));
video1.AddComment(new Comment("James Y.",  "I tried this and my loaf came out perfect."));
video1.AddComment(new Comment("PastryFan", "The crumb structure looks incredible."));

// ── Video 2 ────────────────────────────────────────────────────
var video2 = new Video("Top 10 Python Tips for Beginners", "CodeWithEdrine", 615);
video2.AddComment(new Comment("Lena K.",   "Tip #7 saved me so much time, thank you!"));
video2.AddComment(new Comment("DevDave",   "I never knew about list comprehensions before this."));
video2.AddComment(new Comment("NewCoder",  "Subscribed after the first two minutes."));
video2.AddComment(new Comment("Maya R.",   "Please make a part 2!"));

// ── Video 3 
var video3 = new Video("Hiking the Rocky Mountain Trail", "WildPathMedia", 1083);
video3.AddComment(new Comment("TrailBlazer", "Been on this trail twice, gorgeous views."));
video3.AddComment(new Comment("OutdoorSam",  "Great tips on gear selection."));
video3.AddComment(new Comment("HikingNerd",  "The altitude section was really helpful."));

// ── Video 4 ────────────────────────────────────────────────────
var video4 = new Video("Beginner Guitar Lesson: Chords", "MelodySchool", 540);
video4.AddComment(new Comment("GuitarHope",  "I finally got the F chord after watching this!"));
video4.AddComment(new Comment("MusicLover",  "Very patient teacher, easy to follow."));
video4.AddComment(new Comment("RockFan99",   "Could you do a lesson on bar chords next?"));
video4.AddComment(new Comment("ChordNovice", "Watched this three times and it clicked!"));

// ── Put all videos in a list ────────────────────────────────────
var videos = new List<Video> { video1, video2, video3, video4 };

// ── Display each video and its comments ────────────────────────
foreach (var video in videos)
{
    Console.WriteLine("================================");
    Console.WriteLine($"Title:    {video.Title}");
    Console.WriteLine($"Author:   {video.Author}");
    Console.WriteLine($"Length:   {video.Length} seconds");
    Console.WriteLine($"Comments: {video.GetNumberOfComments()}");
    Console.WriteLine("--- Comments ---");

    foreach (var comment in video.GetComments())
    {
        Console.WriteLine($"  {comment.Name}: {comment.Text}");
    }

    Console.WriteLine();
}
