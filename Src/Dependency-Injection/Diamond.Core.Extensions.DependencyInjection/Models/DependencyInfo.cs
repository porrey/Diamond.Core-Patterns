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
		public virtual PropertyInfo PropertyInfo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual DependencyAttribute DependencyAttribute { get; set; }
	}
}
