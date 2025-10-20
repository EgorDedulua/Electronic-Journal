using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
           StringBuilder sb = new StringBuilder("Площадь");
            sb.AppendFormat(" {0:f2} см ", 123.456);
            Console.WriteLine(sb);
        }
    }
}