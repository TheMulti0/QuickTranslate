using System;

namespace EasyTranslate.Exceptions
{
    public class DetectFailedException : Exception
    {
        public DetectFailedException(
            string message = "Detect Operation Failed.")
            : base(message)
        {
        }

        public DetectFailedException(
            Exception inner,
            string message = "Detect Operation Failed")
            : base(message, inner)
        {
        }
    }
}