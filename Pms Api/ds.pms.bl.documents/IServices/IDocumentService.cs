using ds.pms.apicommon.Models;
using ds.pms.bl.documents.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.documents.IServices
{
    public interface IDocumentService
    {
        PaginatedList<Document> GetActiveDocumentList(int proId, string search, int? limit = 10, int? page = 1, string sort = "");
        Document GetDocumentById(int DocumentId);
        DocumentCommonResponse AddDocument(Document addDocument, string userName);
        DocumentCommonResponse UpdateDocument(Document updateDocument, string userName);
        DocumentCommonResponse SoftDelete(int DocumentId, string userName);
        bool HardDelete(int DocumentId);
    }
}
