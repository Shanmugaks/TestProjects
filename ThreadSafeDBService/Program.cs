using System;
using Microsoft.Owin.Hosting;


namespace ThreadSafeDBService
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serverHostAddress = "http://localhost:1234/";

            using (WebApp.Start<Startup>(url: serverHostAddress))
            {
                Console.WriteLine("Light Weight Self hosted Server( ThreadSafeDBService) - Web API running  @ " + serverHostAddress);
                while (true) ;
            }
        }
    }
}
