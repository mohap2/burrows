using System;
using System.Collections.Generic;

namespace PQ
{
    /// <summary>Implements a priority queue of T, where T has an ordering.</summary>
    /// Elements may be added to the queue in any order, but when we pull
    /// elements out of the queue, they will be returned in 'ascending' order.
    /// Adding new elements into the queue may be done at any time, so this is
    /// useful to implement a dynamically growing and shrinking queue. Both adding
    /// an element and removing the first element are log(N) operations. 
    /// 
    /// The queue is implemented using a priority-heap data structure. For more 
    /// details on this elegant and simple data structure see "Programming Pearls"
    /// in our library. The tree is implemented atop a list, where 2N and 2N+1 are
    /// the child nodes of node N. The tree is balanced and left-aligned so there
    /// are no 'holes' in this list. 
    /// http://stackoverflow.com/a/1937896/1499431
    /// <typeparam name="T">Type T, should implement IComparable[T];</typeparam>
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> mA = new List<T>();

        /// <summary>Clear all the elements from the priority queue</summary>
        public void Clear()
        {
            mA.Clear();
        }

        /// <summary>Add an element to the priority queue - O(log(n)) time operation.</summary>
        /// <param name="item">The item to be added to the queue</param>
        public void Add(T item)
        {
            int n = mA.Count;
            mA.Add(item);
            while (n != 0)
            {
                int p = (n - 1) / 2;    // This is the 'parent' of this item
                if (mA[n].CompareTo(mA[p]) >= 0) break;  // Item >= parent
                T tmp = mA[n]; mA[n] = mA[p]; mA[p] = tmp; // Swap item and parent
                n = p;            // And continue
            }
        }

        /// <summary>Returns the number of elements in the queue.</summary>
        public int Count
        {
            get { return mA.Count; }
        }

        /// <summary>Returns true if the queue is empty.</summary>
        /// Trying to call Peek() or Pop() on an empty queue will throw an exception.
        /// Check using Empty first before calling these methods.
        public bool Empty
        {
            get { return mA.Count == 0; }
        }

        /// <summary>Allows you to look at the first element waiting in the queue, without removing it.</summary>
        /// This element will be the one that will be returned if you subsequently call Pop().
        public T Peek()
        {
            if (Empty) throw new InvalidOperationException("The priority queue is empty.");
            return mA[0];
        }

        /// <summary>Removes and returns the first element from the queue (least element)</summary>
        /// <returns>The first element in the queue, in ascending order.</returns>
        public T Pop()
        {
            if (Empty) throw new InvalidOperationException("The priority queue is empty.");
            T val = mA[0];
            int nMax = mA.Count - 1;
            mA[0] = mA[nMax];
            mA.RemoveAt(nMax);  // Move the last element to the top

            int p = 0;
            while (true)
            {
                // c is the child we want to swap with. If there
                // is no child at all, then the heap is balanced
                int c = p * 2 + 1;
                if (c >= nMax) break;

                // If the second child is smaller than the first, that's the one
                // we want to swap with this parent.
                if (c + 1 < nMax && mA[c + 1].CompareTo(mA[c]) < 0) c++;
                // If the parent is already smaller than this smaller child, then
                // we are done
                if (mA[p].CompareTo(mA[c]) <= 0) break;

                // Otherwise, swap parent and child, and follow down the parent
                T tmp = mA[p]; mA[p] = mA[c]; mA[c] = tmp;
                p = c;
            }
            return val;
        }
    }
}
