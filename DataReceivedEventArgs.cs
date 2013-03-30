using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(string data)
        {
            DataObject = new IrcDataObject(data);
        }

        public IrcDataObject DataObject { get; set; }
    }
}
