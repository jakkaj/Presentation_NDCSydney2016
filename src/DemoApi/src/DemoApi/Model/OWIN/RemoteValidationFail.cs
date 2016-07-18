using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApi.Model.OWIN
{
    public class RemoteValidationFail : Exception
    {
        public string FailReason { get; set; }

        public RemoteValidationFail(string failReason) : base(failReason)
        {
            FailReason = failReason;
            
        }
    }
}
