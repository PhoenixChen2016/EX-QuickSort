using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuickSort
{
	class Program
	{
		static Random random = new Random();

		static void QuickSort(int[] items, int start, int end)
		{
			if (end <= start)
				return;

			var pivotIndex = random.Next(start, end + 1);

			(items[pivotIndex], items[end]) = (items[end], items[pivotIndex]);

			var middle = start;
			var pivotValue = items[end];
			for (var i = start; i < end; i++)
			{
				if (items[i] < pivotValue)
				{
					(items[i], items[middle]) = (items[middle], items[i]);
					middle++;
				}
			}

			(items[middle], items[end]) = (items[end], items[middle]);

			QuickSort(items, start, middle - 1);
			QuickSort(items, middle + 1, end);
		}

		static void Main(string[] args)
		{
			var a = Enumerable.Range(0, 1000000)
				.Select(n => random.Next(1000))
				.ToArray();
			var sw = Stopwatch.StartNew();
			QuickSort(a, 0, a.Length - 1);
			sw.Stop();

			Console.WriteLine(sw.ElapsedMilliseconds);

			//foreach (var n in a)
			//	Console.WriteLine(n);
		}
	}
}
