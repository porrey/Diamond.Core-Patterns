using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Configuration;
using Unity;

#pragma warning disable DF0001

namespace Diamond.Patterns.UnityConfiguration
{
	public delegate void OnFileLoadingDelegate(string file);

	public static class UnityConfigDecorator
	{
		/// <summary>
		/// Loads the Unity configuration file specified in the path.
		/// </summary>
		/// <param name="unity">The Unity container instance to load the configuration into.</param>
		/// <param name="filePath">The full path to the Unity configuration file.</param>
		/// <param name="containerName">The optional Container name to load. The default loads the unnamed Container.</param>
		/// <returns></returns>
		public static Task LoadUnityConfigFromFileAsync(this IUnityContainer unity, string filePath, string containerName = "")
		{
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
			Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			UnityConfigurationSection unitySection = (UnityConfigurationSection)configuration.GetSection("unity");

			if (unitySection != null)
			{
				if (String.IsNullOrWhiteSpace(containerName))
				{
					_ = unity.LoadConfiguration(unitySection);
				}
				else
				{
					_ = unity.LoadConfiguration(unitySection, containerName);
				}
			}
			else
			{
				throw new InvalidOperationException($"The file {filePath} does not contain a valid Unity configuration section.");
			}

			return Task.FromResult(0);
		}

		/// <summary>
		/// Loads the Unity configuration files found in the specified list.
		/// </summary>
		/// <param name="unity">The Unity container instance to load the configuration into.</param>
		/// <param name="files">A list of file paths where the files to be loaded are located.</param>
		public static async Task LoadUnityConfigFromFilesAsync(this IUnityContainer unity, IEnumerable<string> files, OnFileLoadingDelegate callback = null)
		{
			foreach (string file in files)
			{
				if (callback != null)
				{
					callback.Invoke(file);
				}

				// ***
				// *** Load the default unity container.
				// ***
				await unity.LoadUnityConfigFromFileAsync(file, null);
			}
		}

		/// <summary>
		/// Loads the Unity configuration files found in the path specified.
		/// </summary>
		/// <param name="unity">The Unity container instance to load the configuration into.</param>
		/// <param name="path">The folder path where the Unity configuration files are located.</param>
		/// <param name="searchPattern">Specifies a pattern to use to match file names. The default is *.config.</param>
		/// <param name="searchOption">Specifies the search option to use. the default is SearchOption.TopDirectoryOnly.</param>
		public static async Task<IEnumerable<string>> LoadUnityConfigFromFolderAsync(this IUnityContainer unity, string path, string searchPattern = "*.config", SearchOption searchOption = SearchOption.TopDirectoryOnly, OnFileLoadingDelegate callback = null)
		{
			string[] files = Directory.GetFiles(path, searchPattern, searchOption);

			foreach (string file in files)
			{
				if (callback != null)
				{
					callback.Invoke(file);
				}

				// ***
				// *** Load the default unity container.
				// ***
				await unity.LoadUnityConfigFromFileAsync(file, null);
			}

			return files;
		}
	}
}
