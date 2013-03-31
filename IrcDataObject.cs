using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpmebot.Irc
{
    public class IrcDataObject
    {
        private readonly string _data;

        private IrcUser _prefix ;
        private string _command ;
        private string[] _args ;

        public IrcDataObject(string data)
        {
            this._data = data;

            if (!Parse())
            {
                throw new ArgumentException("Could not parse message: " + data);
            }
        }

        public IrcUser Prefix
        {
            get { return _prefix; }
        }

        public string Command
        {
            get { return _command; }
        }

        public string[] Arguments
        {
            get { return _args; }
        }

        private bool Parse()
        {
            if (String.IsNullOrEmpty(_data))
            {
                _prefix = null;
                _command = null;
                _args = null;
                return false;
            }


            var raw = _data;

            if (raw[0] == ':') // prefix is present if the first char is a :
            {
                string[] split1 = raw.Split(new[] {' '}, 2);
                    //TODO: check there'rawData a space, and this returns two items;
                _prefix = IrcUser.NewFromString(split1[0]);
                raw = split1[1];

            }
            else
            {
                _prefix = null;
            }

            // next is a command, with an OPTIONAL(!) list of arguments following, up to 15. We're gonna assume infinite for the purposes of this code

            if (!raw.Contains(" "))
            {
                _command = raw;
                _args = new string[0];
                return true;
            } // ok, there'rawData arguments

            var split2 = raw.Split(new[] {' '}, 2);
            _command = split2[0];
            raw = split2[1];


            // handle the arguments
            {
                var xargs = new List<string>();

                if (raw.Contains(" :"))
                {
                    // split on first instance of " :"
                    var split3 = raw.Split(new[] {" :"}, 2, StringSplitOptions.None);
                    var split4 = split3[0].Split(' ');

                    xargs.AddRange(split4);
                    xargs.Add(split3[1]);
                }
                else
                {
                    var split5 = raw.Split(' ');
                    xargs.AddRange(split5);
                }

                _args = xargs.ToArray();

            }

            return true;
        }


        public override string ToString()
        {
            return _data;
        }
    }
}