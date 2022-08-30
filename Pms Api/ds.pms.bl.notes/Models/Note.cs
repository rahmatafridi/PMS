using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.notes.Models
{
    public class Note
    {
		public int Id { get; set; } // int
		public int NoteCategoryId { get; set; } // int
		public int ParentId { get; set; } // int
		public int TypeId { get; set; } // int
		public string Notes { get; set; } // varchar(max)
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }

        public static implicit operator Note(TblNote dbNote)
        {
            if (dbNote != null)
            {
                Note dlNote = new Note()
                {
                    Id = dbNote.Id,
                    NoteCategoryId = dbNote.NoteCategoryId,
                    ParentId = dbNote.ParentId,
                    TypeId = dbNote.TypeId,
                    Notes = dbNote.Note,
                    AddedDate= (DateTime)dbNote.AddedDate,
                    AddedBy= dbNote.AddedBy
                    
                };
                return dlNote;
            }
            return null;
        }

        public static implicit operator TblNote(Note dlNote)
        {
            if (dlNote != null)
            {
                TblNote dbNote = new TblNote()
                {
                    Id = dlNote.Id,
                    NoteCategoryId = dlNote.NoteCategoryId,
                    ParentId = dlNote.ParentId,
                    TypeId = dlNote.TypeId,
                    Note = dlNote.Notes,
                    AddedDate = (DateTime)dlNote.AddedDate,
                    AddedBy = dlNote.AddedBy
                };
                return dbNote;
            }
            return null;
        }
    }
}
