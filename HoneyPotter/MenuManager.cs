using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoneyPotter
{
    public class MenuManager
    {
        private bool _menuLoaded = false;
        public void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine("#####################################################");
            Console.WriteLine("##               HoneyPotter V 0.0                 ##");
            Console.WriteLine("#####################################################");
        }

        public bool LoadStart()
        {
            PrintHeader();
            Console.WriteLine("1.Configure");
            Console.WriteLine("2.Exit");

            Console.Write(">");

            //string consoleKey = string.Empty;
            StringBuilder consoleKey = new StringBuilder();
            while (true)
            {
                var inputKey = Console.ReadKey();

                consoleKey.Append(inputKey.Key.ToString());
                if (consoleKey.ToString() == "D1Enter")
                {
                    Console.WriteLine();
                    Console.WriteLine("Configure menu, does not work right now");
                    Thread.Sleep(1000);
                    return false;
                }
                if (consoleKey.ToString() == "D2Enter")
                {
                    return true;
                }

                if (inputKey.Key == ConsoleKey.Enter)
                    return false;
            }

            //if(input == "1")
            //{
            //    //TODO FIX THIS LATER!
            //}
            //else if(input == "2")
            //{
            //    Console.WriteLine("Closing application...");
            //    Thread.Sleep((int)TimeSpan.FromSeconds(2).TotalMilliseconds);
            //    return true;
            //}

            return false;
        }
    }
}
