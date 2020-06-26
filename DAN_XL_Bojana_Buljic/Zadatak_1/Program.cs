using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        public static string colorsFile = @"../../Paleta.txt";
        public static List<string> colors = new List<string>() { "red", "blue", "white", "yellow", "green", "orange", "violet" };
               
        public static string[] format = new string[] { "A3", "A4" };
        public static string[] orientation = new string[] { "portrait", "landscape" };
        //public static Random rnd = new Random();
        static object locker = new object();
        static object locker1 = new object();
        static object locker2 = new object();

        static AutoResetEvent event1 = new AutoResetEvent(false);
        static SemaphoreSlim a3semaphore = new SemaphoreSlim(1);
        static SemaphoreSlim a4semaphore = new SemaphoreSlim(1);

        static CountdownEvent countdown = new CountdownEvent(10);

        /// <summary>
        /// Method for writing paper colors in file
        /// </summary>
        public static void WriteColors()
        {
            Console.WriteLine("Writing paper colors in file....\n");
            using (StreamWriter sw = new StreamWriter(colorsFile))
            {
                foreach (var color in colors)
                {
                    sw.WriteLine(color);
                }
            }
        }

        /// <summary>
        /// Method for random selection of paper format
        /// </summary>
        /// <returns>string value for paper format</returns>
        public static string SelectPaperFormat()
        {
            Random rnd = new Random();
            string selectedFormat = (format[rnd.Next(format.Length)]);
            Console.WriteLine(selectedFormat);
            return selectedFormat;
        }

        /// <summary>
        /// Method for random selection of paper color from file
        /// </summary>
        /// <returns>string value for paper color</returns>
        public static string SelectPaperColor()
        {
            string[] allLines = File.ReadAllLines(colorsFile);
            Random rnd = new Random();
            string selectedColor = allLines[rnd.Next(allLines.Length)];
            Console.WriteLine(selectedColor);
            return selectedColor;
        }

        /// <summary>
        /// Method for random selection of paper orientation
        /// </summary>
        /// <returns>paper orientation</returns>
        public static string SelectOrientation()
        {
            Random rnd = new Random();
            string selectedOrientation = (orientation[rnd.Next(orientation.Length)]);
            Console.WriteLine(selectedOrientation);
            return selectedOrientation;
        }

        /// <summary>
        /// Method for A3 printer which prints A3 format documents
        /// </summary>
        public static void A3Printing()
        {
            lock (locker)
            {
                a3semaphore.Wait();
                Console.WriteLine("Document for " + Thread.CurrentThread.Name + " is printing...");
                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + " can pick up A3 document.");
                a3semaphore.Release();
                countdown.Signal();
            }
        }

        /// <summary>
        /// Method for A4 printer which prints A4 format documents
        /// </summary>
        public static void A4Pringing()
        {
            lock (locker1)
            {
                a4semaphore.Wait();
                Console.WriteLine("Document for " + Thread.CurrentThread.Name + " is printing...");
                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + " can pick up A4 document.");
                a4semaphore.Release();
                countdown.Signal();
            }
        }

        /// <summary>
        /// Method for simulating printing
        /// </summary>
        public static void Printing()
        {
            //selecting random paper format
            string selectedFormat = SelectPaperFormat();
            //selecting random paper color from file
            string selectedColor = SelectPaperColor();
            //select random orientation
            string selectedOrientation = SelectOrientation();
            

            Console.WriteLine(Thread.CurrentThread.Name + " sent request for printing " + selectedFormat + " format. Color: " +
                        selectedColor + ". Orientation: " + selectedOrientation);
            event1.Set();
            Thread.Sleep(100);

            event1.WaitOne();
            if (selectedFormat == "A3")
            {
                
            }
            else
            {
                
            }
            //lock (locker2)
            //{
            //    if (countdown.CurrentCount == 0)
            //    {
            //        countdown.Signal();
            //    }
            //}
            //Thread.Sleep(1000);

        }

        //public static void PrintRequest()
        //{
        //    Console.WriteLine("Please select paper format A3 or A4:");
        //    selectedFormat = (format[rnd.Next(format.Length)]);
        //    Console.WriteLine(selectedFormat + "\n");
        //    //SelectPaperFormat();
        //    Console.WriteLine("What paper color do you want to print on?");
        //    string[] allLines = File.ReadAllLines(colorsFile);
        //    selectedColor = allLines[rnd.Next(allLines.Length)];
        //    Console.WriteLine(selectedColor + "\n");
        //    //SelectPaperColor();
        //    Console.WriteLine("Select paper orientation (landscape/portrait)");
        //    selectedOrientation = (orientation[rnd.Next(orientation.Length)]);
        //    Console.WriteLine(selectedOrientation + "\n");

        //    Console.WriteLine(Thread.CurrentThread.Name + " sent request for printing " + selectedFormat + " format. Color: " +
        //                selectedColor + ". Orientation: " + selectedOrientation+"\nDocument is printing...\n");           
        //    event1.Set();
        //    Thread.Sleep(100);

        //    event1.WaitOne();
        //    if (selectedFormat == "A3")
        //    {
        //        lock (locker)
        //        {
        //            if (countdown.CurrentCount > 0)
        //            {
        //                Console.WriteLine(Thread.CurrentThread.Name + " can pick up " + selectedFormat + " document.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        lock (locker1)
        //        {
        //            if (countdown.CurrentCount > 0)
        //            {
        //                Console.WriteLine(Thread.CurrentThread.Name + " can pick up " + selectedFormat + " document.");
        //            }
        //        }
        //    }
        //    lock (locker2)
        //    {
        //        if (countdown.CurrentCount == 0)
        //        {
        //            countdown.Signal();
        //        }
        //    }
        //    Thread.Sleep(1000);         

            


        static void Main(string[] args)
        {
            WriteColors();
            
            Thread[] computers = new Thread[10];
            for (int i = 0; i < computers.Length; i++)
            {
                computers[i] = new Thread(Printing)
                {
                    //naming each thread
                    Name = String.Format("Computer_{0}", i + 1)
                };
                computers[i].Start();
            }

            //while (countdown.CurrentCount>0)
            //{
            //    for(int i=0;i<10;i++)
            //    {
            //        if(!computers[i].IsAlive)
            //        {
            //            string comp = computers[i].Name;
            //            computers[i] = new Thread(() => PrintRequest());
            //            computers[i].Name = comp;
            //            computers[i].Start();
            //        }
            //    }
            //}
            Console.ReadLine();

        }
    }
}
