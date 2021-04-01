using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SystemThreadingChannels_Sample
{
    public class CustomChannel<T>
    {
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(0);

        public void Write(T item)
        {
            _queue.Enqueue(item);
            _sem.Release();
        }

        public async Task<T> ReadAsync()
        {
            await _sem.WaitAsync();
            _queue.TryDequeue(out T item);
            return item;
        }
    }
}
