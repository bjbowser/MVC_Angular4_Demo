using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker
{
    public class Issue
    {
        public int IssueID { get; set; }
        public DateTime DateAdded { get; set; }
        public Boolean Active { get; set; }
        public IssueType Type { get; set; }
        public int IssueTypeID { get; set; }
        public string IssueText { get; set; }
        public string TextRecieved { get; set; }
        public string TextWanted { get; set; }
        public string CloseDesc { get; set; }
    }
}
