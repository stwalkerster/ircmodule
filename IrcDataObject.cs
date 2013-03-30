using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public class IrcDataObject
    {
        private string data;

        private IrcUser _prefix = null;
        private string _command = null;
        private string[] _args = null;

        public IrcDataObject(string data)
        {
            this.data = data;

            parse(data, out _prefix, out _command, out _args);
        }

        public static bool parse(string s, out IrcUser prefix, out string command, out string[] args)
        {
            var data = s;

            if (data[0] == ':') // prefix is present if the first char is a :
            {
                string[] split1 = data.Split(new[] {' '}, 2);//TODO: check there's a space, and this returns two items;
                prefix = IrcUser.newFromString(split1[0]);
                data = split1[1];

            }
            else
            {
                prefix = null;
            }

            // next is a command, with an OPTIONAL(!) list of arguments following, up to 15. We're gonna assume infinite for the purposes of this code

            if (!data.Contains(" "))
            {
                command = data;
                args = new string[0];
                return true;
            } // ok, there's arguments

            var split2 = data.Split(new[] {' '}, 2);
            command = split2[0];
            data = split2[1];


            // handle the arguments
            {
                var xargs = new List<string>();

                if (data.Contains(" :"))
                {
                    // split on first instance of " :"
                    var split3 = data.Split(new[] {" :"}, 2, StringSplitOptions.None);
                    var split4 = split3[0].Split(' ');

                    xargs.AddRange(split4);
                    xargs.Add(split3[1]);
                }
                else
                {
                    var split5 = data.Split(' ');
                    xargs.AddRange(split5);
                }

                args = xargs.ToArray();

            }

            return true;
        }


        public override string ToString()
        {
            return data;
        }
    }
}