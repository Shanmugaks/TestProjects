using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Cors;
using System.Web.Http;


namespace ThreadSafeDBService.Controllers
{
    /// <summary>
    /// Description for ThreadSafeInProcessController.
    /// </summary>
    public class SyncThisController : SyncBaseController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Get(string StoredProcName)
        {
            IHttpActionResult Result = null;
            try
            {
                Console.WriteLine("SyncThisController - Enter");
                MutexName = "Global\\" + "ThreadSafeDBService" + StoredProcName;
                ProcName = StoredProcName;
                ConnectionString = "data source=SHANMUGAPC\\SQLEXPRESS;initial catalog=ShanDB;Integrated Security=true;";
                MakeThreadSafe();
                Console.WriteLine("SyncThisController - Exit");
            }
            catch (Exception e)
            {
                Result = InternalServerError(e);
            }

            return Result;
        }
    }
}