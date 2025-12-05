using System;
using System.IO;
using TagLib;

namespace TunedIn.Models
{
    public class AvaloniaFileAbstraction : TagLib.File.IFileAbstraction
    {
        private readonly Stream _readStream;

        public AvaloniaFileAbstraction(string path, Stream readStream)
        {
            Name = path;
            _readStream = readStream;
        }

        string TagLib.File.IFileAbstraction.Name => Name;

        Stream TagLib.File.IFileAbstraction.ReadStream => _readStream;

        Stream TagLib.File.IFileAbstraction.WriteStream => throw new NotSupportedException("Writing is not supported");

        public string Name { get; }

        public Stream ReadStream => _readStream;

        public Stream WriteStream => throw new NotSupportedException("Writing is not supported");

        void TagLib.File.IFileAbstraction.CloseStream(Stream stream)
        {
            // Let caller manage stream disposal
        }

        public void CloseStream(Stream stream)
        {
            // Let caller manage stream disposal
        }
    }
}