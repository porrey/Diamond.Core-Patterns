using System;
using System.Collections.Generic;
using System.ComponentModel;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public class StateDictionary : Dictionary<string, object>, IStateDictionary
	{
		public T Get<T>(string key, T defaultValue = default(T))
		{
			T returnValue = defaultValue;

			if (this.ContainsKey(key.ToLower()))
			{
				if (!this.CanConvertParameter(key, out returnValue))
				{
					returnValue = defaultValue;
				}
			}

			return returnValue;
		}

		public TProperty Get<TProperty>(string key)
		{
			TProperty returnValue = default(TProperty);

			if (this.ContainsKey(key.ToLower()))
			{
				if (!this.CanConvertParameter(key, out returnValue))
				{
					throw new InvalidCastException();
				}
			}
			else
			{
				throw new ArgumentOutOfRangeException($"The context dictionary does not have a property named '{key}'.");
			}

			return returnValue;
		}

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

		public void Set<TProperty>(string key, TProperty value)
		{
			if (this.ContainsKey(key.ToLower()))
			{
				this[key.ToLower()] = value;
			}
			else
			{
				this.Add(key.ToLower(), value);
			}
		}

		public bool CanConvertParameter(string key, Type targetType, out object convertedValue)
		{
			bool returnValue = false;
			convertedValue = null;

			// ***
			// *** Get the string value
			// ***
			object value = this[key.ToLower()];

			// ***
			// *** Get the converter for string
			// ***
			TypeConverter converter = TypeDescriptor.GetConverter(targetType);

			// ***
			// *** Check if the value can be converted
			// ***
			if (value is string)
			{
				if (targetType == typeof(int))
				{
					convertedValue = Convert.ToInt32(value);
					returnValue = true;
				}
				else if (targetType == typeof(bool))
				{
					string stringValue = (string)value;

					if (stringValue.ToLower() == "yes" || stringValue.ToLower() == "true" || stringValue.ToLower() == "1" || stringValue.ToLower() == "on")
					{
						convertedValue = Convert.ChangeType(true, targetType);
						returnValue = true;
					}
					else
					{
						convertedValue = false;
						returnValue = true;
					}
				}
				else if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
				{
					string stringValue = (string)value;

					// ***
					// *** Allow the word NOW to indicate the current date and time.
					// ***
					if (stringValue.ToLower().Equals("now"))
					{
						convertedValue = DateTime.Now;
						returnValue = true;
					}
					else if (converter.CanConvertFrom(typeof(string)))
					{
						convertedValue = converter.ConvertFromString(stringValue);
						returnValue = true;
					}
				}
				else if (targetType == typeof(TimeSpan))
				{
					convertedValue = Convert.ChangeType(TimeSpan.Parse((string)value), targetType);
					returnValue = true;
				}
				else if (converter.CanConvertFrom(typeof(string)))
				{
					convertedValue = converter.ConvertFromString((string)value);
					returnValue = true;
				}
			}
			else
			{
				convertedValue = value;
				returnValue = true;
			}

			return returnValue;
		}

		public bool CanConvertParameter<T>(string key, out T convertedValue)
		{
			bool returnValue = false;
			convertedValue = default(T);

			object convertedObject = null;
			if (this.CanConvertParameter(key, typeof(T), out convertedObject))
			{
				convertedValue = (T)convertedObject;
				returnValue = true;
			}

			return returnValue;
		}

		public new bool ContainsKey(string key)
		{
			return base.ContainsKey(key.ToLower());
		}

		public new void Add(string key, object value)
		{
			base.Add(key.ToLower(), value);
		}

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
