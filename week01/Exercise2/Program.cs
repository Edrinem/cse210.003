using System;
using System.Runtime.CompilerServices;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your garde percentage? ");
        string answer=Console.ReadLine();
        int score=int.Parse(answer);

        string grade ="";
        
        if(score>90)
        {
            grade="A";
        }
        else if(score>80)
        {
            grade="B";
        }
        else if (score>70)
        {
            grade="C";
        
        }
        else if(score>60)
        {
            grade="D";
        }
        else
        {
            grade="F";
        }

        Console.WriteLine($"Your grade is {grade}");

    
    }
}