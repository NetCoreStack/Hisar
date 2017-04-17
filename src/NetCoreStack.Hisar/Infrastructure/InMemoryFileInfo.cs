using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace NetCoreStack.Hisar
{
    public class InMemoryFileInfo : IFileInfo
    {
        private long? _length;

        public bool Exists => true;

        public long Length
        {
            get
            {
                if (!_length.HasValue)
                {
                    _length = Stream.Length;
                }
                return _length.Value;
            }
        }

        public string PhysicalPath { get; }

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public bool IsDirectory => false;

        public MemoryStream Stream { get; }

        public InMemoryFileInfo(string name, string physicalPath, byte[] bytes, DateTimeOffset lastModified)
        {
            Name = name;
            PhysicalPath = physicalPath;
            Stream = new MemoryStream(bytes);
            LastModified = lastModified;
        }

        public Stream CreateReadStream()
        {
            return Stream;
        }
    }
}
