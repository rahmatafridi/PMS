using ds.pms.dal.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;

namespace ds.pms.bl.documents.Models
{
    public class Document
    {
        
        //public IFormFile File { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string MimeType { get; set; }
        public string DocObject { get; set; }
        public byte[] DocObjectArr { get; set; }

        public long Size { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? ParentId { get; set; }
        public int? TypeId { get; set; }
        public int? ClinetId { get; set; }

        public static implicit operator Document(TblDocument dbDocument)
        {
            if (dbDocument != null)
            {
                Document dlDocument = new Document()
                {
                    Id = dbDocument.Id,
                    CategoryId = dbDocument.CategoryId,
                    Name = dbDocument.Name,
                    Extension = dbDocument.Extension,
                    MimeType = dbDocument.MimeType,
                    DocObject = dbDocument.DocObject,
                    //DocObjectArr = dbDocument.DocObject,
                    Size = dbDocument.Size,
                    ClinetId = dbDocument.ClinetId,
                    ParentId= dbDocument.ParentId,
                    TypeId=dbDocument.TypeId,
                    AddedBy= dbDocument.AddedBy,
                    AddedDate= dbDocument.AddedDate
                 
                };
                return dlDocument;
            }
            return null;
        }

        public static implicit operator TblDocument(Document dlDocument)
        {
            if (dlDocument != null)
            {
                TblDocument dbDocument = new TblDocument()
                {
                    Id = dlDocument.Id,
                    CategoryId = dlDocument.CategoryId,
                    Name = dlDocument.Name,
                    Extension = dlDocument.Extension,
                    MimeType = dlDocument.MimeType,
                    DocObject = dlDocument.DocObject,
                    //DocObjectArr = dbDocument.DocObject,
                    ClinetId = dlDocument.ClinetId,
                    ParentId = dlDocument.ParentId,
                    TypeId = dlDocument.TypeId,
                    Size = dlDocument.Size,
                    AddedBy= dlDocument.AddedBy,
                    AddedDate= dlDocument.AddedDate
                };
                return dbDocument;
            }
            return null;
        }
    }
}
