using System.Collections.Generic;

namespace DarkRoom.Core
{
	/// <summary>
	/// 简单的优先级队列, 内部用list来存储数据
	/// </summary>
	public class CPriorityQueue<T> where T : IPriority
	{
		private List<T> m_innerList = null;

		public CPriorityQueue()
		{
			m_innerList = new List<T>(4);
        }

		public int Count
		{
			get { return m_innerList.Count; }
		}

		public void Clear()
		{
			m_innerList.Clear();
		}

		public void Push(T item)
		{
			m_innerList.Add(item);
		}

		public T Pop()
		{
			if (m_innerList.Count <= 0) return default(T);

			Sort();
			T item = m_innerList[0];
			m_innerList.RemoveAt(0);
			return item;
		}

		public bool Contains(T item)
		{
			return m_innerList.Contains(item);
		}

		private void Sort()
		{
			m_innerList.Sort(Compare);
		}

		private int Compare(T x, T y)
		{
            if (x.GetPriority() > y.GetPriority())return 1;
			if (x.GetPriority() < y.GetPriority())return -1;
			return 0;
		}
	}


	public interface IPriority
	{
		float GetPriority();
	}
}