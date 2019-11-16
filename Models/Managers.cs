using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Managers
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Cities City { get; set; }
    }
}
