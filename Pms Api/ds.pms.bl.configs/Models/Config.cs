using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.configs.Models
{
    public class Config
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public string Key { get; set; } // varchar(50)
        public string Value { get; set; } // varchar(500)
        public string Description { get; set; } // varchar(500)
        public string Client { get; set; }


        public static implicit operator Config(TblConfig dbConfig)
        {
            if (dbConfig != null)
            {
                Config dlConfig = new Config()
                {
                    Id = dbConfig.Id,
                    ClientId = dbConfig.ClientId,
                    Key = dbConfig.Key,
                    Value = dbConfig.Value,
                    Description = dbConfig.Description,
                };
                return dlConfig;
            }
            return null;
        }

        public static implicit operator TblConfig(Config dlConfig)
        {
            if (dlConfig != null)
            {
                TblConfig dbConfig = new TblConfig()
                {
                    Id = dlConfig.Id,
                    ClientId = dlConfig.ClientId,
                    Key = dlConfig.Key,
                    Value = dlConfig.Value,
                    Description = dlConfig.Description,
                };
                return dbConfig;
            }
            return null;
        }


        public static implicit operator Config(ConfigList db)
        {
            if (db != null)
            {
                Config dl = new Config()
                {
                    Id=db.Id,
                    Client= db.Client,
                    ClientId= db.ClientId,
                    Description= db.Description,
                    Value= db.Value,
                    Key= db.Key
                };
                return dl;
            }
            return null;
        }

        public static implicit operator ConfigList(Config dl)
        {
            if (dl != null)
            {
                ConfigList db = new ConfigList()
                {
                    Id = dl.Id,
                    ClientId = dl.ClientId,
                    Client = dl.Client,
                    Description = dl.Description,
                    Value = dl.Value,
                    Key = dl.Key
                };
                return db;
            }
            return null;
        }
    }
}
