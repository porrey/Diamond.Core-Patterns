using System.Reflection;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public class DependencyInfo
	{
		/// <summary>
		/// 
		/// </summary>
		public PropertyInfo PropertyInfo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DependencyAttribute DependencyAttribute { get; set; }
	}
}
