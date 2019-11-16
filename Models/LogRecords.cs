using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class LogRecords
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserTitle { get; set; }
        public string MethodTitle { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
