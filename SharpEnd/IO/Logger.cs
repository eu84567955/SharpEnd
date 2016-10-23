using System;
using System.Text;

namespace SharpEnd
{
    public enum ELogType : byte
    {
        Info,
        Warning,
        Error,
        Debug,
    }

    public static class Log
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
            Console.Title = "SharpEnd";

            SkipLine();

            Console.WriteLine(" > SharpEnd (MapleStory {0} v.{1}.{2})", Application.Version.Localisation.ToString(), Application.Version.Version, Application.Version.Patch);

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
            Error(true ? exception.ToString() : exception.Message); // TODO: Exception stack trace configuration.
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

        public static void Hex(string label, byte[] value, params object[] args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(label, args));
            sb.Append('\n');

            if (value == null || value.Length == 0)
            {
                sb.Append("(Empty)");
            }
            else
            {
                int lineSeparation = 0;

                foreach (byte b in value)
                {
                    if (lineSeparation == 16)
                    {
                        sb.Append('\n');
                        lineSeparation = 0;
                    }

                    sb.AppendFormat("{0:X2} ", b);
                    lineSeparation++;
                }
            }

            Log.WriteItem("Hex", ConsoleColor.Magenta, sb.ToString());
        }

        public static void Hex(string label, byte b, params object[] args)
        {
            Log.Hex(label, new byte[] { b }, args);
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
