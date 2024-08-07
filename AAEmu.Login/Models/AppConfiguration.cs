﻿using AAEmu.Commons.Models;
using AAEmu.Commons.Utils;

namespace AAEmu.Login.Models;

public class AppConfiguration : Singleton<AppConfiguration>
{
    public string SecretKey { get; set; }
    public bool AutoAccount { get; set; }
    public bool SkipHostResolve { get; set; }
    public DBConnections Connections { get; set; }
    public NetworkConfig InternalNetwork { get; set; }
    public NetworkConfig Network { get; set; }

    public class NetworkConfig
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public int NumConnections { get; set; }
    }

    public class DBConnections
    {
        public MySqlConnectionSettings MySQLProvider { get; set; }
    }
}
