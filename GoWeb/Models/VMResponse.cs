using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace GoWeb.Models
{
    public class VMResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public object data { get; set; }

    }
}
