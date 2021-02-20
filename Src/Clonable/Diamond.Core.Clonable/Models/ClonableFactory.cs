namespace Diamond.Core.Clonable
{
	/// <summary>
	/// Holds the current factory for cloning objects. 
	/// </summary>
	public static class ClonableFactory
	{
		private static IObjectCloneFactory Factory { get; set; }

		/// <summary>
		/// Gets the current factory for cloning objects.
		/// </summary>
		/// <returns></returns>
		public static IObjectCloneFactory GetFactory()
		{
			if (ClonableFactory.Factory == null)
			{
				throw new NoClonableFactorySetException();
			}

			return ClonableFactory.Factory;
		}

		/// <summary>
		/// Sets the current factory for cloning objects. This value can be set directly
		/// or by referencing a NuGet package that implements the IObjectCloneFactory interface.
		/// </summary>
		/// <param name="factory"></param>
		public static void SetFactory(IObjectCloneFactory factory)
		{
			ClonableFactory.Factory = factory;
		}
	}
}
