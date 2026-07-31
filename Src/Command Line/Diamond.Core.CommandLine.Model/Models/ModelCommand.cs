//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.CommandLine.Model
{
	/// <summary>
	/// 
	/// </summary>
	public class ModelCommand<TModel> : Command, ICommand
	{
		private static readonly MethodInfo s_getValueByName =
			typeof(ParseResult)
				.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				.First(m => m.Name == "GetValue"
						 && m.IsGenericMethod
						 && m.GetParameters().Length == 1
						 && m.GetParameters()[0].ParameterType == typeof(string));

		private readonly List<(PropertyInfo Property, Option Option)> _optionBindings = new();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		public ModelCommand(string name, string description = null)
			: base(name, description)
		{
			this.BuildOptions();
			this.RegisterAction();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		public ModelCommand(ILogger<ModelCommand<TModel>> logger, string name, string description = null)
			: base(name, description)
		{
			this.Logger = logger;
			this.BuildOptions();
			this.RegisterAction();
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<ModelCommand<TModel>> Logger { get; set; } = new NullLogger<ModelCommand<TModel>>();

		private void RegisterAction()
		{
			this.SetAction(async (ParseResult parseResult, CancellationToken cancellationToken) =>
			{
				TModel model = Activator.CreateInstance<TModel>();

				foreach ((PropertyInfo property, Option option) in this._optionBindings)
				{
					object value = s_getValueByName
						.MakeGenericMethod(property.PropertyType)
						.Invoke(parseResult, new object[] { option.Name });
					property.SetValue(model, value);
				}

				return await this.OnHandleCommand(model);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		protected void BuildOptions()
		{
			//
			// Get the type for the model.
			//
			Type t = typeof(TModel);

			//
			// Get the properties on the model.
			//
			PropertyInfo[] properties = t.GetProperties();

			//
			// The options are loaded into a list so they can be sorted.
			//
			IList<OptionDescriptor> items = new List<OptionDescriptor>();

			//
			// Try to load each property.
			//
			foreach (PropertyInfo property in properties)
			{
				//
				// Only load member types that can are read/write.
				//
				if (property.MemberType == MemberTypes.Property && property.CanWrite && property.CanRead)
				{
					//
					// Get the custom attributes on the model property.
					//
					IEnumerable<Attribute> attrs = property.GetCustomAttributes();

					//
					// Check if the Required attribute is present.
					//
					bool isRequired = attrs.Where(t => (Type)t.TypeId == typeof(RequiredAttribute)).SingleOrDefault() != null;

					//
					// Use the display attribute for the rest of the option properties.
					//
					DisplayAttribute display = attrs.Where(t => (Type)t.TypeId == typeof(DisplayAttribute)).SingleOrDefault() as DisplayAttribute;

					//
					// Create the option descriptor.
					//
					Func<string, string> s2 = (string n) => { return n[..2] == "--" ? n : $"--{n}"; };
					Func<string, string> s1 = (string n) => { return n[..1] == "-" ? n : $"-{n}"; };

					OptionDescriptor optionDescriptor = new()
					{
						Name = s2((display?.Name) ?? property.Name).ToLower(),
						Alias = s1((display?.ShortName) ?? property.Name[..1].ToLower()),
						Description = (display?.Description) ?? property.Name,
						Order = (display?.Order) ?? 0,
						IsRequired = isRequired,
						PropertyType = property.PropertyType,
						ModelProperty = property
					};

					//
					// Add the descriptor to the list.
					//
					items.Add(optionDescriptor);
				}
			}

			//
			// Add the options to this command is sorted order.
			//
			foreach (OptionDescriptor item in items.OrderBy(t => t.Order))
			{
				this.Logger.LogDebug("Adding {type} option '{optionName}' to the '{commandName}' command [Description ='{description}', Type = {namespace}.{type}].", item.IsRequired ? "required" : "optional", item.Name, this.Name, item.Description, item.PropertyType.Namespace, item.PropertyType.Name);

				Type[] typeArgs = { item.PropertyType };
				Type makeme = typeof(Option<>).MakeGenericType(typeArgs);
				Option option = (Option)Activator.CreateInstance(makeme, item.Name, new string[] { item.Alias });
				option.Description = item.Description;

				this.Add(option);
				this._optionBindings.Add((item.ModelProperty, option));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual Task<int> OnHandleCommand(TModel item)
		{
			return Task.FromResult(0);
		}
	}
}
