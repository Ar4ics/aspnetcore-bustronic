﻿using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class PassengerSmsCodes
    {
        public string PhoneNo { get; set; }
        public string SmsCode { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
