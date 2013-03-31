using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public abstract class IrcClientWriterBase : IDisposable
    {
        private StreamWriter _writer;
        private IIrc _network;

        protected IrcClientWriterBase(StreamWriter writer, IIrc network)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (network == null)
            {
                throw new ArgumentNullException("network");
            }

            _network = network;
            _writer = writer;

            _writer.AutoFlush = true;
        }

        protected internal StreamWriter Writer
        {
            get { return _writer; }
        }

        protected internal IIrc Network
        {
            get { return _network; }
        }

        [System.ComponentModel.Localizable(false)]
        public abstract void WriteLine(string message);

        public void Dispose()
        {
            _writer.Flush();
            _writer.Dispose();
            _writer = null;
            GC.SuppressFinalize(this);
        }
    }
}
