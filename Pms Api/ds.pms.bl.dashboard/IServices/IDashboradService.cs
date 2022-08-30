using ds.pms.dal.CustomModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.dashboard.IServices
{
    public interface IDashboradService
    {
        Dashboard LoadDashboardData(int clientId);
 
    }
}
