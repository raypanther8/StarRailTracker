using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarRailTracker.Service
{
    static internal class GlobalClient
    {
        public static readonly HttpClient Client = new();
    }
}
