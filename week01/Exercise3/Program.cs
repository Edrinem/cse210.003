using System;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        int magicNumber = randomGenerator.Next(1, 100);
        
        int guess=0;

       while(guess!=magicNumber)
       {
        Console.Write("what is your guess? ");
        guess=int.Parse(Console.ReadLine());
       if(guess>magicNumber)
        {
            Console.WriteLine("lower");
        }
        if(guess<magicNumber)
        {
            Console.WriteLine("higher");
        }
        if(guess==magicNumber)
        {
            Console.WriteLine("you got it!");
        }
       }

    }
}