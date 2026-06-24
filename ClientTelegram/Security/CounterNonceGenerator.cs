using System.Buffers.Binary;

namespace ClientTelegram.Security
{
    /// <summary>
    /// Singleton, all account take from this generator
    /// the nonce, the key is shared to all account
    /// </summary>
    public class CounterNonceGenerator
    {
        private long _counter;
        private long _reservedUpTo;
        private readonly object _lock = new object();
        private readonly Func<int, long> _reserveBlock;
        /// <summary>
        /// How many counter values to reserve from the DB in one go.
        /// Smaller = less waste on restart, more frequent DB round-trips.
        /// </summary>
        private const int BlockSize = 100;

        public CounterNonceGenerator(Func<int, long> reserveBlock)
        {
            _reserveBlock = reserveBlock;
        }


        public byte[] Next()
        {
            long value;
            lock (_lock)
            {
                if (_counter >= _reservedUpTo)
                {
                    long start = _reserveBlock(BlockSize);
                    _counter = start;
                    _reservedUpTo = start + BlockSize;
                }
                //use current value and generate next value for next nonce
                value = _counter++; 
            }
            //convert number in 12 byte (nonce dimension)
            byte[] nonce = new byte[12];
            BinaryPrimitives.WriteInt64BigEndian(nonce.AsSpan(4, 8), value);
            return nonce;
        }
    }
}
