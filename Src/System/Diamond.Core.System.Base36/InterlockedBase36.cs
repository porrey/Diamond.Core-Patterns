namespace System
{
	/// <summary>
	/// Represents a thread-safe wrapper for a Base36 value that can be incremented or added to atomically.
	/// </summary>
	public sealed class InterlockedBase36
	{
		private int value;

		/// <summary>
		/// Initializes a new instance of the <see cref="InterlockedBase36"/> class with the specified starting value.
		/// </summary>
		/// <param name="startValue">The initial value of the Base36 counter.</param>
		public InterlockedBase36(Base36 startValue)
		{
			this.value = startValue.Value;
		}

		/// <summary>
		/// Gets the current value of the Base36 counter in a thread-safe manner.
		/// </summary>
		public Base36 Current
		{
			get
			{
				return new Base36(Volatile.Read(ref this.value));
			}
		}

		/// <summary>
		/// Atomically increments the Base36 counter by one and returns the new value.
		/// </summary>
		/// <returns>The new value of the Base36 counter after the increment.</returns>
		public Base36 Increment()
		{
			int nextValue = Interlocked.Increment(ref this.value);

			return new Base36(nextValue);
		}
		
		/// <summary>
		/// Atomically adds the specified amount to the Base36 counter and returns the new value.
		/// </summary>
		/// <param name="amount">The amount to add to the Base36 counter.</param>
		/// <returns>The new value of the Base36 counter after the addition.</returns>
		public Base36 Add(int amount)
		{
			int nextValue = Interlocked.Add(ref this.value, amount);

			return new Base36(nextValue);
		}
	}
}