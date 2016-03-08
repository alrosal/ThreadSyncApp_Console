using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSyncApp_Console {

    class Public_Methods {


        public void EnterLEVEL2(string CurrentBoatInRoom) {
            
            Console.WriteLine("{0} entered in room LEVEL 2.", CurrentBoatInRoom, Console.BackgroundColor = GetThreadColor(CurrentBoatInRoom));

            Console.WriteLine("{0} is very scared in room LEVEL 2.", CurrentBoatInRoom, Console.BackgroundColor = GetThreadColor(CurrentBoatInRoom));
            
            Thread.Sleep(500);

            Console.WriteLine("{0} is leaving room LEVEL 2.", CurrentBoatInRoom, Console.BackgroundColor = GetThreadColor(CurrentBoatInRoom));

        }

        public void EnterLEVEL1(string CurrentBoatInRoom) {

            Console.WriteLine("{0} entered Horror House.", CurrentBoatInRoom, Console.BackgroundColor = GetThreadColor(CurrentBoatInRoom));


            Console.WriteLine("{0} is very scared in Horror House.", CurrentBoatInRoom, Console.BackgroundColor = GetThreadColor(CurrentBoatInRoom));

            Thread.Sleep(500);
            
        }


        public void WriteLog(string CurrentBoatInRoom) {

            StreamWriter sw = null;

            try {
                sw = new StreamWriter(@"D:\Logs\log-file.txt", true);

                sw.Write("APP 1 [{1}] : {0} entered in room LEVEL 2. \n", CurrentBoatInRoom, DateTime.Now);

                sw.Close();

            }
            catch (IOException ex) {
                Console.WriteLine("Error: {0}", ex);
                //sw.Close();
            }

        }


        public ConsoleColor GetThreadColor(string thread) {

            thread = thread.Replace("Boat_", "");

            return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), thread);

        }
    }
}
