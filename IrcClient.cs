using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

[assembly: InternalsVisibleTo("TestHarness")]

namespace Helpmebot.Irc
{
    public class IrcClient : IIrc
    {
        private readonly string _server;
        private readonly ushort _port;
        private string _nickname;
        private readonly string _userName;
        private readonly string _realName;
        private readonly string _password;
        private TcpClient _socket;
        private Type _ircWriterType = typeof (NaiveIrcClientWriter);
        private IrcClientWriterBase _ircWriter;
        private StreamReader _ircReader;

        public Type IrcWriterType
        {
            get { return _ircWriterType; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (_socket == null)
                {
                    if (value.IsSubclassOf(typeof (IrcClientWriterBase)))
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

        internal event EventHandler<DataReceivedEventArgs> DataReceived;

        protected IrcClient()
        {

        }


        public IrcClient(string server, ushort port, string nickname, string userName,
                         string realName)
            : this(server, port, nickname, userName, realName, null)
        {
        }

        public IrcClient(string server, ushort port, string nickname, string userName,
                         string realName, string password)
            : this()
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

            if (string.IsNullOrEmpty(realName))
            {
                throw new ArgumentNullException("realName");
            }
            _realName = realName;

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }
            _userName = userName;

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
                    (IrcClientWriterBase)
                    Activator.CreateInstance(_ircWriterType, new StreamWriter(socketStream, Encoding.UTF8), this);

                _ircReader = new StreamReader(socketStream, Encoding.UTF8);

                var readerThread = new Thread(this.readerThread);
                readerThread.Start();

                DataReceived += IrcClient_DataReceived;
            }
            catch (SocketException)
            {
                throw; // TODO: handle nicely.
            }
        }

        private static void IrcClient_DataReceived(object sender, DataReceivedEventArgs e)
        {
            // do some basic handling now
            
            if (e.DataObject.Command == "PING")
            {
                e.Network.Pong(e.DataObject.Arguments[0]);
            }
            
        }

        protected virtual void RegisterConnection()
        {
            Pass();
            Nick();
            User();
        }

        protected void Pass()
        {
            if (_password != null)
            {
                _ircWriter.WriteLine("PASS " + _password);
            }
        }

        protected void User()
        {
            _ircWriter.WriteLine("USER " + _userName + " * * :" + _realName);
        }

        protected void Nick()
        {
            _ircWriter.WriteLine("NICK " + _nickname);
        }

        protected void Nick(string nickname)
        {
            _ircWriter.WriteLine("NICK " + nickname);
            _nickname = nickname;
        }

        internal void Pong(string s)
        {
            _ircWriter.WriteLine("PONG :" + s);
        }

        private void readerThread()
        {
            try
            {
                while (_socket.Connected)
                {
                    string line = _ircReader.ReadLine();
                    if (line == null)
                    {
                        _socket.Close();
                        continue;
                    }

                    EventHandler<DataReceivedEventArgs> temp = DataReceived;
                    if (temp != null)
                    {
                        temp(this, new DataReceivedEventArgs(line, this));
                    }
                }
            }
            catch (ThreadAbortException)
            {

            }
        }
    }
}