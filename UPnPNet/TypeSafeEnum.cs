using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UPnPNet
{
	public abstract class TypeSafeEnum<T, TValue> where T: TypeSafeEnum<T, TValue> where TValue : IEquatable<TValue>
	{
		public TValue Value { get; }

		private static readonly IList<T> _modes = new List<T>();
		public static IReadOnlyCollection<T> Modes { get; } = new ReadOnlyCollection<T>(_modes);

		protected static T GetByValueInternal(TValue value, T random)
		{
			if (Modes.Count == 0)
			{
				//If we do not use a T as a argument or value, the super class may not be loaded fully yet
				//So it should never get to this point, but just in case
				TValue fix = random.Value;
			}
			return Modes.FirstOrDefault(x => x.Value.Equals(value));
		}

		protected TypeSafeEnum(TValue value)
		{
			Value = value;
			_modes.Add((T)this);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}