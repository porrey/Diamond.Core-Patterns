using System;

namespace Diamond.Core.Repository.DateTimeModel
{
	/// <summary>
	/// Provides extension methods for the Platinum application.
	/// </summary>
	public static class DbEnvironment
	{
		/// <summary>
		/// Gets the current system user.
		/// </summary>
		public static string CurrentUser => $@"{Environment.UserDomainName}\{Environment.UserName}";
	}
}
