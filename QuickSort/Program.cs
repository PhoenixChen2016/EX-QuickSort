using System;
using System.Collections.Generic;
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

		/// <summary>
		/// 合併兩個已經排好序的List
		/// </summary>
		/// <param name="left">左側List</param>
		/// <param name="right">右側List</param>
		/// <returns></returns>
		static List<int> merge(Queue<int> left, Queue<int> right)
		{
			List<int> temp = new List<int>();
			while (left.Count > 0 && right.Count > 0)
			{
				if (left.Peek() <= right.Peek())
				{
					temp.Add(left.Dequeue());
				}
				else
				{
					temp.Add(right.Dequeue());
				}
			}
			if (left.Count > 0)
			{
				temp.AddRange(left.ToArray());
			}
			if (right.Count > 0)
			{
				temp.AddRange(right.ToArray());
			}
			return temp;
		}

		static List<int> Merge(List<List<int>> lists)
		{
			if (lists.Count <= 1)
				return lists.FirstOrDefault();

			var tasks = new List<Task<List<int>>>();

			var length = lists.Count >> 1 << 1;
			for (var i = 0; i < length; i += 2)
			{
				var index = i;
				var left = new Queue<int>(lists[index]);
				var right = new Queue<int>(lists[index + 1]);
				tasks.Add(Task.Run(() => merge(left, right)));
			}

			Task.WaitAll(tasks.ToArray());

			var result = tasks.Select(t => t.Result).ToList();
			if (lists.Count % 2 > 0)
				result.Add(lists.Last());

			return Merge(result);
		}

		static void Main(string[] args)
		{
			var a = Enumerable.Range(0, 1000_00000)
				.Select(n => random.Next())
				.ToArray();

			var sw = Stopwatch.StartNew();
			//QuickSort(a, 0, a.Length - 1);

			var tasks = new List<Task>();
			for (var i = 0; i < 100; i++)
			{
				var index = i;
				tasks.Add(Task.Run(() => QuickSort(a, index * 1000000, index * 1000000 + 1000000 - 1)));
			}

			Task.WaitAll(tasks.ToArray());

			var numberGroups = new List<List<int>>();
			for (var i = 0; i < 100; i++)
				numberGroups.Add(a.Skip(i * 1000000).Take(1000000).ToList());

			var result = Merge(numberGroups);

			sw.Stop();

			Console.WriteLine(sw.ElapsedMilliseconds);

			//foreach (var n in a)
			//	Console.WriteLine(n);
		}
	}
}
