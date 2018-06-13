using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker
{
    public class IssueType
    {
        public int IssueTypeID { get; set; }
        public string IssueName { get; set; }
        public string IssueDescription { get; set; }

        public static IssueType Convert(string v)
        {
            IssueType it = new IssueType();
            it.IssueDescription = v;

            return it;
        }
    }
}
