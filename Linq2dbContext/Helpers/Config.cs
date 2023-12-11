using Microsoft.Extensions.Configuration;
using System;

namespace Linq2dbContext.Helpers
{
	public static class Config
	{
		public static readonly bool IsDebug = true;
		public static readonly bool IsDebugThread = false;
		static IConfiguration _configuration = null;
		private static IConfiguration Configuration
		{
			get
			{
				if (_configuration == null)
				{
					var physicalPath = $"{AppDomain.CurrentDomain.BaseDirectory}appSetting.json";
					_configuration = new ConfigurationBuilder()
							.AddJsonFile(physicalPath, optional: true, reloadOnChange: true)
							.Build();
				}
				return _configuration;
			}
		}

		private static IConfigurationSection _connections = null;
		public static IConfigurationSection Connections
		{
			get
			{
				if (_connections == null)
					_connections = Configuration.GetSection("ConnectionStrings");

				return _connections;
			}
		}

		private static IConfigurationSection _defaultConns = null;
		public static IConfigurationSection DefaultConns
		{
			get
			{
				if (_defaultConns == null)
					_defaultConns = Configuration.GetSection("DefaultConns");

				return _defaultConns;
			}
		}
	}
}
