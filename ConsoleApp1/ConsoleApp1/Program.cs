using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        //Вариант 14
        static void Main(string[] args)
        {
            string str = "a b cat d dog";
            StringBuilder result = new StringBuilder();
            foreach (string word in str.Split(" "))
            {
                if (word.Length > 1)
                    result.Append(String.Concat(word, " "));
            }
            Console.WriteLine(result);
        }
    }
}