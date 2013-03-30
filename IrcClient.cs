using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Helpmebot.Irc
{
    public class IrcClient : IIrc
    {
        private readonly string _server;
        private readonly ushort _port;
        private string _nickname;
        private readonly string _username;
        private readonly string _realname;
        private readonly string _password;
        private TcpClient _socket;
        private Type _ircWriterType = typeof(NaiveIrcClientWriter);
        private IrcClientWriterBase _ircWriter;
        private StreamReader _ircReader;

        public Type IrcWriterType
        {
            get { return _ircWriterType; }
            set
            {
                if (_socket == null)
                {
                    if (value.IsSubclassOf(typeof(IrcClientWriterBase)))
                    {
                        _ircWriterType = value;
                    }
                    else throw new ArgumentOutOfRangeException("value");
                }
                else
                {
                    throw new NotSupportedException("Changing active writer after connection is not supported.");
                }
            }
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        protected IrcClient()
        {
             
        }

        public IrcClient(string server, ushort port = 6667, string nickname = "IrcUser", string username = "user",
                         string realname = "An IRC Client", string password = null) : this()
        {
            if (string.IsNullOrEmpty(server))
            {
                throw new ArgumentNullException("server");
            }
            _server = server;

            if (port < 1 || port > 65535)
            {
                throw new ArgumentNullException("port");
            }
            _port = port;

            if (string.IsNullOrEmpty(nickname))
            {
                throw new ArgumentNullException("nickname");
            }
            _nickname = nickname;

            if (string.IsNullOrEmpty(realname))
            {
                throw new ArgumentNullException("realname");
            }
            _realname = realname;

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }
            _username = username;

            _password = string.IsNullOrEmpty(server) ? null : password;
            
        }

        public void Connect()
        {
            if (_socket != null)
            {
                return; // do nothing since we're already connected/
            }

            try
            {
                _socket = new TcpClient(_server, _port);
                var socketStream = _socket.GetStream();

                _ircWriter =
                    (IrcClientWriterBase) Activator.CreateInstance(_ircWriterType, new StreamWriter(socketStream, Encoding.UTF8), this);

                _ircReader = new StreamReader(socketStream, Encoding.UTF8);

                var readerThread = new Thread(this.readerThread);
                readerThread.Start();

            }
            catch (SocketException ex)
            {
                throw; // TODO: handle nicely.
            }
        }

        private void readerThread()
        {
            try
            {
                while (_socket.Connected)
                {
                    string line = _ircReader.ReadLine();
                    EventHandler<DataReceivedEventArgs> temp = DataReceived;
                    if (temp != null)
                    {
                        temp(this, new DataReceivedEventArgs(line));
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                
            }
        }
    }
}
