using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSyncApp_Console {
    public class Mutual_Exclusion : iLockingConstruct {
        
        private Mutex mut = new Mutex(false, "Test");
        private const int numIterations = 1;
        private const int numBoats = 5;
        private string CurrentBoatInRoom = "";
        private int numDisappointedBoats = 0;

        private Public_Methods methods = new Public_Methods();


        public void Start_Locking() {

            Console.WriteLine("==============================Starting Horror House in MUTEX MODE=============================================");


            try {

                //start with a series of threads running
                Thread[] threads = new Thread[numBoats];

                for (int i = 0; i < numBoats; i++) {

                    threads[i] = new Thread(new ThreadStart(ThreadProcess));
                    threads[i].Name = string.Format("Boat_{0}", i + 1);
                    threads[i].Start();

                }


                //tell the threads to shutdown and wait until finished
                for (int i = 0; i < numBoats; i++) {
                    threads[i].Join();
                }

                Console.WriteLine("Number of disappointed boats: {0}", numDisappointedBoats, Console.BackgroundColor = ConsoleColor.Red);
                Console.ReadLine();

            }
            catch (Exception ex) {
                Console.WriteLine("Error: {0}", ex.Message);
                Thread.CurrentThread.Abort();
                //throw new Exception(ex.Message);
            }
        }


        private void ThreadProcess() {

            //threads cannot enter here if this is locked. (current thread is not yet finished processing.)
            if (mut.WaitOne()) {
                for (var i = 0; i < numIterations; i++) {
                    EnterHorrorBoatHouse();
                }

                mut.ReleaseMutex();
            }
            else {
                Console.WriteLine("Another Process is running: {0} is waiting in line....", Thread.CurrentThread.Name, Console.BackgroundColor = methods.GetThreadColor(Thread.CurrentThread.Name));
            }

        }


        private void EnterHorrorBoatHouse() {

            Console.WriteLine("{0} is entering Horror House. Current Boat: {1} {2}", Thread.CurrentThread.Name, CurrentBoatInRoom, CurrentBoatInRoom != "" ? ", " + Thread.CurrentThread.Name + " is waiting in line." : "none", Console.BackgroundColor = methods.GetThreadColor(Thread.CurrentThread.Name));
            
            if (CurrentBoatInRoom == "") {

                CurrentBoatInRoom = Thread.CurrentThread.Name;

                methods.EnterLEVEL1(CurrentBoatInRoom);

                methods.WriteLog(CurrentBoatInRoom);

                CurrentBoatInRoom = "";

                Console.WriteLine("{0} is leaving horror house, they shat their pants...", Thread.CurrentThread.Name, Console.BackgroundColor = methods.GetThreadColor(Thread.CurrentThread.Name));
            }
            else {
                //Expected: no event should pass here 
                Console.WriteLine("{0} cannot enter in room LEVEL 2 because another boat is already in. Current boat in: {1}. {0} is leaving horror house, disappointed...", Thread.CurrentThread.Name, CurrentBoatInRoom, Console.BackgroundColor = methods.GetThreadColor(Thread.CurrentThread.Name));

                numDisappointedBoats++;

            }




        }



    }
}
