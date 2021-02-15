namespace Diamond.Core.Repository
{
	/// <summary>
	/// 
	/// </summary>
	public static class RepositoryDecorator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IReadOnlyRepository<TInterface> AsReadonly<TInterface>(this IWritableRepository<TInterface> item)
			where TInterface : IEntity
		{
			IReadOnlyRepository<TInterface> returnValue = item as IReadOnlyRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotReadableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IQueryableRepository<TInterface> AsQueryable<TInterface>(this IWritableRepository<TInterface> item)
			where TInterface : IEntity
		{
			IQueryableRepository<TInterface> returnValue = item as IQueryableRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotQueryableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IReadOnlyRepository<TInterface> AsReadonly<TInterface>(this IQueryableRepository<TInterface> item)
			where TInterface : IEntity
		{
			IReadOnlyRepository<TInterface> returnValue = item as IReadOnlyRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotReadableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IWritableRepository<TInterface> AsWritable<TInterface>(this IQueryableRepository<TInterface> item)
			where TInterface : IEntity
		{
			IWritableRepository<TInterface> returnValue = item as IWritableRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotWritableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IQueryableRepository<TInterface> AsQueryable<TInterface>(this IReadOnlyRepository<TInterface> item)
			where TInterface : IEntity
		{
			IQueryableRepository<TInterface> returnValue = item as IQueryableRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotQueryableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IWritableRepository<TInterface> AsWritable<TInterface>(this IReadOnlyRepository<TInterface> item)
			where TInterface : IEntity
		{
			IWritableRepository<TInterface> returnValue = item as IWritableRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotWritableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IReadOnlyRepository<TInterface> AsReadonly<TInterface>(this IRepository<TInterface> item)
			where TInterface : IEntity
		{
			IReadOnlyRepository<TInterface> returnValue = item as IReadOnlyRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotReadableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IWritableRepository<TInterface> AsWritable<TInterface>(this IRepository<TInterface> item)
			where TInterface : IEntity
		{
			IWritableRepository<TInterface> returnValue = item as IWritableRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotWritableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IQueryableRepository<TInterface> AsQueryable<TInterface>(this IRepository<TInterface> item)
			where TInterface : IEntity
		{
			IQueryableRepository<TInterface> returnValue = item as IQueryableRepository<TInterface>;

			if (returnValue == null)
			{
				throw new RepositoryNotQueryableException(item.GetType());
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsReadonly<TInterface>(this IWritableRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IReadOnlyRepository<TInterface>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsQueryable<TInterface>(this IWritableRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IQueryableRepository<TInterface>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsReadonly<TInterface>(this IQueryableRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IReadOnlyRepository<TInterface>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsWritable<TInterface>(this IQueryableRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IWritableRepository<TInterface>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsQueryable<TInterface>(this IReadOnlyRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IQueryableRepository<TInterface>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsWritable<TInterface>(this IReadOnlyRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IWritableRepository<TInterface>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsReadonly<TInterface>(this IRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IReadOnlyRepository<TInterface>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsWritable<TInterface>(this IRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IWritableRepository<TInterface>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsQueryable<TInterface>(this IRepository<TInterface> item)
			where TInterface : IEntity
		{
			return (item is IQueryableRepository<TInterface>);
		}
	}
}