using System;
using Microsoft.Owin.Hosting;


namespace ThreadSafeDBService
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             * RUN IN ADMINISTRATOR MODE 
             * 
             * COMMAND TO VIEW ALL RESERVED IP ADDRESS
             * netstat - a - b
             * 
             * COMMAND TO RESERVE IP ADDRESS
             * netsh http add urlacl url = http://10.65.120.120:1234/ user=shanmugasun
             * 
            */

            StartOptions options = new StartOptions();
           // options.Urls.Add("http://localhost:1234");
            options.Urls.Add("http://10.65.120.120:1234");
            options.Urls.Add("http://127.0.0.1:1234");
            //  options.Urls.Add(string.Format("http://{0}:1234", Environment.MachineName));

            //const string serverHostAddress = "http://localhost:1234";

            using (WebApp.Start<Startup>(options))
            {
                Console.WriteLine("Light Weight Self hosted Server( ThreadSafeDBService) - Web API running locally " );
                while (true) ;
            }
        }
    }
}
