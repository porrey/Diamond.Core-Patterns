using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Provides extension methods for converting arrays of objects to strongly typed arrays.
	/// </summary>
	/// <remarks>The methods in this class are intended to simplify conversion scenarios where an array of objects
	/// needs to be cast or converted to a specific type. Null values and database nulls are handled according to .NET
	/// conventions for default values and type compatibility. Use these extensions when working with data sources that
	/// return object arrays, such as database query results.</remarks>
	public static class ObjectArrayExtensions
	{
		/// <summary>
		/// Converts an array of objects to an array of the specified type, handling null and database null values
		/// appropriately.
		/// </summary>
		/// <remarks>If a value in the input array is null or DBNull.Value, it is converted to the default value of T
		/// when T is a reference or nullable type. Otherwise, an exception is thrown. Values that are not already of type T
		/// are converted using Convert.ChangeType with invariant culture.</remarks>
		/// <typeparam name="T">The target element type for the resulting array. Must be compatible with the values in the input array.</typeparam>
		/// <param name="values">The array of objects to convert. Cannot be null.</param>
		/// <returns>An array of type T containing the converted values from the input array. Null and DBNull values are converted to
		/// the default value of T if T is a reference or nullable type.</returns>
		/// <exception cref="ArgumentNullException">Thrown if the values parameter is null.</exception>
		/// <exception cref="InvalidCastException">Thrown if a null or DBNull value is encountered and T is a non-nullable value type, or if a value cannot be
		/// converted to type T.</exception>
		public static T[] ToArrayOf<T>(this object[] values)
		{
			ArgumentNullException.ThrowIfNull(values);

			T[] result = new T[values.Length];

			for (int i = 0; i < values.Length; i++)
			{
				object value = values[i];

				if (value == null || value == DBNull.Value)
				{
					if (default(T) == null)
					{
						result[i] = default!;
					}
					else
					{
						throw new InvalidCastException($"Value at index {i} is null and cannot be converted to type {typeof(T).Name}.");
					}
				}
				else if (value is T typedValue)
				{
					result[i] = typedValue;
				}
				else
				{
					Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
					result[i] = (T)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
				}
			}

			return result;
		}

		/// <summary>
		/// Converts an array of objects to a strongly typed array based on the element type of the specified property
		/// descriptor.
		/// </summary>
		/// <remarks>All values are converted to the element type of the property descriptor using standard conversion
		/// logic. The method requires that the property descriptor represents an array property; otherwise, an exception is
		/// thrown.</remarks>
		/// <param name="values">The array of values to convert. Each element will be converted to the target element type of the property
		/// descriptor.</param>
		/// <param name="propertyDescriptor">The property descriptor that defines the target array type and element type for conversion. Must represent an
		/// array property.</param>
		/// <returns>An array containing the converted values, with each element of the target type defined by the property descriptor.
		/// The array will have the same length as the input values.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the specified property is not an array type or if the element type cannot be determined.</exception>
		public static Array ConvertArray(this object[] values, PropertyDescriptor propertyDescriptor)
		{
			ArgumentNullException.ThrowIfNull(values);
			ArgumentNullException.ThrowIfNull(propertyDescriptor);

			Type propertyType = propertyDescriptor.PropertyType;
			Type elementType = propertyType;

			if (!propertyType.IsArray)
			{
				throw new InvalidOperationException($"The property '{propertyDescriptor.Name}' is not an array type.");
			}

			elementType = propertyType.GetElementType()
				?? throw new InvalidOperationException($"Could not determine the element type for '{propertyDescriptor.Name}'.");

			Array result = Array.CreateInstance(elementType, values.Length);

			for (int i = 0; i < values.Length; i++)
			{
				object value = values[i];
				object convertedValue = ConvertValue(value, elementType);
				result.SetValue(convertedValue, i);
			}

			return result;
		}

		/// <summary>
		/// Converts the specified array of values to the element type of the given array property and assigns the resulting
		/// array to the property on the provided instance.
		/// </summary>
		/// <param name="values">The array of values to convert and assign to the property. Each value will be converted to the property's element
		/// type.</param>
		/// <param name="instance">The object instance whose property will be set. Cannot be null.</param>
		/// <param name="propertyInfo">The reflection information for the array property to assign. Must represent an array property and cannot be null.</param>
		/// <exception cref="InvalidOperationException">Thrown if the specified property is not an array type or if the element type of the property cannot be determined.</exception>
		public static Array ConvertArray(this object[] values, object instance, PropertyInfo propertyInfo)
		{
			ArgumentNullException.ThrowIfNull(instance);
			ArgumentNullException.ThrowIfNull(propertyInfo);
			ArgumentNullException.ThrowIfNull(values);

			Type propertyType = propertyInfo.PropertyType;

			if (!propertyType.IsArray)
			{
				throw new InvalidOperationException($"Property '{propertyInfo.Name}' is not an array type.");
			}

			Type elementType = propertyType.GetElementType()
				?? throw new InvalidOperationException($"Could not determine the element type for property '{propertyInfo.Name}'.");

			Array convertedArray = Array.CreateInstance(elementType, values.Length);

			for (int i = 0; i < values.Length; i++)
			{
				object convertedValue = ConvertValue(values[i], elementType);
				convertedArray.SetValue(convertedValue, i);
			}

			return convertedArray;
		}

		private static object ConvertValue(object value, Type targetType)
		{
			if (value == null || value == DBNull.Value)
			{
				if (!targetType.IsValueType || Nullable.GetUnderlyingType(targetType) != null)
				{
					return null;
				}

				throw new InvalidCastException($"Cannot convert null to {targetType.Name}.");
			}

			Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

			if (underlyingType.IsInstanceOfType(value))
			{
				return value;
			}

			if (underlyingType.IsEnum)
			{
				if (value is string stringValue)
				{
					return Enum.Parse(underlyingType, stringValue, true);
				}

				object enumValue = Convert.ChangeType(value, Enum.GetUnderlyingType(underlyingType), CultureInfo.InvariantCulture);
				return Enum.ToObject(underlyingType, enumValue);
			}

			return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
		}
	}
}