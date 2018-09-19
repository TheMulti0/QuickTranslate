namespace EasyTranslate.Translators
{
    internal class ExternalKey
    {
        public long Time { get; }
        public long Value { get; }

        public ExternalKey(long time, long value)
        {
            Time = time;
            Value = value;
        }
    }
}