using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public class IrcUser
    {
        /// <summary>
        /// Gets or sets the Nickname.
        /// </summary>
        /// <value>The Nickname.</value>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>The UserName.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the HostName.
        /// </summary>
        /// <value>The HostName.</value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the Network.
        /// </summary>
        /// <value>The Network.</value>
        public IIrc Network { get; private set; }

        /// <summary>
        /// News from string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IrcUser NewFromString(string source)
        {
            return NewFromString(source, null);
        }

        /// <summary>
        /// New user from string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="network">The Network.</param>
        /// <returns></returns>
        public static IrcUser NewFromString(string source, IIrc network)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (network == null)
            {
                throw new ArgumentNullException("network");
            }

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
            catch (IndexOutOfRangeException)
            {
                //TODO: do soemthing;
            }

            var ret = new IrcUser
                           {
                               HostName = host,
                               Nickname = nick,
                               UserName = user,
                               Network = network,
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

            if (this.Nickname != null)
                endResult = this.Nickname;

            if (this.UserName != null)
            {
                endResult += "!" + this.UserName;
            }
            if (this.HostName != null)
            {
                endResult += "@" + this.HostName;
            }

            return endResult;
        }


    }
}
