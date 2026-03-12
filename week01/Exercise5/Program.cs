using System;

class Program
{
    static void Main(string[] args)
    {
        DisplayWelcome();
        string name = PromptUserName();
        int number = PromptUserNumber();
        float square = SquareNumber(number);

        

        static void DisplayWelcome()
        {
            Console.WriteLine("Welcome to the program!");
        }

        static string PromptUserName()
        {
            Console.Write("Please enter your full name. ");
            string userName = Console.ReadLine();
            return userName;
        }

        static int PromptUserNumber()
        {
            Console.Write("Please enter your favorite number. ");
            int userNumber = int.Parse(Console.ReadLine());
            return userNumber;
        }
        static float SquareNumber(int userNumber)
        {
            float squaredNumber = userNumber * userNumber;
            return squaredNumber;

        }
        Console.WriteLine($"{name}, the square of your number is {square}.");

    }
}