using SQ.Common.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library.Handlers
{
    public class ThreadHandler
    {
        public static int MAX_SOCKET_THREAD = 1000;

        public static int INIT_SOCKET_THREAD_COUNT = 3; //make it 20 in production.

        public static int MAX_PUBLISHER_THREAD = 1;

        public static Queue<Thread> SocketWorkerPool = new Queue<Thread> ();

        public static Queue<Thread> PublishThreadPool = new Queue<Thread> ();

        public static int activeCount;

        private readonly static object SocketWorkerPool_lock = new object();

        public static Queue<Action> PendingTask = new Queue<Action>();

        private readonly static object PendingTask_lock = new object();
        private ThreadHandler() { }

        public static void PublishThreadStart()
        {
            Thread publisher = new Thread(MessageHandler.ProcessMessage);
            PublishThreadPool.Enqueue(publisher);
            publisher.Start();
        }

        public static void Publish()
        {
            if(PublishThreadPool.Count == MAX_PUBLISHER_THREAD)
            {
                return;
            }
            PublishThreadStart();   
        }

        public static void SocketPoolInit()
        {
            if(SocketWorkerPool.Count == 0)
            {
                //make init thread and put them in thread.
                for(int i = 0;i< INIT_SOCKET_THREAD_COUNT; i++)
                {
                    Thread socketThread = new Thread(Work);//define the work function for it to use.
                    socketThread.Start();
                    SocketWorkerPool.Enqueue(socketThread);
                }
            }
        }

        public static int MAX_PENDING = 1; //that is if 21 task occur create a new thread and make that thread take up that task
      
        
        //todo: Implement later
        /*  public static void MonitorThePool() //decide when to call. maybe during enque a task
        {
            lock (SocketWorkerPool_lock)
            {
                lock (PendingTask_lock)
                {
                    if (SocketWorkerPool.Count == 20 && PendingTask.Count > 0)
                    {
                        Thread socketThread = new Thread(Work);
                        SocketWorkerPool.Enqueue(socketThread);
                        socketThread.Start();
                    }
                }
            }
        }*/

        private static  void Work()
        {
            while (true)
            {
                Action Task = null;
                
                while(Task == null) 
                {
                    lock(PendingTask_lock) 
                    {
                        if(PendingTask.Count != 0)
                        {
                            Task = PendingTask.Dequeue();
                        }
                    }

                    if(Task != null)
                    {
                        Thread.Sleep(10000); //10s waiting Period
                    }
                }

                Task();
                //get a socket
                //listen to coonection
                //pass msg from file to listeners
            }
        }

        public static void AddWork(Action task)
        {
            lock (PendingTask_lock)
            {
                PendingTask.Enqueue(task);
            }
        }

    }
}
