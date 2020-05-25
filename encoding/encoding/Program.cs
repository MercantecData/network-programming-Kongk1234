using System;
using System.Text;

namespace encoding
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "mcdonalds er en måge";
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            foreach(byte b in bytes)
            {
                Console.WriteLine(b);
            }
            string test = Encoding.ASCII.GetString(bytes);
            Console.WriteLine(test);

            byte[] bytes2 = Encoding.UTF8.GetBytes(text);
            foreach(byte a in bytes2)
            {
                Console.WriteLine(a);
            }
            string test2 = Encoding.UTF8.GetString(bytes2);
            Console.WriteLine(test2);
        }
    }
}
