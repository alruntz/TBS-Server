using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TBSEngine
{
    // Message pathern (int)<typeRoom>|(int)<roomID>|(string)<function:value1;value2>

    public class SocketMessage
    {
        public class FunctionSocketMessage
        {
            public string functionName;
            public List<string> parameters;

            public FunctionSocketMessage(string functionName, List<string> parameters)
            {
                this.functionName = functionName;
                this.parameters = parameters;
            }
        }

        public FunctionSocketMessage FunctionMessage => m_functionMessage;
        public string Message => m_message;

        private readonly FunctionSocketMessage m_functionMessage;
        private readonly string m_message;

        public SocketMessage(FunctionSocketMessage functionMessage)
        {
            m_functionMessage = functionMessage;
            m_message = GetString();
        }

        private string GetString()
        {
            string str = m_functionMessage.functionName;

            if (m_functionMessage.parameters != null && m_functionMessage.parameters.Count > 0)
            {
                str += ":";
                for (int i = 0; i < m_functionMessage.parameters.Count; i++)
                {
                    str += FunctionMessage.parameters[i];
                    if (i < FunctionMessage.parameters.Count - 1)
                        str += ';';
                }
            }

            return str;
        }

        public static SocketMessage Parse(string message)
        {
            string[] splitted = null;
            List<string> parameters = null;
            if ((splitted = message.Split(':')) != null && splitted.Length == 2)
            {
                parameters = splitted[1].Split(';').ToList();
                return new SocketMessage(new FunctionSocketMessage(splitted[0], parameters));
            }

            return new SocketMessage(new FunctionSocketMessage(message, null));
        }
    }
}
