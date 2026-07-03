namespace System
{
	/// <summary>
	/// Represents a thread-safe wrapper for a Base36 value that can be incremented or added to atomically.
	/// </summary>
	public sealed class InterlockedBase36
	{
		/// <summary>
		/// The underlying integer value representing the Base36 counter.
		/// </summary>
		private int _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="InterlockedBase36"/> class with the specified starting value.
		/// </summary>
		/// <param name="startValue">The initial value of the Base36 counter.</param>
		public InterlockedBase36(Base36 startValue)
		{
			//
			// Store the initial value as an integer for atomic operations.
			//
			this._value = startValue.Value;
		}

		/// <summary>
		/// Gets the current value of the Base36 counter in a thread-safe manner.
		/// </summary>
		public Base36 Current
		{
			get
			{
				//
				// Volatile.Read is used to ensure that the read operation is atomic and thread-safe.
				//
				return new Base36(Volatile.Read(ref this._value));
			}
		}

		/// <summary>
		/// Atomically increments the Base36 counter by one and returns the new value.
		/// </summary>
		/// <returns>The new value of the Base36 counter after the increment.</returns>
		public Base36 Increment()
		{
			//
			// Interlocked.Increment is used to ensure that the increment operation is atomic and thread-safe.
			//
			int nextValue = Interlocked.Increment(ref this._value);

			//
			// Return a new Base36 instance with the updated value.
			//
			return new Base36(nextValue);
		}
		
		/// <summary>
		/// Atomically adds the specified amount to the Base36 counter and returns the new value.
		/// </summary>
		/// <param name="amount">The amount to add to the Base36 counter.</param>
		/// <returns>The new value of the Base36 counter after the addition.</returns>
		public Base36 Add(int amount)
		{
			//
			// Interlocked.Add is used to ensure that the addition operation is atomic and thread-safe.
			//
			int nextValue = Interlocked.Add(ref this._value, amount);

			//
			// Return a new Base36 instance with the updated value.
			//
			return new Base36(nextValue);
		}
	}
}