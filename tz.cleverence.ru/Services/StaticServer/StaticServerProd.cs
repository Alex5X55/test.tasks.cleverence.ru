
namespace tz.cleverence.ru.Services.StaticServer
{
    public static class StaticServerProd
    {
        private static int _countProd = 0;

        private static readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        public static int GetCountProd()
        {
            _rwLock.EnterReadLock();
            try
            {
                return _countProd;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public static void AddToCountProd(int value)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _countProd += value;
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

    }
}
