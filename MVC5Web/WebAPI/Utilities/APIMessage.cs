using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Utilites
{
    public struct APIMessage
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object Data { get; set; }
    }
}