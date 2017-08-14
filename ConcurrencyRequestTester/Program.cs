using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyRequestTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number of concurrent request to send\n");            
            int NumOfRequest = Convert.ToInt32(Console.ReadLine());
            Console.Write("\n Press any key to continue to send {0} requests ", NumOfRequest);
            Console.ReadLine();
            RunConcurrentRequest(NumOfRequest);
            Console.Write("\n Press any key to continue to EXIT application ");
            Console.ReadLine();
        }

        
        private static void RunConcurrentRequest(int requests)
        {
            var tasks = new List<Task>();
            int count = 1;
            try
            {
                for (var index = 0; index < requests; index++)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        //var url = "http://10.65.120.120:1234/api/OS_2fAuthorisation_UpdValidateAuthCode?iSystemID=1&iCallingContext=89&sAccountID=shanmuga&sMobileNo=87654321&sAuthorisationCode=8888&iExpiryMinutes=20";
                        var url = "http://www.rediffmail.com/";
                        WebClient client = new WebClient();
                        client.DownloadString(url);
                        Console.WriteLine("Request {0} completed",count++);
                        
                    }));
                }
                Task.WaitAll(tasks.ToArray());
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception - {0}", e);
            }
            
        }
        
    }
}
