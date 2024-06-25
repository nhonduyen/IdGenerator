namespace Snowflake
{
    public class SnowflakeIdGenerator
    {
        // Define the epoch start date (e.g., January 1, 2020)
        private static readonly DateTime EpochStart = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Bit allocations for each component
        private const int TimestampBits = 41;
        private const int MachineIdBits = 10;
        private const int SequenceBits = 12;

        // Maximum values for each component
        private const long MaxMachineId = (1L << MachineIdBits) - 1;
        private const long MaxSequence = (1L << SequenceBits) - 1;

        // Bit shifts for each component
        private const int MachineIdShift = SequenceBits;
        private const int TimestampShift = SequenceBits + MachineIdBits;

        private readonly object _lock = new object();

        private readonly long _machineId;
        private long _lastTimestamp = -1L;
        private long _sequence = 0L;

        public SnowflakeIdGenerator(long machineId)
        {
            if (machineId < 0 || machineId > MaxMachineId)
            {
                throw new ArgumentException($"Machine ID must be between 0 and {MaxMachineId}");
            }
            _machineId = machineId;
        }

        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = GetCurrentTimestamp();

                if (timestamp < _lastTimestamp)
                {
                    throw new InvalidOperationException("Clock moved backwards. Refusing to generate id.");
                }

                if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & MaxSequence;
                    if (_sequence == 0)
                    {
                        // Sequence exhausted, wait for next millisecond
                        timestamp = WaitForNextMillisecond(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0L;
                }

                _lastTimestamp = timestamp;

                return ((timestamp << TimestampShift) |
                       (_machineId << MachineIdShift) |
                       _sequence);
            }
        }

        private static long GetCurrentTimestamp()
        {
            return (long)(DateTime.UtcNow - EpochStart).TotalMilliseconds;
        }

        private static long WaitForNextMillisecond(long lastTimestamp)
        {
            var timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetCurrentTimestamp();
            }
            return timestamp;
        }
    }
}
