using ds.pms.apicommon.Models;
using ds.pms.bl.clients.Models;

namespace ds.pms.bl.clients.IServices
{
    public interface IClientService
    {
        PaginatedList<Client> GetActiveClientList(string search, int? limit = 10, int? page = 1, string sort = "");
        Client GetClientById(int clientId);
        ClientCommonResponse AddClient(Client addClient, string userName);
        ClientCommonResponse UpdateClient(UpdateClient updateClient, string userName);
        bool IsValidEmail(string email);
        bool IsValidEmail(long? clientId, string email);
        ClientCommonResponse SoftDelete(int clientId, string userName);
        bool HardDelete(int clientId);
        bool CopyRoleToNewlyAddedClient(int clientId, string userName= null);
    }
}
