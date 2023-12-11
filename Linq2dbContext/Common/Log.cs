using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog;

namespace Linq2dbContext.Common
{
	public static class Log
	{
		public static Logger Instance { get; private set; }
		static Log()
		{
			LogManager.ReconfigExistingLoggers();
			Instance = LogManager.GetCurrentClassLogger();
		}

		public static void Shutdown()
		{
			if (Instance != null)
				Instance = null;

			LogManager.Shutdown();
		}
	}
}
