using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Zadatak_1
{
    class Program
    {
        /// <summary>
        /// File with paper colors
        /// </summary>
        public static string colorsFile = @"../../Paleta.txt";
        /// <summary>
        /// List of paper colors
        /// </summary>
        public static List<string> colors = new List<string>() { "red", "blue", "white", "yellow", "green", "orange", "violet" };
        /// <summary>
        /// Array of paper formats
        /// </summary>        
        public static string[] format = new string[] { "A3", "A4" };
        /// <summary>
        /// array with paper orientation
        /// </summary>
        public static string[] orientation = new string[] { "portrait", "landscape" };
        /// <summary>
        /// Computer names list
        /// </summary>
        public static List<string> comps = new List<string>();

        /// <summary>
        /// object for random selection
        /// </summary>
        public static Random rnd = new Random();
        
        //objects for locking printers
        static object locker1 = new object();
        static object locker2 = new object();
                
        static AutoResetEvent event1 = new AutoResetEvent(true);
        static AutoResetEvent event2 = new AutoResetEvent(true);        

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
            string selectedFormat = (format[rnd.Next(format.Length)]);            
            return selectedFormat;
        }

        /// <summary>
        /// Method for random selection of paper color from file
        /// </summary>
        /// <returns>string value for paper color</returns>
        public static string SelectPaperColor()
        {
            string[] allLines = File.ReadAllLines(colorsFile);            
            string selectedColor = allLines[rnd.Next(allLines.Length)];            
            return selectedColor;
        }

        /// <summary>
        /// Method for random selection of paper orientation
        /// </summary>
        /// <returns>paper orientation</returns>
        public static string SelectOrientation()
        {            
            string selectedOrientation = (orientation[rnd.Next(orientation.Length)]);            
            return selectedOrientation;
        }

        /// <summary>
        /// Method for A3 printer which prints A3 format documents
        /// </summary>
        public static void A3Printing()
        {
            //printer is locked so that only one by one computer can print document on it
            lock (locker1)
            {                
                Console.WriteLine("Document for " + Thread.CurrentThread.Name + " is printing...");
                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + " can pick up A3 document.");                
            }

        }

        /// <summary>
        /// Method for A4 printer which prints A4 format documents
        /// </summary>
        public static void A4Printing()
        {
            //printer is locked so that only one by one computer can print document on it
            lock (locker2)
            {                
                Console.WriteLine("Document for " + Thread.CurrentThread.Name + " is printing...");
                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + " can pick up A4 document.");                
            }            
        }       

        /// <summary>
        /// Method for simulating printing
        /// </summary>
        public static void Printing()
        {
            while(comps.Count<10)
            {
                var computer = Thread.CurrentThread.Name;
                //selecting random paper format
                string selectedFormat = SelectPaperFormat();
                //selecting random paper color from file
                string selectedColor = SelectPaperColor();
                //select random orientation
                string selectedOrientation = SelectOrientation();

                //message that print request is sent
                Console.WriteLine(computer + " sent request for printing " + selectedFormat + " format document. Color: " +
                                   selectedColor + ". Orientation: " + selectedOrientation);

                //if selected format is A3, then send print request to A3 printer by calling method A3Printing
                if (selectedFormat == "A3")
                {
                    event1.WaitOne();                    
                    if (comps.Count==10)
                    {
                        return;
                        
                    }
                    A3Printing();
                    event1.Set();
                }
                //if selected format is A4, then send print request to A4 printer by calling method A4Printing
                else
                {                   
                    event2.WaitOne();  
                    if (comps.Count == 10)
                    {
                        return;
                    }
                    A4Printing();
                    event2.Set();
                }
                //add computer that printed if not already added in list of computers
                if(!comps.Contains(Thread.CurrentThread.Name))
                {
                    comps.Add(Thread.CurrentThread.Name);
                }
            }                     
            
        }
                
        static void Main(string[] args)
        {
            //first starting thread for writing paper colors in file
            Thread writeColors = new Thread (()=> WriteColors())
            {
                Name="write_colors"
            };
            writeColors.Start();            
            writeColors.Join();            
            
            //creating and starting computer threads
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(Printing)
                {
                    //naming each thread
                    Name = String.Format("Computer_{0}", i + 1)
                };                
                thread.Start();                
            }
            
            Console.ReadKey();

        }
    }
}
