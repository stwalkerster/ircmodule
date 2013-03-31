using System.IO;

namespace Helpmebot.Irc
{
    public class NaiveIrcClientWriter : IrcClientWriterBase
    {
        public NaiveIrcClientWriter(StreamWriter writer, IIrc network) : base(writer, network)
        {
        }

        public override void WriteLine(string message)
        {
            Writer.WriteLine(message);
        }
    }
}