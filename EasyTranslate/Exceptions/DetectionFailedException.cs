using System;

namespace EasyTranslate.Exceptions
{
    public class DetectionFailedException : Exception
    {
        public DetectionFailedException(
            string message = "Detect Operation Failed.")
            : base(message)
        {
        }

        public DetectionFailedException(
            Exception inner,
            string message = "Detect Operation Failed")
            : base(message, inner)
        {
        }
    }
}