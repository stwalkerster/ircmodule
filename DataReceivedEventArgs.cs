using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public class DataReceivedEventArgs : EventArgs
    {
        private IrcClient _network;

        public DataReceivedEventArgs(string data, IrcClient network)
        {
            _network = network;
            DataObject = new IrcDataObject(data, _network);
            Handled = false;
        }

        public IrcDataObject DataObject { get; set; }

        public bool Handled { get; set; }

        public IrcClient Network
        {
            get { return _network; }
        }
    }
}
