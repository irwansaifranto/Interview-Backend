using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moduit.Domain.Objects
{
    public class ResourceProfile
    {
        public string BaseUrl { get; set; }
        public int CallAPIPageSize { get; set; }
        public int RefreshApiExpirationTime { get; set; }
    }
}
