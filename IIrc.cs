using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public interface IIrc
    {
        Type IrcWriterType { get; set; }
        void Connect();
    }
}
