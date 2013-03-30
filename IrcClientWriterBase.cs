using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public abstract class IrcClientWriterBase
    {
        protected readonly StreamWriter _writer;
        protected readonly IIrc _network;

        protected IrcClientWriterBase(StreamWriter writer, IIrc network)
        {
            _network = network;
            _writer = writer;
        }

        public abstract void WriteLine(string message);
    }
}
