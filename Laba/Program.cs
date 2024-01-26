using Buses.Classes;
using Laba.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;

namespace Buses
{
    public class Program
    {
        public static async Task Main()
        {
            using (BusContext db = new BusContext())
            {
                FillWithMutex(db);
                await FillWithMonitorAsync(db);
                await ReadAsync(db);
            }
        }
        static void FillWithMutex(BusContext db)
        {
            Mutex mutex = new();
            int number = 0;

            ThreadStart generateStop = () =>
            {
                for (int i = 0; i < 20; i++)
                {
                    mutex.WaitOne();
                    db.BusStops.Add(new BusStop($"Stop{number}", $"Location{number}"));
                    number++;
                    mutex.ReleaseMutex();
                }
            };
            ThreadStart generatePassenger = () =>
            {
                for (int i = 0; i < 20; i++)
                {
                    mutex.WaitOne();
                    db.Passengers.Add(new Passenger($"Name{number}"));
                    number++;
                    mutex.ReleaseMutex();
                }
            };

            var thread1 = new Thread(generateStop);
            var thread2 = new Thread(generatePassenger);
            var thread3 = new Thread(generateStop);
            var thread4 = new Thread(generatePassenger);

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();

            bool allThreadsAreDone = false;
            while (!allThreadsAreDone)
            {
                allThreadsAreDone = thread1.ThreadState == ThreadState.Stopped && thread2.ThreadState == ThreadState.Stopped &&
                    thread3.ThreadState == ThreadState.Stopped && thread4.ThreadState == ThreadState.Stopped;
            }
            db.SaveChanges();
        }
        static async Task FillWithMonitorAsync(BusContext db)
        {
            var locker = new int[1];
            int number = 0;
            var tasks = new Task[4];

            Func<Task> generateStop = async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    int temp;
                    lock (locker)
                    {
                        temp = number;
                        number++;
                    }
                    await db.BusStops.AddAsync(new BusStop($"Stop{temp}", $"Location{temp}"));
                }
            };
            Func<Task> generatePassenger = async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    int temp;
                    lock (locker)
                    {
                        temp = number;
                        number++;
                    }
                    await db.Passengers.AddAsync(new Passenger($"Name{number}"));
                }
            };
            tasks[0] = generateStop();
            tasks[1] = generatePassenger();
            tasks[2] = generateStop();
            tasks[3] = generatePassenger();

            Task.WaitAll(tasks);

            await db.SaveChangesAsync();
        }
        static async Task ReadAsync(BusContext db)
        {
            var passengers = await db.Passengers.ToListAsync();
            foreach (var passenger in passengers)
            {
                Console.WriteLine(passenger);
            }
            var firstClient = await db.Passengers.FirstOrDefaultAsync();
            Console.WriteLine(firstClient);
        }
        static void FillContext()
        {
            using (BusContext db = new BusContext())
            {
                var passenger = new Passenger("Yana");
                var stop1 = new BusStop("Peremogy shkola st.", "Peremogy 5 st.");
                var stop2 = new BusStop("Polyana", "Borshagivska 128 st.");
                var stop3 = new BusStop("Arkadiya", "Vadyma Hetmana 24 st.");
                var stop4 = new BusStop("Arkadiya", "Arkadii 17 st.");
                List<BusStop> busStops = new List<BusStop>()
                {
                stop1, stop2, stop3
                };
                List<BusStop> busStops2 = new List<BusStop>()
                {
                stop1, stop3, stop4
                };
                List<BusStop> busStops3 = new List<BusStop>()
                {
                stop1, stop2, stop3, stop4
                };
                Bus bus1 = new Bus(25, 10);
                Bus bus2 = new Bus(50, 20);
                MiniBus miniBus = new MiniBus(10);
                Route route = new Route(busStops);
                Route route1 = new Route(busStops2);
                Route route2 = new Route(busStops3);
                Journey journey1 = new Journey(bus1, route, new DateTime(2023, 11, 10));
                passenger.BuyTicket(journey1, "Window");
                Journey journey2 = new Journey(bus2, route1, new DateTime(2023, 12, 16));
                passenger.BuyTicket(journey2, "Front");
                Journey journey3 = new Journey(miniBus, route2, new DateTime(2023, 12, 30));
                passenger.BuyTicket(journey3, "Back");
                db.Passengers.Add(passenger);
                db.SaveChanges();
            }
        }
    }
}
