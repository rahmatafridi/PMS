using ds.pms.dal.Models;

namespace ds.pms.bl.clients.Models
{
    public class Client
    {
        public int Id { get; set; } // int
        public string Name { get; set; } // varchar(200)
        public string Email { get; set; } // varchar(100)
        public string Mobile { get; set; } // varchar(20)
        public string Website { get; set; } // varchar(200)
        public string Address1 { get; set; } // varchar(2000)
        public string Address2 { get; set; } // varchar(2000)
        public string Address3 { get; set; } // varchar(2000)
        public string PostCode { get; set; } // varchar(20)
        public string City { get; set; } // varchar(200)
        public string County { get; set; } // varchar(200)
        public string DisplayName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }

        public static implicit operator Client(TblClient dbClient)
        {
            if (dbClient != null)
            {
                Client dlClient = new Client()
                {
                    Id = dbClient.Id,
                    Name = dbClient.Name,
                    Email = dbClient.Email,
                    Mobile = dbClient.Mobile,
                    Website = dbClient.Website,
                    Address1 = dbClient.Address1,
                    Address2 = dbClient.Address2,
                    Address3 = dbClient.Address3,
                    PostCode = dbClient.PostCode,
                    City = dbClient.City,
                    County = dbClient.County,
                };
                return dlClient;
            }
            return null;
        }

        public static implicit operator TblClient(Client dlClient)
        {
            if (dlClient != null)
            {
                TblClient dbClient = new TblClient()
                {
                    Id = dlClient.Id,
                    Name = dlClient.Name,
                    Email = dlClient.Email,
                    Mobile = dlClient.Mobile,
                    Website = dlClient.Website,
                    Address1 = dlClient.Address1,
                    Address2 = dlClient.Address2,
                    Address3 = dlClient.Address3,
                    PostCode = dlClient.PostCode,
                    City = dlClient.City,
                    County = dlClient.County,
                };
                return dbClient;
            }
            return null;
        }
    }
}
