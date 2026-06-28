
namespace tz.cleverence.ru.Services.StaticServer
{
    public static class StaticServer
    {
        private static int _count = 0;

        private static readonly object _lock = new object();
        private static int _activeReaders = 0;
        private static int _waitingWriters = 0;
        private static bool _isWriting = false;

        public static int GetCount()
        {
            lock(_lock)
            {
                while(_isWriting || _waitingWriters > 0)
                {
                    Monitor.Wait(_lock);
                }
                _activeReaders++;
            }

            try
            {
                Thread.Sleep(250);
                return _count;
            }
            finally
            {
                lock(_lock)
                {
                    _activeReaders--;
                    if(_activeReaders == 0)
                    {
                        Monitor.PulseAll(_lock);
                    }
                }

            }
        }

        public static void AddToCount(int value)
        {
            lock(_lock)
            {
                _waitingWriters++;

                while(_activeReaders > 0 || _isWriting)
                {
                    Monitor.Wait(_lock);
                }

                _waitingWriters--;
                _isWriting = true;
            }

            try
            {
                Thread.Sleep(250);
                _count += value;
            }
            finally
            {
                lock(_lock)
                {
                    _isWriting = false;
                    Monitor.PulseAll(_lock);
                }
            }
        }
    }
}
