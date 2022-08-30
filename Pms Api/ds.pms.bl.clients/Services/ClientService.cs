using ds.pms.apicommon.Models;
using ds.pms.bl.clients.Converters;
using ds.pms.bl.clients.IServices;
using ds.pms.bl.clients.Models;
using ds.pms.bl.logger;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using ds.pms.helpers.Security;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace pms.bl.clients.Services
{
    public class ClientService : IClientService
    {
        private ClientRepository clientRepository;
        private RoleRepository roleRepository;
        private UserRepository userRepository;
        private ConfigRepository configRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public ClientService(string _databaseProvider, string _connectionString, ILogger<ClientService> logger)
        {
            clientRepository = new ClientRepository(_databaseProvider, _connectionString);
            roleRepository = new RoleRepository(_databaseProvider, _connectionString);
            userRepository = new UserRepository(_databaseProvider, _connectionString);
            configRepository = new ConfigRepository(_databaseProvider, _connectionString);
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public ClientService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds, ILogger<ClientService> logger)
        {
            clientRepository = new ClientRepository(_databaseProvider, _connectionString);
            roleRepository = new RoleRepository(_databaseProvider, _connectionString);
            userRepository = new UserRepository(_databaseProvider, _connectionString);

            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Client> GetActiveClientList(string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Client> clientsPaginatedList = new PaginatedList<Client>();
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
                clientsPaginatedList = Pagination.ConvertDalToBl(clientRepository.GetActiveClientList(search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return clientsPaginatedList;
        }

        public Client GetClientById(int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    Client client = clientRepository.GetClientById(clientId);
                    if (client != null)
                        return client;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public ClientCommonResponse AddClient(Client addClient, string userName)
        {
            ClientCommonResponse clientCommonResponse = new ClientCommonResponse();
            try
            {
                clientCommonResponse.Success = false;
                if (addClient != null)
                {
                    if (IsValidEmail(addClient.Email))
                    {
                        TblClient tblClient = addClient;
                        tblClient.IsActive = true;
                        tblClient.AddedBy = userName;
                        tblClient.AddedDate = DateTime.UtcNow;
                        //tblClient.IsDeleted = false;
                        clientCommonResponse.Client = clientRepository.Add(tblClient);
                        if (clientCommonResponse.Client != null && clientCommonResponse.Client.Id > 0)
                        {
                            var adminRoles = roleRepository.GetAdminRoles();
                            TblUser tblUser = new TblUser();
                            tblUser.AddedBy = userName;
                            tblUser.AddedDate = DateTime.Now;
                            tblUser.ClientId = clientCommonResponse.Client.Id;
                            tblUser.IsActive = true;
                            tblUser.Username = addClient.UserEmail;
                            tblUser.IsDeleted = false;
                            tblUser.Email = addClient.Email;
                            tblUser.Displayname = addClient.DisplayName;
                            tblUser.Password = Hash.GeneratePasswordHash(addClient.Password);

                            var user = userRepository.Add(tblUser);
                            if(user != null)
                            {
                                var userRole = new TblUserRole();
                                var role = new TblRole();
                                foreach (var item in adminRoles)
                                {
                                    role.AddedBy = userName;
                                    role.AddedDate = DateTime.Now;
                                    role.ClientId = user.ClientId;
                                    role.Description = item.Description;
                                    role.IsActive = item.IsActive;
                                    role.IsTemplate = false;
                                    role.Name = item.Name;

                                   var roleResult= roleRepository.Add(role);
                                   if(roleResult != null)
                                    {
                                        userRole.RoleId = roleResult.Id;
                                        userRole.UserId = user.Id;
                                        userRepository.AssignRoleToUser(userRole);
                                    }
                                }
                            }
                            var getConfigs = configRepository.GetAdminConfigs();
                            foreach (var item in getConfigs.Items)
                            {
                                var addConfig = (new TblConfig()
                                {
                                    AddedDate=DateTime.Now,
                                    Description=item.Description,
                                    ClientId= clientCommonResponse.Client.Id,
                                    Key = item.Key,
                                    Value = item.Value
                                });
                                configRepository.Add(addConfig);
                            }

                            clientCommonResponse.Success = true;
                            return clientCommonResponse;
                        }
                        else
                            clientCommonResponse.Message = "Unable to add client.";
                    }
                    else
                        clientCommonResponse.Message = "Email already in use.";
                }
                else
                    clientCommonResponse.Message = "Supplied client information is not valid.";
            }
            catch (Exception ex)
            {
                clientCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return clientCommonResponse;
        }

        public ClientCommonResponse UpdateClient(UpdateClient updateClient, string userName)
        {
            ClientCommonResponse clientCommonResponse = new ClientCommonResponse();
            try
            {
                clientCommonResponse.Success = false;
                if (updateClient != null && updateClient.Id > 0)
                {
                    if (IsValidEmail(updateClient.Id, updateClient.Email))
                    {
                        TblClient tblClient;
                        tblClient = clientRepository.GetClientById(updateClient.Id);
                        if (tblClient != null)
                        {
                            tblClient.Name = updateClient.Name;
                            tblClient.Email = updateClient.Email;
                            tblClient.Mobile = updateClient.Mobile;
                            tblClient.Website = updateClient.Website;
                            tblClient.Address1 = updateClient.Address1;
                            tblClient.Address2 = updateClient.Address2;
                            tblClient.Address3 = updateClient.Address3;
                            tblClient.PostCode = updateClient.PostCode;
                            tblClient.City = updateClient.City;
                            tblClient.County = updateClient.County;
                            //tblClient.IsActive = updateClient.IsActive;
                            tblClient.ModifiedBy = userName;
                            tblClient.ModifiedDate = DateTime.UtcNow;
                            clientCommonResponse.UpdateClient = clientRepository.Update(tblClient);
                            if (clientCommonResponse.UpdateClient != null && clientCommonResponse.UpdateClient.Id > 0)
                            {
                                clientCommonResponse.Success = true;
                                return clientCommonResponse;
                            }
                            else
                                clientCommonResponse.Message = "Unable to update client.";
                        }
                        else
                            clientCommonResponse.Message = "Client does not exists.";
                    }
                    else
                        clientCommonResponse.Message = "Email already in use.";
                }
                else
                    clientCommonResponse.Message = "Supplied client information is not valid.";
            }
            catch (Exception ex)
            {
                clientCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return clientCommonResponse;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                return !clientRepository.DoesEmailExists(email);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public bool IsValidEmail(long? clientId, string email)
        {
            try
            {
                return !clientRepository.DoesAnyOtherUserUseThisEmail(clientId, email);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public ClientCommonResponse SoftDelete(int clientId, string userName)
        {
            ClientCommonResponse clientCommonResponse = new ClientCommonResponse();
            try
            {
                clientCommonResponse.Success = false;
                if (clientId > 0)
                {
                    TblClient tblClient = clientRepository.GetClientById(clientId);
                    if (tblClient != null && tblClient.Id > 0)
                    {
                        tblClient.ModifiedBy = userName;
                        tblClient.ModifiedDate = DateTime.UtcNow;
                        tblClient.IsDeleted = true;
                        tblClient.DeletedBy = userName;
                        tblClient.DeletedDate = DateTime.Now;
                        clientCommonResponse.UpdateClient = clientRepository.Update(tblClient);
                        if (clientCommonResponse.UpdateClient != null && clientCommonResponse.UpdateClient.Id > 0)
                        {
                            clientCommonResponse.Success = true;
                            return clientCommonResponse;
                        }
                        else
                            clientCommonResponse.Message = "Unable to delete client.";
                    }
                    else
                        clientCommonResponse.Message = "Client does not exists.";
                }
                else
                    clientCommonResponse.Message = "Supplied client information is not valid.";
            }
            catch (Exception ex)
            {
                clientCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return clientCommonResponse;
        }

        public bool HardDelete(int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    return clientRepository.Delete(clientId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public bool CopyRoleToNewlyAddedClient(int clientId, string userName=null)
        {
            bool result = false;
            try
            {
                if (clientId > 0)
                {
                    var rolesToAdd = clientRepository.CopyRoleToNewlyAddedClient();
                    foreach (var role in rolesToAdd)
                    {
                        TblRole tblRole = new TblRole();
                        tblRole.ClientId = clientId;
                        tblRole.Name = role.Name;
                        tblRole.Description = role.Description;
                        tblRole.IsActive = true;
                        tblRole.IsTemplate = false;
                        tblRole.AddedBy = userName;
                        tblRole.AddedDate = DateTime.UtcNow;
                        roleRepository.Add(tblRole);
                    }
                    result = true;
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
