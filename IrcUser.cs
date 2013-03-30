using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using helpmebot.Irc;

namespace Helpmebot.Irc
{
    public class IrcUser
    {
        /// <summary>
        /// Gets or sets the nickname.
        /// </summary>
        /// <value>The nickname.</value>
        public string nickname { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string username { get; set; }

        /// <summary>
        /// Gets or sets the hostname.
        /// </summary>
        /// <value>The hostname.</value>
        public string hostname { get; set; }

        /// <summary>
        /// Gets or sets the network.
        /// </summary>
        /// <value>The network.</value>
        public IIrc network { get; private set; }

        /// <summary>
        /// News from string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IrcUser newFromString(string source)
        {
            return newFromString(source, null);
        }

        /// <summary>
        /// New user from string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="network">The network.</param>
        /// <returns></returns>
        public static IrcUser newFromString(string source, IIrc network)
        {
            string user, host;
            string nick = user = host = null;
            try
            {
                if ((source.Contains("@")) && (source.Contains("!")))
                {
                    char[] splitSeparators = {'!', '@'};
                    string[] sourceSegment = source.Split(splitSeparators, 3);
                    nick = sourceSegment[0];
                    user = sourceSegment[1];
                    host = sourceSegment[2];
                }
                else if (source.Contains("@"))
                {
                    char[] splitSeparators = {'@'};
                    string[] sourceSegment = source.Split(splitSeparators, 2);
                    nick = sourceSegment[0];
                    host = sourceSegment[1];
                }
                else
                {
                    nick = source;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                //TODO: do soemthing;
            }

            IrcUser ret = new IrcUser
                           {
                               hostname = host,
                               nickname = nick,
                               username = user,
                               network = network,
                           };
            return ret;
        }

        /// <summary>
        ///   Recompiles the source string
        /// </summary>
        /// <returns>nick!user@host, OR nick@host, OR nick</returns>
        public override string ToString()
        {

            string endResult = string.Empty;

            if (this.nickname != null)
                endResult = this.nickname;

            if (this.username != null)
            {
                endResult += "!" + this.username;
            }
            if (this.hostname != null)
            {
                endResult += "@" + this.hostname;
            }

            return endResult;
        }


    }
}
