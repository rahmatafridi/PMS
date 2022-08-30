using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.properties.Converters;
using ds.pms.bl.properties.IService;
using ds.pms.bl.properties.Models;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ds.pms.bl.properties.Service
{
    public class PropertyService : IPropertyService
    {
        private PropertyRepository propertyRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public PropertyService(string _databaseProperty, string _connectionString, ILogger<PropertyService> propertyServiceLogger)
        {
            propertyRepository = new PropertyRepository(_databaseProperty, _connectionString);
            logging = new Logging(propertyServiceLogger);
            _className = this.GetType().Name;
        }

        public PropertyService(string _databaseProperty, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<PropertyService> propertyServiceLogger)
        {
            propertyRepository = new PropertyRepository(_databaseProperty, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(propertyServiceLogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Property> GetActivePropertyList(int clientId, int providerId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Property> propertiesPaginatedList = new PaginatedList<Property>();
            try
            {
                int pageSize = limit ?? 10;
                int pageNumber = page ?? 1;
                string sortBy = string.Empty, sortDirection = string.Empty;
                if (!string.IsNullOrEmpty(sort))
                {
                    string[] sorting = SortDirection.SortDir(sort);
                    if (sorting.Length > 1)
                    {
                        sortBy = sorting[0];
                        sortDirection = sorting[1];
                    }
                }
                propertiesPaginatedList = Pagination.ConvertDalToBl(propertyRepository.GetActivePropertyList(clientId, providerId, search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return propertiesPaginatedList;
        }

        public Property GetPropertyById(int propertyId)
        {
            try
            {
                if (propertyId > 0)
                {
                    Property property = propertyRepository.GetPropertyById(propertyId);
                    if (property != null)
                        return property;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public PropertyCommonResponse AddProperty(Property addProperty, string userName)
        {
            PropertyCommonResponse propertyCommonResponse = new PropertyCommonResponse();
            try
            {
                propertyCommonResponse.Success = false;
                if (addProperty != null)
                {
                    TblProperty tblProperty = addProperty;
                    tblProperty.AddedBy = userName;
                    tblProperty.AddedDate = DateTime.UtcNow;
                    //tblProperty.IsDeleted = false;
                    propertyCommonResponse.Property = propertyRepository.Add(tblProperty);
                    if (propertyCommonResponse.Property != null && propertyCommonResponse.Property.Id > 0)
                    {
                        if(propertyCommonResponse.Property.NumberOfRooms != null || propertyCommonResponse.Property.NumberOfRooms != null)
                        {
                           // int i = (int)propertyCommonResponse.Property.NumberOfRooms;
                            for ( int i = 1;  i <= propertyCommonResponse.Property.NumberOfRooms;  i++)
                            {
                                var room = new TblRoom();
                                room.AddedBy = userName;
                                room.AddedDate = DateTime.Now;
                                room.CoreRent = 0;
                                room.RoomNo = i;
                                room.IsActive = true;
                                room.PersonalCharge = 0;
                                room.ServiceCharge = 0;
                                room.PropertyId = propertyCommonResponse.Property.Id;
                                propertyRepository.AddRoom(room);
                            }
                        }
                        propertyCommonResponse.Success = true;
                        return propertyCommonResponse;
                    }
                    else
                        propertyCommonResponse.Message = "Unable to add Property.";
                }
            }
            catch (Exception ex)
            {
                propertyCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return propertyCommonResponse;
        }

        public PropertyCommonResponse UpdateProperty(Property updateProperty, string userName)
        {
            PropertyCommonResponse propertyCommonResponse = new PropertyCommonResponse();
            try
            {
                propertyCommonResponse.Success = false;
                if (updateProperty != null && updateProperty.Id > 0)
                {
                    TblProperty tblProperty;
                    tblProperty = propertyRepository.GetPropertyById(updateProperty.Id);
                    if (tblProperty != null)
                    {
                        tblProperty.ClientId = updateProperty.ClientId;
                        tblProperty.ProviderId = updateProperty.ProviderId;
                        tblProperty.AreaId = updateProperty.AreaId;
                        tblProperty.AgentId = updateProperty.AgentId;
                        tblProperty.Address1 = updateProperty.Address1;
                        tblProperty.Address2 = updateProperty.Address2;
                        tblProperty.Address3 = updateProperty.Address3;
                        tblProperty.PostCode = updateProperty.PostCode;
                        tblProperty.City = updateProperty.City;
                        tblProperty.County = updateProperty.County;
                        tblProperty.Country = updateProperty.Country;
                        tblProperty.FireExitBlanket = updateProperty.FireExitBlanket;
                        tblProperty.NumberOfRooms = updateProperty.NumberOfRooms;
                        tblProperty.TitleNo = updateProperty.TitleNo;
                        tblProperty.LastRegNumber = updateProperty.LastRegNumber;
                        tblProperty.LocalAuth = updateProperty.LocalAuth;
                        tblProperty.DateSlaStart = updateProperty.DateSlaStart;
                        tblProperty.DateSlaEnd = updateProperty.DateSlaEnd;
                        tblProperty.DateLeaseStart = updateProperty.DateLeaseStart;
                        tblProperty.DateLeaseEnd = updateProperty.DateLeaseEnd;
                        tblProperty.DatePreAcceptInsp = updateProperty.DatePreAcceptInsp;
                        tblProperty.DateInspection = updateProperty.DateInspection;
                        tblProperty.DateExempt = updateProperty.DateExempt;
                        tblProperty.DateQuarterlyAudit = updateProperty.DateQuarterlyAudit;
                        tblProperty.DateQuarterlyInsp = updateProperty.DateQuarterlyInsp;
                        tblProperty.IsPublished = updateProperty.IsPublished;
                        tblProperty.ModifiedBy = userName;
                        tblProperty.ModifiedDate = DateTime.UtcNow;
                        propertyCommonResponse.Property = propertyRepository.Update(tblProperty);
                        if (propertyCommonResponse.Property != null && propertyCommonResponse.Property.Id > 0)
                        {
                            propertyCommonResponse.Success = true;
                            return propertyCommonResponse;
                        }
                        else
                            propertyCommonResponse.Message = "Unable to update property.";
                    }
                    else
                        propertyCommonResponse.Message = "Property does not exists.";
                }
                else
                    propertyCommonResponse.Message = "Supplied property information is not valid.";
            }
            catch (Exception ex)
            {
                propertyCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return propertyCommonResponse;
        }

        public PropertyCommonResponse SoftDelete(int propertyId, string userName)
        {
            PropertyCommonResponse propertyCommonResponse = new PropertyCommonResponse();
            try
            {
                propertyCommonResponse.Success = false;
                if (propertyId > 0)
                {
                    TblProperty tblProperty = propertyRepository.GetPropertyById(propertyId);
                    if (tblProperty != null && tblProperty.Id > 0)
                    {
                        tblProperty.ModifiedBy = userName;
                        tblProperty.ModifiedDate = DateTime.UtcNow;
                        tblProperty.IsDeleted = true;
                        tblProperty.DeletedBy = userName;
                        tblProperty.DeletedDate = DateTime.Now;
                        propertyCommonResponse.Property = propertyRepository.Update(tblProperty);
                        if (propertyCommonResponse.Property != null && propertyCommonResponse.Property.Id > 0)
                        {
                            propertyCommonResponse.Success = true;
                            return propertyCommonResponse;
                        }
                        else
                            propertyCommonResponse.Message = "Unable to delete property.";
                    }
                    else
                        propertyCommonResponse.Message = "Property does not exists.";
                }
                else
                    propertyCommonResponse.Message = "Supplied property information is not valid.";
            }
            catch (Exception ex)
            {
                propertyCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return propertyCommonResponse;
        }

        public bool HardDelete(int propertyId)
        {
            try
            {
                if (propertyId > 0)
                {
                    return propertyRepository.Delete(propertyId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public bool AddComplianceProperty(AddComplianceProperty addComplianceProperty)
        {
            bool result = false;
            try
            {
                if (addComplianceProperty != null && addComplianceProperty.PropertyId > 0 && addComplianceProperty.ComplianceIds.Count > 0)
                {
                    foreach (var item in addComplianceProperty.ComplianceIds)
                    {
                        try
                        {
                            TblComplianceProperty tblComplianceProperty = new TblComplianceProperty();
                            tblComplianceProperty.PropertyId = addComplianceProperty.PropertyId;
                            tblComplianceProperty.ComplianceId = item;
                            tblComplianceProperty = propertyRepository.AddComplianceProperty(tblComplianceProperty);
                            if (tblComplianceProperty != null && tblComplianceProperty.Id > 0)
                                result = true;
                        }
                        catch (Exception ex)
                        {
                            _methodName = MethodBase.GetCurrentMethod().Name;
                            logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                result = false;
            }
            return result;
        }
    }
}
