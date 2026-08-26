using System;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<TElement, TPriority>
{
    public int Count { get; private set; }
    private readonly SortedDictionary<TPriority, Queue<TElement>> backing_dict;

    public PriorityQueue()
    {
        backing_dict = new SortedDictionary<TPriority, Queue<TElement>>();
        Count = 0;
    }


    public void Enqueue(TElement element, TPriority priority)
    {
        if(!backing_dict.TryGetValue(priority, out Queue<TElement> queue))
        {
            queue = new();
            backing_dict[priority] = queue;
        }

        queue.Enqueue(element);
        Count++;
    }

    public TElement Dequeue()
    {
        if(Count == 0)
            throw new InvalidOperationException("Cannot dequeue when empty");

        KeyValuePair<TPriority, Queue<TElement>> highest_priority_pair = backing_dict.First();
        Queue<TElement> highest_priority_queue = highest_priority_pair.Value;
        TPriority highest_priority_key = highest_priority_pair.Key;

        TElement highest_priority_element = highest_priority_queue.Dequeue();
        Count--;
        
        if(highest_priority_queue.Count == 0)
            backing_dict.Remove(highest_priority_key);

        return highest_priority_element;
    }
}