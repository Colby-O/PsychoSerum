using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Helpers
{
    internal static class ListExtension
    {
		public static void Shuffle<T>(this IList<T> ts) {
			int count = ts.Count;
			int last = count - 1;
			for (var i = 0; i < last; ++i) {
				int r = Random.Range(i, count);
				T tmp = ts[i];
				ts[i] = ts[r];
				ts[r] = tmp;
			}
		}
    }
}
