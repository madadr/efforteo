using System;

namespace Efforteo.Common.Exceptions
{
    public class EfforteoException : Exception
    {
        public string Code { get; }

        public EfforteoException()
        {
        }

        public EfforteoException(string code)
        {
            Code = code;
        }

        public EfforteoException(string code, string message) : base(message)
        {
            Code = code;
        }
    }
}