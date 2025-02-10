using System.Collections.Generic;

namespace Sprint0
{
    public class CommandRequest
    {
        public string CommandKey { get; }
        public Dictionary<string, object> Parameters { get; }

        public CommandRequest(string commandKey, Dictionary<string, object> parameters)
        {
            CommandKey = commandKey;
            Parameters = parameters;
        }
    }
}