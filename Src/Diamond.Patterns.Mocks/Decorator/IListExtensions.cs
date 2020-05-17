using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Diamond.Patterns.Mocks
{
	public static class IListExtensions
	{
		public delegate void ItemLoadDelegate<TItem>(TItem item);

		public static void FromJsonFile<TItem>(this IList<TItem> list, string file, ItemLoadDelegate<TItem> onItemLoaded = null)
		{
			IEnumerable<TItem> returnValue = new TItem[0];

			// ***
			// *** Use file info to expand relative paths.
			// ***
			FileInfo fileInfo = new FileInfo(file);

			if (fileInfo.Exists)
			{
				// ***
				// *** Load the file.
				// ***
				string json = File.ReadAllText(fileInfo.FullName);

				// ***
				// *** Deserialize the json.
				// ***
				IEnumerable<TItem> items = JsonConvert.DeserializeObject<IEnumerable<TItem>>(json);

				foreach (TItem item in items)
				{
					list.Add(item);
					onItemLoaded?.Invoke(item);
				}
			}
			else
			{
				throw new FileNotFoundException(fileInfo.FullName);
			}
		}

		public static Task FromJsonFileAsync<TItem>(this IList<TItem> list, string file, ItemLoadDelegate<TItem> onItemLoaded = null)
		{
			list.FromJsonFile(file, onItemLoaded);
			return Task.FromResult(0);
		}
	}
}
