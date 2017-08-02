﻿using System;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace ThreadSafeDBService.Controllers
{
    public abstract class SyncBaseController : ApiController
    {
        protected string ConnectionString;
        protected string MutexName;
        protected string ProcName;
        public bool MakeThreadSafe()
        {
            // create a global mutex
            using (var mutex = new Mutex(false, MutexName))
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
                    return false;
                }

                // otherwise, we've acquired the mutex and should do what we need to do,
                // then ensure that we always release the mutex
                try
                {
                    ExecuteStoredProc();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }

                return true;
            }
        }


        public void ExecuteStoredProc()
        {
            using (SqlConnection ConnectionObject = new SqlConnection(ConnectionString))
            {
                using (SqlCommand Sqlcommand = new SqlCommand(ProcName, ConnectionObject))
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