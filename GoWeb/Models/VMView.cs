using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace GoWeb.Models
{
    public class VMView
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get;}
    }
}
