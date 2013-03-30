using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace helpmebot.Irc
{
    public class IrcClient : IIrc
    {
        private readonly string _server;
        private readonly short _port;
        private string _nickname;
        private readonly string _username;
        private readonly string _realname;
        private readonly string _password;

        protected IrcClient()
        {

        }

        public IrcClient(string server, short port = 6667, string nickname = "IrcUser", string username = "user", string realname = "An IRC Client", string password = null)
        {
            _server = server;
            _port = port;
            _nickname = nickname;
            _realname = realname;
            _username = username;
            _password = password;
        }


    }
}
