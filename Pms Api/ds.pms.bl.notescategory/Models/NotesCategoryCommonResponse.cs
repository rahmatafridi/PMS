using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.notescategory.Models
{
    public class NotesCategoryCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public NotesCategory NotesCategory  { get; set; }
    }
}
