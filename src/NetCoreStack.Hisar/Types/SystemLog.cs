using NetCoreStack.Contracts;
using System;

namespace NetCoreStack.Hisar
{
    public class SystemLog : EntityIdentityBson
    {
        public long? UserId { get; set; }

        public string Ip { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

        public string ErrorCode { get; set; }

        public int Level { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
