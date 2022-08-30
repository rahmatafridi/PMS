using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.notescategory.Models
{
    public class NotesCategory
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public int ParentId { get; set; } // int
        public int TypeId { get; set; } // int
        public string Name { get; set; } // varchar(100)

        public static implicit operator NotesCategory(TblNotesCategory dbNotesCategory)
        {
            if (dbNotesCategory != null)
            {
                NotesCategory dlNotesCategory = new NotesCategory()
                {
                    Id = dbNotesCategory.Id,
                    ClientId = dbNotesCategory.ClientId,
                    ParentId = dbNotesCategory.ParentId,
                    TypeId = dbNotesCategory.TypeId,
                    Name = dbNotesCategory.Name,
                };
                return dlNotesCategory;
            }
            return null;
        }

        public static implicit operator TblNotesCategory(NotesCategory dlNotesCategory)
        {
            if (dlNotesCategory != null)
            {
                TblNotesCategory dbNotesCategory = new TblNotesCategory()
                {
                    Id = dlNotesCategory.Id,
                    ClientId = dlNotesCategory.ClientId,
                    ParentId = dlNotesCategory.ParentId,
                    TypeId = dlNotesCategory.TypeId,
                    Name = dlNotesCategory.Name,
                };
                return dbNotesCategory;
            }
            return null;
        }
    }
}
