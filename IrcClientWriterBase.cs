using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public abstract class IrcClientWriterBase : IDisposable
    {
        protected readonly StreamWriter _writer;
        protected readonly IIrc _network;

        protected IrcClientWriterBase(StreamWriter writer, IIrc network)
        {
            _network = network;
            _writer = writer;

            _writer.AutoFlush = true;
        }

        public abstract void WriteLine(string message);

        public void Dispose()
        {
            _writer.Flush();
            _writer.Dispose();
        }
    }
}
