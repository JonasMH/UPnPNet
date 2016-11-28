using System.Collections.Generic;

namespace UPnPNet.Extensions
{
	public static class DictionaryExtensions
	{
		public static IDictionary<TKey, TValue> AddInline<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			dictionary.Add(key, value);
			return dictionary;
		}
	}
}