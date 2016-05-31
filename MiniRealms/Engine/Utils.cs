using System;

namespace MiniRealms.Engine
{
    public static class Utils
    {
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string SpacesPushleft(string message, int width, int start = 0, int offset = 0)
        {
            var s = message;

            var spaces = (width - (s.Length + start));

            for (var i = 0; i < spaces - offset; i++)
            {
                s = " " + s;
            }

            return s;
        }

        public static string SpacesCenter(string message, int width, int start = 0, int offset = 0)
        {
            var spaces = (width - message.Length);
            spaces = width - spaces;

            for (var i = 0; i < spaces - offset; i++)
            {
                message = " " + message;
            }

            return message;
        }

        public static string SpacesPushright(string message, int width, int start = 0, int offset = 0)
        {
            var s = message;

            var spaces = (width - (s.Length + start));

            for (var i = 0; i < spaces + offset; i++)
            {
                s = s + " ";
            }

            return s;
        }
    }
}
