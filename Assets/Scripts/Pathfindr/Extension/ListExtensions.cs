using System.Collections.Generic;
using System.Linq;

public static class ListExtensions 
{
	// check if list has been initialised and populated

	public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
	{
		if(enumerable == null)
		{
			return true;
		}

		return !enumerable.Any();
	}
}