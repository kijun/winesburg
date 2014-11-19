﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<TKey, TElement>
{
    private SortedDictionary<TKey, Queue<TElement>> dictionary = new SortedDictionary<TKey, Queue<TElement>>();
    private HashSet<TElement> items = new HashSet<TElement>();

    
    public void Enqueue(TKey key, TElement item)
    {
        Queue<TElement> queue;
        if (!dictionary.TryGetValue(key, out queue))
        {
            queue = new Queue<TElement>();
            dictionary.Add(key, queue);
        }
        
        queue.Enqueue(item);
        items.Add(item);
    }
    
    public TElement Dequeue()
    {
        if (dictionary.Count == 0)
            throw new Exception("No items to Dequeue:");
        var key = dictionary.Keys.First();
        
        var queue = dictionary[key];
        var output = queue.Dequeue();
        if (queue.Count == 0)
            dictionary.Remove(key);

        items.Remove(output);
        return output;
    }

    public bool ContainsValue(TElement item) {
        return items.Contains(item);
    }

    public int Count {
        get {
            return items.Count;
        }
    }
}