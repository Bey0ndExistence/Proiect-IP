using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
public interface IDatabaseAccess
    {
        bool RegisterUser(Dictionary<string, string> userDict);
        Dictionary<string, string> GetUserInfo(string username, List<string> fields);
        string GetLogs(string username1, string username2);
        bool SaveLogs(string username1, string username2, string conversation);
        bool LoginCheck(Dictionary<string, string> credentials);
    }
}
