using System;

class Program
{
    static void Main(string[] args)
    {
        Reference reference = new Reference("Proverbs", 3, 5, 6);

        Scripture scripture = new Scripture(reference,
            "Trust in the Lord with all thine heart and lean not unto thine own understanding");

        string input = "";

        while (input != "quit" && !scripture.IsCompletelyHidden())
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());

            Console.WriteLine("\nPress Enter to continue or type 'quit' to exit:");
            input = Console.ReadLine();

            if (input != "quit")
            {
                scripture.HideRandomWords();
            }
        }

        Console.Clear();
        Console.WriteLine(scripture.GetDisplayText());
    }
}
