using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public static class IEnumerableExtensions
{

	public static IEnumerable<t> Randomize<t> (this IEnumerable<t> target)
	{
		Random r = new Random ();

		return target.OrderBy (x => (r.Next ()));
	}
}