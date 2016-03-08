using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSyncApp_Console {
    class Semaphores : iLockingConstruct {
        
        private System.Object lockObject = new object();
        private const int numIterations = 1;
        private const int numBoats = 6;
        private string CurrentBoatInRoom = "";
        private int numDisappointedBoats = 0;
        private int numBoatsLevel1 = 0;
        private int maxBoatsLevel1 = 3;
        private SemaphoreSlim sem_slim; //only three boats could enter in Level 2

        private Public_Methods methods = new Public_Methods();


        public Semaphores(int MaxThreads) {
            maxBoatsLevel1 = MaxThreads;
        }


        public void Start_Locking() {

            Console.WriteLine("==============================Starting Horror House in SEMAPHORE MODE=============================================");

            //create instance for semaphore with max threads
            sem_slim = new SemaphoreSlim(maxBoatsLevel1);

            try {

                Console.WriteLine("Max number of boats to enter: {0}", maxBoatsLevel1);

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

            //threads cannot enter here if this is locked. (locked if max thread has reached.)
            sem_slim.Wait();

            for (var i = 0; i < numIterations; i++) {
                EnterHorrorBoatHouse();
            }

            sem_slim.Release();

        }


        private void EnterHorrorBoatHouse() {

            Console.WriteLine("{0} is entering Horror House. Current Boat #: {1} {2}", Thread.CurrentThread.Name, numBoatsLevel1, numBoatsLevel1 == maxBoatsLevel1 ? ", " + Thread.CurrentThread.Name + " is waiting in line." : "", Console.BackgroundColor = methods.GetThreadColor(Thread.CurrentThread.Name));

            numBoatsLevel1++;
                        
            if (numBoatsLevel1 <= maxBoatsLevel1) {
                
                methods.EnterLEVEL1(Thread.CurrentThread.Name);
                
                //Enter Level 2: only one boat can enter                
                lock (lockObject) {
                    CurrentBoatInRoom = Thread.CurrentThread.Name;

                    Console.WriteLine("Current # of Boats in: {1}", CurrentBoatInRoom, numBoatsLevel1, Console.BackgroundColor = methods.GetThreadColor(Thread.CurrentThread.Name));

                    methods.EnterLEVEL2(CurrentBoatInRoom);

                    numBoatsLevel1--;

                    Console.WriteLine("{0} is leaving horror house, they shat their pants... Current # of Boats in house: {1}.", CurrentBoatInRoom, numBoatsLevel1, Console.BackgroundColor = methods.GetThreadColor(Thread.CurrentThread.Name));

                }

            }
            else {
                //Expected: no event should pass here 
                Console.WriteLine("{0} cannot enter in room LEVEL 2 because room Level 2 is full. {0} is leaving horror house, disappointed...", Thread.CurrentThread.Name, Console.BackgroundColor = methods.GetThreadColor(Thread.CurrentThread.Name));

                numDisappointedBoats++;
            }

        }

        //hello
    }
}
