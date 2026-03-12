using System;
using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();

        int userInput=-1;
        while(userInput!=0)
        {
            Console.Write("Enter a number (enter 0 to quit): ");
            userInput=int.Parse(Console.ReadLine());
            if(userInput!=0)
            {
                numbers.Add(userInput);

            }
        }
        int sum=0;
        foreach(int number in numbers)
        {
            sum+=number;
        }
        Console.WriteLine($"the sum is {sum}");
        float average=sum/numbers.Count;
        Console.WriteLine($"the average number is {average}");

        int max=numbers[0];
        foreach(int number in numbers)        {
            if(number>max)
            {
                max=number;
            }
        }
        Console.WriteLine($"the biggest number is {max}");

    }
}