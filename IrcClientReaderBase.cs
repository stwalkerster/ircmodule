using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public abstract class IrcClientReaderBase
    {
        protected readonly StreamReader _reader;
        protected readonly IIrc _network;

        protected IrcClientReaderBase(StreamReader reader, IIrc network)
        {
            _network = network;
            _reader = reader;
        }

        public abstract string ReadLine();
    }
}