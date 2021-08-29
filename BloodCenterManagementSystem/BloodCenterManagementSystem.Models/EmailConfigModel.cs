using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Models
{
    public class EmailConfigModel
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
    }
}
