using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class CityVersions
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string VersionNumber { get; set; }
        public string VersionCommentary { get; set; }
        public DateTime VersionedAt { get; set; }

        public virtual Cities City { get; set; }
    }
}
