using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.notes.Models
{
    public class NoteCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Note Note { get; set; }
    }
}
