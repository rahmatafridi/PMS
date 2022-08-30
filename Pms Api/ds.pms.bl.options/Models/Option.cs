using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.options.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }
        public int? SortOrder { get; set; }
        public int? HeaderId { get; set; }

        public static implicit operator Option(TblOption db)
        {
            if (db != null)
            {
                Option dl = new Option()
                {
                    Id = db.Id,
                    Title = db.Title,
                    Value= db.Value,
                    HeaderId = db.HeaderId,
                    SortOrder= db.SortOrder


                };
                return dl;
            }
            return null;
        }

        public static implicit operator TblOption(Option dl)
        {
            if (dl != null)
            {
                TblOption dbNote = new TblOption()
                {
                    Id = dl.Id,
                    Title = dl.Title,

                };
                return dbNote;
            }
            return null;
        }
    }
}
