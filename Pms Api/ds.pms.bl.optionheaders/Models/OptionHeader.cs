using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.optionheaders.Models
{
    public class OptionHeader
    {
       public int Id { get; set; }
       public int? ClientId { get; set; }
       public string Title { get; set; }
       public string ClientName { get; set; }
        public static implicit operator OptionHeader(TblOptionHeader db)
        {
            if (db != null)
            {
                OptionHeader dlNote = new OptionHeader()
                {
                    Id = db.Id,
                    Title = db.Title,
                    ClientId= db.ClientId

                };
                return dlNote;
            }
            return null;
        }

        public static implicit operator TblOptionHeader(OptionHeader dl)
        {
            if (dl != null)
            {
                TblOptionHeader dbNote = new TblOptionHeader()
                {
                    Id = dl.Id,
                    Title = dl.Title,
                    ClientId=dl.ClientId,
         
                };
                return dbNote;
            }
            return null;
        }
    }
}
