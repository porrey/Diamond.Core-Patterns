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
using System.Collections.Concurrent;
using System.Linq;

namespace Diamond.Core.Workflow.State
{
	/// <summary>
	/// 
	/// </summary>
	public class StateDictionary : ConcurrentDictionary<string, object>, IStateDictionary
	{
		/// <summary>
		/// 
		/// </summary>
		public StateDictionary()
			: this(new IStateTypeConverter[]
					{
						new NullableIntConverter(),
						new IntConverter(),
						new BoolConverter(),
						new DateConverter(),
						new NullableDateConverter(),
						new TimeSpanConverter(),
						new EnumConverter(),
						new StringConverter(),
						new StringArrayConverter(),
						new UintConverter(),
						new SingleConverter(),
						new DoubleConverter(),
						new IDictionaryConverter()
					})
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="converters"></param>
		public StateDictionary(IStateTypeConverter[] converters)
		{
			this.Converters = converters;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IStateTypeConverter[] Converters { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public T Get<T>(string key, T defaultValue = default)
		{
			T returnValue = defaultValue;

			if (this.ContainsKey(key.ToLower()))
			{
				(bool success, string errorMessage, T convertedValue) = this.ConvertParameter<T>(key);

				if (success)
				{
					returnValue = convertedValue;
				}
				else
				{
					returnValue = defaultValue;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public TProperty Get<TProperty>(string key)
		{
			TProperty returnValue = default(TProperty);

			if (this.ContainsKey(key.ToLower()))
			{
				(bool success, string errorMessage, TProperty convertedValue) = this.ConvertParameter<TProperty>(key);

				if (success)
				{
					returnValue = convertedValue;
				}
				else
				{
					throw new InvalidCastException();
				}
			}
			else
			{
				throw new MissingContextPropertyException(key);
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="key"></param>
		/// <param name="initializeValue"></param>
		/// <returns></returns>
		public TProperty TryGet<TProperty>(string key, TProperty initializeValue)
		{
			TProperty returnValue = initializeValue;

			if (this.ContainsKey(key))
			{
				returnValue = (TProperty)this[key.ToLower()];
			}
			else
			{
				this.Add(key.ToLower(), initializeValue);
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set<TProperty>(string key, TProperty value)
		{
			if (this.ContainsKey(key))
			{
				//
				// Try to dispose this object. If it is not disposable
				// nothing will happen.
				//
				object obj = this[key];

				if (obj != null)
				{
					ITryDisposable<object> disposable = TryDisposableFactory.Create(obj);
					disposable.Dispose();
				}

				this[key] = value;
			}
			else
			{
				this.Add(key, value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="targetType"></param>
		/// <returns></returns>
		public (bool, string, object) ConvertParameter(string key, Type targetType)
		{
			(bool Success, string ErrorMessage, object ConvertedValue) returnValue = (false, null, null);

			//
			// Get the string value
			//
			object value = this[key.ToLower()];

			//
			// Check if the value can be converted
			//
			if (value is string)
			{
				IStateTypeConverter customConverter = (from tbl in this.Converters
													   where tbl.TargetType == targetType ||
													   tbl.TargetType == targetType.BaseType
													   select tbl).FirstOrDefault();

				if (customConverter != null)
				{
					returnValue = customConverter.ConvertSource(value, targetType);
				}
				else
				{
					throw new ArgumentOutOfRangeException($"A value converter for type '{targetType.Name}' could not be found.");
				}
			}
			else
			{
				returnValue.ConvertedValue = value;
				returnValue.Success = true;
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public (bool, string, T) ConvertParameter<T>(string key)
		{
			(bool Success, string ErrorMessage, T ConvertedValue) returnValue = (false, null, default);

			(bool success, string errorMessage, object convertedValue) = this.ConvertParameter(key, typeof(T));

			returnValue.Success = success;
			returnValue.ConvertedValue = (T)convertedValue;
			returnValue.ErrorMessage = errorMessage;

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public new bool ContainsKey(string key)
		{
			return base.ContainsKey(key.ToLower());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(string key, object value)
		{
			if (!base.TryAdd(key.ToLower(), value))
			{
				throw new AddItemToStateException(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public new object this[string key]
		{
			get
			{
				return base[key.ToLower()];
			}
			set
			{
				base[key.ToLower()] = value;
			}
		}
	}
}
