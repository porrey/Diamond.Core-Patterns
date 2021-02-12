//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System.Diagnostics;
using System.Text.Json;

namespace Diamond.Core.Extensions.Configuration.Services
{
	internal class JsonConfigurationFileParser
	{
		internal JsonConfigurationFileParser()
		{
		}

		private readonly SortedDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		private readonly Stack<string> _paths = new Stack<string>();
		private int _baseIndex = 0;

		public static IDictionary<string, string> Parse(int baseIndex, string json)
			=> new JsonConfigurationFileParser().ParseStream(baseIndex, json);

		private IDictionary<string, string> ParseStream(int baseIndex, string json)
		{
			_baseIndex = baseIndex;

			JsonDocumentOptions jsonDocumentOptions = new JsonDocumentOptions
			{
				CommentHandling = JsonCommentHandling.Skip,
				AllowTrailingCommas = true,
			};

			using (JsonDocument doc = JsonDocument.Parse(json, jsonDocumentOptions))
			{
				if (doc.RootElement.ValueKind != JsonValueKind.Object)
				{
					throw new FormatException("Invalid top level JSON object.");
				}

				VisitElement(doc.RootElement);
			}

			return _data;
		}

		private void VisitElement(JsonElement element)
		{
			var isEmpty = true;

			foreach (JsonProperty property in element.EnumerateObject())
			{
				isEmpty = false;
				EnterContext(property.Name);
				VisitValue(property.Value);
				ExitContext();
			}

			if (isEmpty && _paths.Count > 0)
			{
				_data[_paths.Peek()] = null;
			}
		}

		private void VisitValue(JsonElement value)
		{
			Debug.Assert(_paths.Count > 0);

			switch (value.ValueKind)
			{
				case JsonValueKind.Object:
					VisitElement(value);
					break;

				case JsonValueKind.Array:
					int index = _baseIndex;
					foreach (JsonElement arrayElement in value.EnumerateArray())
					{
						EnterContext(index.ToString());
						VisitValue(arrayElement);
						ExitContext();
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

		private void EnterContext(string context) =>
			_paths.Push(_paths.Count > 0 ?
				_paths.Peek() + ":" + context :
				context);

		private void ExitContext() => _paths.Pop();
	}
}
