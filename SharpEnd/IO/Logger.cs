using System;
using System.Text;

namespace SharpEnd
{
    internal enum ELogType : byte
    {
        Info,
        Warning,
        Error,
        Debug,
    }

    internal static class Log
    {
        private const byte LabelWidth = 11;

        private static string Margin
        {
            get
            {
                return new string(' ', LabelWidth);
            }
        }

        public static void Entitle()
        {
            SkipLine();

            Console.WriteLine(" > SharpEnd v1.0");

            SkipLine();
        }

        public static void Inform(string value, params object[] args)
        {
            WriteItem("Info", ConsoleColor.White, value, args);
        }

        public static void Warn(string value, params object[] args)
        {
            WriteItem("Warning", ConsoleColor.Yellow, value, args);
        }

        public static void Error(Exception exception)
        {
            Error(exception.ToString());
        }

        public static void Error(string value, params object[] args)
        {
            WriteItem("Error", ConsoleColor.Red, value, args);
        }

        public static void Success(string value, params object[] args)
        {
            WriteItem("Success", ConsoleColor.Green, value, args);

            Log.SkipLine();
        }

        public static void SkipLine()
        {
            Console.WriteLine();
        }

        private static void WriteItem(string label, ConsoleColor labelColor, string value, params object[] args)
        {
            lock (typeof(Log))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(' ', LabelWidth - label.Length - 3);
                sb.Append("[");
                sb.Append(label);
                sb.Append("]");
                sb.Append(" ");

                label = sb.ToString();
                value = string.Format(value, args);

                Console.ForegroundColor = labelColor;
                Console.Write(label);
                Console.ForegroundColor = ConsoleColor.Gray;

                bool first = true;

                foreach (string s in value.Split('\n'))
                {
                    string[] lines = new string[(int)Math.Ceiling((float)s.Length / (float)(Console.BufferWidth - Log.LabelWidth))];

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i == lines.Length - 1)
                        {
                            lines[i] = s.Substring((Console.BufferWidth - LabelWidth) * i);
                        }
                        else
                        {
                            lines[i] = s.Substring((Console.BufferWidth - LabelWidth) * i, (Console.BufferWidth - LabelWidth));
                        }
                    }

                    foreach (string line in lines)
                    {
                        if (!first)
                        {
                            Console.Write(Margin);
                        }

                        if ((line.Length + LabelWidth) < Console.BufferWidth)
                        {
                            Console.WriteLine(line);
                        }
                        else if ((line.Length + LabelWidth) == Console.BufferWidth)
                        {
                            Console.Write(line);
                        }

                        first = false;
                    }
                }
            }
        }
    }
}
