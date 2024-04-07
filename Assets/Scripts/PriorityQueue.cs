using System.Collections;
using System.Collections.Generic;
public class PriorityQueue<T>
{
    private List<T> heap;
    private Comparer<T> comparer;

    public int Count
    {
        get { return heap.Count; }
    }

    public PriorityQueue()
    {
        heap = new List<T>();
        comparer = Comparer<T>.Default;
    }

    public void Enqueue(T item)
    {
        heap.Add(item);
        int i = heap.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (comparer.Compare(heap[parent], heap[i]) <= 0)
                break;

            // Swap the elements
            T temp = heap[i];
            heap[i] = heap[parent];
            heap[parent] = temp;

            i = parent;
        }
    }

    public T Dequeue()
    {
        if (heap.Count == 0)
            throw new System.InvalidOperationException("Priority queue is empty");

        T min = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        int i = 0;
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left < heap.Count && comparer.Compare(heap[left], heap[smallest]) < 0)
                smallest = left;

            if (right < heap.Count && comparer.Compare(heap[right], heap[smallest]) < 0)
                smallest = right;

            if (smallest == i)
                break;

            // Swap the elements
            T temp = heap[i];
            heap[i] = heap[smallest];
            heap[smallest] = temp;

            i = smallest;
        }

        return min;
    }

    public T Peek()
    {
        if (heap.Count == 0)
            throw new System.InvalidOperationException("Priority queue is empty");

        return heap[0];
    }
}