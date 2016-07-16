using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApi.Model.Contract;

namespace DemoApi.Model.Service
{
    public class SampleService : ISampleService
    {
        public string GetSomeValue()
        {
            return "This is some value and that got 44444!";
        }
    }
}
