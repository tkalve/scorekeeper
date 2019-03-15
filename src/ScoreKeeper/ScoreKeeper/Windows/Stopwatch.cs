using System;

namespace ScoreKeeper.Windows
{
    public class Stopwatch : System.Diagnostics.Stopwatch
    {
        TimeSpan _offset = new TimeSpan();

        public Stopwatch()
        {
        }

        public Stopwatch(TimeSpan offset)
        {
            _offset = offset;
        }

        public void Reset()
        {
            base.Reset();
            _offset = new TimeSpan();
        }

        public void SetOffset(TimeSpan offsetElapsedTimeSpan)
        {
            _offset = _offset.Add(offsetElapsedTimeSpan);
        }

        public TimeSpan Elapsed
        {
            get { return base.Elapsed + _offset; }
            set { _offset = value; }
        }

        public long ElapsedMilliseconds
        {
            get { return base.ElapsedMilliseconds + _offset.Milliseconds; }
        }

        public long ElapsedTicks
        {
            get { return base.ElapsedTicks + _offset.Ticks; }
        }
    }
}