using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessSynchronizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please provide a file path");
                return;
            }

            var path = args[0];
            var mutexName = "Global\\" + "ProcessSynchronizer";

            // create a global mutex
            using (var mutex = new Mutex(false, mutexName))
            {
                Console.WriteLine("Waiting for mutex");
                var mutexAcquired = false;
                try
                {
                    // acquire the mutex (or timeout after 60 seconds)
                    // will return false if it timed out
                    mutexAcquired = mutex.WaitOne(60000);
                }
                catch (AbandonedMutexException)
                {
                    // abandoned mutexes are still acquired, we just need
                    // to handle the exception and treat it as acquisition
                    mutexAcquired = true;
                }

                // if it wasn't acquired, it timed out, so can handle that how ever we want
                if (!mutexAcquired)
                {
                    Console.WriteLine("I have timed out acquiring the mutex and can handle that somehow");
                    return;
                }

                // otherwise, we've acquired the mutex and should do what we need to do,
                // then ensure that we always release the mutex
                try
                {
                    DoWork(path);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }

            }
        }

        static void DoWork(string path)
        {
            Console.WriteLine("Mutex acquired");
            string ConnectionString = "data source=SHANMUGAPC\\SQLEXPRESS;initial catalog=ShanDB;Integrated Security=true;";
            ExecuteStoredProc(ConnectionString);
            /*
            

            // delay for a little so we can see some blocking
            Thread.Sleep(15000);

            // create the specified directory if it doesn't exist
            Console.WriteLine("Creating directory " + path);
            var directory = new DirectoryInfo(path);
            if (!directory.Exists)
                directory.Create();

            // write a file to it
            var filePath = Path.Combine(path, DateTime.UtcNow.Ticks.ToString());
            Console.WriteLine("Creating file " + filePath);
            var file = new FileInfo(filePath);
            file.Create();
            */
        }

        static void ExecuteStoredProc(string ConnectionString)
        {
            using (SqlConnection ConnectionObject = new SqlConnection(ConnectionString))
            {
                using (SqlCommand Sqlcommand = new SqlCommand("SamplePrinter", ConnectionObject))
                {
                    Sqlcommand.CommandType = CommandType.StoredProcedure;

                   // Sqlcommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = txtFirstName.Text;
                  //  Sqlcommand.Parameters.Add("@LastName", SqlDbType.VarChar).Value = txtLastName.Text;

                    ConnectionObject.Open();
                    Sqlcommand.ExecuteNonQuery();
                }
            }
        }

    }
}

/*
 * 
 * 
 * 
 * 
 * 
Declare @Object as Int;
Declare @ResponseText as Varchar(8000);
 
Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;
Exec sp_OAMethod @Object, 'open', NULL, 'get',
                                                              'http://www.webservicex.com/stockquote.asmx/GetQuote?symbol=MSFT', --Your Web Service Url (invoked)
                                                              'false'
Exec sp_OAMethod @Object, 'send'
Exec sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT
 
Select @ResponseText
 
Exec sp_OADestroy @Object

EXEC sp_configure 'Ole Automation Procedures';  
GO  

sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
sp_configure 'Ole Automation Procedures', 1;
GO
RECONFIGURE;
GO
*/
