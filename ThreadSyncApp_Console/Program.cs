using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ThreadSyncApp_Console {
    class Program {
        

        static void Main(string[] args) {


            Console.WriteLine(@"
                               HORROR HOUSE CONSOLE APP.
                                - MUTEX and LOCK : Only ONE BOAT can enter in Horror House.
                                - SEMAPHORES: User Based-Input on MAX Boats to enter in Horror House. 
                               ");



            iLockingConstruct locking_object = null;
            string choice = "";

            do {
                Console.WriteLine("Choose Locking Construct:");
                Console.WriteLine("A: Mutex Locking");
                Console.WriteLine("B: Lock Statement");
                Console.WriteLine("C: Semaphores");
                Console.WriteLine("D: No Thread Locking");
                Console.WriteLine("E: ReaderWriter Lock");

                choice = Console.ReadLine();

                switch (choice.ToUpper()) {

                    case "A":
                    Mutual_Exclusion mut = new Mutual_Exclusion();

                    locking_object = mut;
                    break;

                    case "B":
                    Lock lock_object = new Lock();

                    locking_object = lock_object;
                    break;

                    case "C":

                    Console.WriteLine("Please enter max threads: ");
                    var maxthread = Console.ReadLine();
                    int maxthread_int = 0;
                                        
                    if(int.TryParse(maxthread, out maxthread_int)) {

                        Semaphores sem_object = new Semaphores(maxthread_int);

                        locking_object = sem_object;

                    } else {
                        Console.WriteLine("Invalid Input.");
                        choice = "";
                    }

                    break;

                    case "D":
                    No_Lock no_lock_object = new No_Lock();

                    locking_object = no_lock_object;
                    break;

                    case "E":
                    ReaderWriter_Lock rw_lock_object = new ReaderWriter_Lock();

                    locking_object = rw_lock_object;
                    break;

                    default:
                    Console.WriteLine("Please select from the menu.");
                    choice = "";
                    break;

                }
            } while (choice == "");


            if(locking_object != null)
                locking_object.Start_Locking();
            

        }
        
        

    }
}
