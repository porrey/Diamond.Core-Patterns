//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Provides functionality to parse a JSON configuration file into a dictionary of key-value pairs, where keys
	/// represent hierarchical paths in the JSON structure.
	/// </summary>
	/// <remarks>This class is designed to process JSON configuration files and convert their structure into a flat
	/// dictionary format. Keys in the dictionary represent the hierarchical paths of the JSON properties, separated by
	/// colons (e.g., "Parent:Child:Property").</remarks>
	public class ServicesConfigurationFileParser
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServicesConfigurationFileParser"/> class.
		/// </summary>
		/// <remarks>This constructor creates a default instance of the <see cref="ServicesConfigurationFileParser"/> 
		/// for parsing service configuration files. Use this class to load and interpret configuration data  for
		/// services.</remarks>
		public ServicesConfigurationFileParser()
		{
		}

		private readonly SortedDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		private readonly Stack<string> _paths = new Stack<string>();
		private int _baseIndex = 0;

		/// <summary>
		/// Parses the specified JSON string and returns a dictionary of key-value pairs.
		/// </summary>
		/// <param name="baseIndex">The base index used to adjust the parsing logic. Must be a non-negative integer.</param>
		/// <param name="json">The JSON string to parse. Cannot be <see langword="null"/> or empty.</param>
		/// <returns>A dictionary containing the parsed key-value pairs from the JSON string.  The dictionary will be empty if the JSON
		/// string does not contain any valid key-value pairs.</returns>
		public static IDictionary<string, string> Parse(int baseIndex, string json) => new ServicesConfigurationFileParser().ParseStream(baseIndex, json);

		private IDictionary<string, string> ParseStream(int baseIndex, string json)
		{
			_baseIndex = baseIndex;

			//
			// Set up JSON options.
			//
			JsonDocumentOptions jsonDocumentOptions = new JsonDocumentOptions
			{
				CommentHandling = JsonCommentHandling.Skip,
				AllowTrailingCommas = true,
			};

			//
			// Create a document and parse the JSON text.
			//
			using (JsonDocument doc = JsonDocument.Parse(json, jsonDocumentOptions))
			{
				//
				// Only allow an object at the root of the document.
				//
				if (doc.RootElement.ValueKind != JsonValueKind.Object)
				{
					throw new FormatException("Invalid top level JSON object.");
				}

				//
				// Start parsing t the top level object.
				//
				this.VisitElement(doc.RootElement);
			}

			return _data;
		}

		private void VisitElement(JsonElement element)
		{
			bool isEmpty = true;

			foreach (JsonProperty property in element.EnumerateObject())
			{
				isEmpty = false;
				this.AddPathItem(property.Name);
				this.VisitValue(property.Value);
				this.GetPath();
			}

			if (isEmpty && _paths.Count > 0)
			{
				_data[_paths.Peek()] = null;
			}
		}

		private void VisitValue(JsonElement value)
		{
			switch (value.ValueKind)
			{
				case JsonValueKind.Object:
					this.VisitElement(value);
					break;

				case JsonValueKind.Array:
					int index = _baseIndex;
					
					foreach (JsonElement arrayElement in value.EnumerateArray())
					{
						this.AddPathItem(index.ToString());
						this.VisitValue(arrayElement);
						this.GetPath();
						index++;
					}
					break;

				case JsonValueKind.Number:
				case JsonValueKind.String:
				case JsonValueKind.True:
				case JsonValueKind.False:
				case JsonValueKind.Null:
					string key = _paths.Peek();

					if (_data.ContainsKey(key))
					{
						throw new FormatException($"The key '{key}' is duplicated.");
					}

					_data[key] = value.ToString();
					break;

				default:
					break;
					throw new FormatException($"Unsupported JSON token '{value.ValueKind}'.");
			}
		}

		private void AddPathItem(string pathItem) => _paths.Push(_paths.Count > 0 ? _paths.Peek() + ":" + pathItem : pathItem);

		private void GetPath() => _paths.Pop();
	}
}
