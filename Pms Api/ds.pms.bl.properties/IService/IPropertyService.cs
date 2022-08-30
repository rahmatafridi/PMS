using ds.pms.apicommon.Models;
using ds.pms.bl.properties.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.properties.IService
{
    public interface IPropertyService
    {
        PaginatedList<Property> GetActivePropertyList(int clientId, int providerId, string search, int? limit = 10, int? page = 1, string sort = "");
        Property GetPropertyById(int PropertyId);
        PropertyCommonResponse AddProperty(Property addProperty, string userName);
        PropertyCommonResponse UpdateProperty(Property updateProperty, string userName);
        PropertyCommonResponse SoftDelete(int PropertyId, string userName);
        bool HardDelete(int PropertyId);
        bool AddComplianceProperty(AddComplianceProperty addComplianceProperty);
    }
}
