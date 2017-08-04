using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace ThreadSafeDBService.Controllers
{
    /// <summary>
    /// Description for ThreadSafeOutProcessController.
    /// </summary>
    public class OS_2fAuthorisation_InsAuthCodeController : ApiController
    {
        static protected string MutexName = "Global\\" + "ThreadSafeDBService" + "ALL";
        static protected string ProcName = "OS_2fAuthorisation_InsAuthCode_ThreadSafe";
        static protected string ConnectionString = "data source=MSLDEVSQL03;initial catalog=MMSGCommon;Integrated Security=true;";

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Post(int iSystemID, int iCallingContext, string sAccountID, string sMobileNo, string sAuthorisationCode, int iExpiryMinutes)
        {
            IHttpActionResult Result = Ok("Success");
            Console.WriteLine("SyncAllController - START({0})", Thread.CurrentThread.ManagedThreadId);
           
            try
            {
               lock(Synchronizer.sharedLockObject)
                {
                    using (SqlConnection ConnectionObject = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand Sqlcommand = new SqlCommand(ProcName, ConnectionObject))
                        {
                            Sqlcommand.CommandType = CommandType.StoredProcedure;
                            Sqlcommand.Parameters.Add("@iSystemID", SqlDbType.Int).Value = iSystemID;
                            Sqlcommand.Parameters.Add("@iCallingContext", SqlDbType.Int).Value = iCallingContext;
                            Sqlcommand.Parameters.Add("@sAccountID", SqlDbType.VarChar, 255).Value = sAccountID;
                            Sqlcommand.Parameters.Add("@sMobileNo", SqlDbType.VarChar, 20).Value = sMobileNo;
                            Sqlcommand.Parameters.Add("@sAuthorisationCode", SqlDbType.VarChar, 128).Value = sAuthorisationCode;
                            Sqlcommand.Parameters.Add("@iExpiryMinutes", SqlDbType.Int).Value = iExpiryMinutes;
                            ConnectionObject.Open();
                            Sqlcommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("SyncAllController - CATCH START({0})", Thread.CurrentThread.ManagedThreadId);
                Result = InternalServerError(e);
                Console.WriteLine("SyncAllController - CATCH END({0})", Thread.CurrentThread.ManagedThreadId);
            }
            Console.WriteLine("SyncAllController - END({0})", Thread.CurrentThread.ManagedThreadId);
            return Result;
        }

        /*
        public IHttpActionResult Post(int iSystemID, int iCallingContext, string sAccountID, string sMobileNo, string sAuthorisationCode, int iExpiryMinutes)
        {
            IHttpActionResult Result = Ok("Success");
            Console.WriteLine("SyncAllController - START({0})", Thread.CurrentThread.ManagedThreadId);

            try
            {
                Console.WriteLine("SyncAllController - TRY START({0})", Thread.CurrentThread.ManagedThreadId);

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
                        Result = InternalServerError(new Exception("Not able to acquire Mutex"));
                    }
                    // otherwise, we've acquired the mutex and should do what we need to do,
                    // then ensure that we always release the mutex
                    try
                    {
                        using (SqlConnection ConnectionObject = new SqlConnection(ConnectionString))
                        {
                            using (SqlCommand Sqlcommand = new SqlCommand(ProcName, ConnectionObject))
                            {
                                Sqlcommand.CommandType = CommandType.StoredProcedure;
                                Sqlcommand.Parameters.Add("@iSystemID", SqlDbType.Int).Value = iSystemID;
                                Sqlcommand.Parameters.Add("@iCallingContext", SqlDbType.Int).Value = iCallingContext;
                                Sqlcommand.Parameters.Add("@sAccountID", SqlDbType.VarChar, 255).Value = sAccountID;
                                Sqlcommand.Parameters.Add("@sMobileNo", SqlDbType.VarChar, 20).Value = sMobileNo;
                                Sqlcommand.Parameters.Add("@sAuthorisationCode", SqlDbType.VarChar, 128).Value = sAuthorisationCode;
                                Sqlcommand.Parameters.Add("@iExpiryMinutes", SqlDbType.Int).Value = iExpiryMinutes;
                                ConnectionObject.Open();
                                Sqlcommand.ExecuteNonQuery();
                            }
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
                Console.WriteLine("SyncAllController - TRY END({0})", Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception e)
            {
                Console.WriteLine("SyncAllController - CATCH START({0})", Thread.CurrentThread.ManagedThreadId);
                Result = InternalServerError(e);
                Console.WriteLine("SyncAllController - CATCH END({0})", Thread.CurrentThread.ManagedThreadId);
            }
            Console.WriteLine("SyncAllController - END({0})", Thread.CurrentThread.ManagedThreadId);
            return Result;
        }
        */

        public IHttpActionResult Get(int iSystemID, int iCallingContext, string sAccountID, string sMobileNo, string sAuthorisationCode, int iExpiryMinutes)
        {
            Console.WriteLine("SyncAllController - START({0})", Thread.CurrentThread.ManagedThreadId);
            IHttpActionResult Result = Ok("Success - INSERT");
            try
            {
                Console.WriteLine("SyncAllController - TRY START({0})", Thread.CurrentThread.ManagedThreadId);               
                lock (Synchronizer.sharedLockObject)
                {
                    Thread.Sleep(5000);
                }
                Console.WriteLine("SyncAllController - TRY END({0})", Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception e)
            {
                Console.WriteLine("SyncAllController - CATCH START({0})", Thread.CurrentThread.ManagedThreadId);
                Result = InternalServerError(e);
                Console.WriteLine("SyncAllController - CATCH END({0})", Thread.CurrentThread.ManagedThreadId);
            }

            Console.WriteLine("SyncAllController - END({0})", Thread.CurrentThread.ManagedThreadId);
            return Result;
        }
    }
}