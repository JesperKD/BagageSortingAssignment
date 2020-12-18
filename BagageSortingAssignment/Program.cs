using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BagageSortingAssignment
{
    class BagageSorting
    {
        Queue<Bagage> newBagageQueue = new Queue<Bagage>();
        Queue<Bagage> terminal1Queue = new Queue<Bagage>();
        Queue<Bagage> terminal2Queue = new Queue<Bagage>();
        Queue<Bagage> terminal3Queue = new Queue<Bagage>();

        public void CreateBagage()
        {
            while (Thread.CurrentThread.IsAlive)
            {

                if (newBagageQueue.Count == 0)
                {
                    lock (newBagageQueue)
                    {
                        if (newBagageQueue.Count != 0)
                        {
                            Monitor.PulseAll(newBagageQueue);
                            Monitor.Wait(newBagageQueue);
                        }
                        else if (newBagageQueue.Count == 0)
                        {
                            while (newBagageQueue.Count < 9)
                            {
                                Bagage englandBagage = new Bagage { Destination = "England" };
                                Bagage barcelonaBagage = new Bagage { Destination = "Barcelona" };
                                Bagage usaBagage = new Bagage { Destination = "USA" };
                                Console.WriteLine("Incoming bagage for " + englandBagage.Destination);
                                Console.WriteLine("Incoming bagage for " + barcelonaBagage.Destination);
                                Console.WriteLine("Incoming bagage for " + usaBagage.Destination);
                                newBagageQueue.Enqueue(englandBagage);
                                newBagageQueue.Enqueue(barcelonaBagage);
                                newBagageQueue.Enqueue(usaBagage);
                            }
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }


        public void SplitBagage()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                lock (newBagageQueue)
                {
                    while (newBagageQueue.Count == 0)
                    {
                        Console.WriteLine("Splitter is waiting");
                        Monitor.PulseAll(newBagageQueue);
                        Monitor.Wait(newBagageQueue);
                    }
                    while (newBagageQueue.Count != 0)
                    {
                        if (newBagageQueue.Peek().Destination == "England")
                        {
                            Console.WriteLine("Routing bagage to Terminal 1.");
                            terminal1Queue.Enqueue(newBagageQueue.Dequeue());
                        }
                        if (newBagageQueue.Peek().Destination == "Barcelona")
                        {
                            Console.WriteLine("Routing bagage to Terminal 2");
                            terminal2Queue.Enqueue(newBagageQueue.Dequeue());
                        }
                        if(newBagageQueue.Peek().Destination == "USA")
                        {
                            Console.WriteLine("Routing bagage to Terminal 3");
                            terminal3Queue.Enqueue(newBagageQueue.Dequeue());
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
        }


        public void Terminal1()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                lock (terminal1Queue)
                {
                    while (terminal1Queue.Count == 0)
                    {
                        Console.WriteLine("Terminal 1 is waiting");
                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("Terminal 1 sends off bagage for: " + terminal1Queue.Peek().Destination);
                    terminal1Queue.Dequeue();
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }
        }


        public void Terminal2()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                lock (terminal2Queue)
                {
                    while (terminal2Queue.Count == 0)
                    {
                        Console.WriteLine("Terminal 2 is waiting");
                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("Terminal 2 sends oof bagage for: " + terminal2Queue.Peek().Destination);
                    terminal2Queue.Dequeue();
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }
        }
        public void Terminal3()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                lock (terminal3Queue)
                {
                    while (terminal3Queue.Count == 0)
                    {
                        Console.WriteLine("Terminal 3 is waiting");
                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("Terminal3 sends off bagage for: " + terminal3Queue.Peek().Destination);
                    terminal3Queue.Dequeue();
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            BagageSorting bs = new BagageSorting();

            Thread incoming = new Thread(bs.CreateBagage);
            Thread splitter = new Thread(bs.SplitBagage);
            Thread terminal1 = new Thread(bs.Terminal1);
            Thread terminal2 = new Thread(bs.Terminal2);
            Thread terminal3 = new Thread(bs.Terminal3);

            incoming.Start();
            splitter.Start();
            terminal2.Start();
            terminal1.Start();
            terminal3.Start();

            Console.Read();
        }
    }


    public class Bagage
    {
        public string Destination { get; set; }

    }
}
