using SharpEnd.Network;

namespace SharpEnd.Game.Scripting
{
    public class ScriptNpc : PlayerScriptInteraction
    {
        private int m_npcId;
        private MessageSequenceCache m_sequence;
        private volatile bool m_terminated;

        public ScriptNpc(Script script, GameClient client, int npcId)
            : base(script, client)
        {
            m_npcId = npcId;
            m_sequence = new MessageSequenceCache();
            m_terminated = false;
        }

        private static byte
            SAY = 0x00
            ;

        public void Say(string text)
        {

        }

        public void SayNext(string text)
        {

        }

        public byte AskYesNo(string text)
        {
            return 0;
        }
    }

    public class MessageSequenceCache
    {
        private Node m_first;
        private Node m_cursor;

        public void Add(string data, bool showNext)
        {
            Node insert = new Node(data, showNext);

            if (m_first == null)
            {
                m_first = insert;
            }
            else
            {
                m_cursor.m_forward = insert;

                insert.m_backward = m_cursor;
            }
        }

        public void Clear()
        {
            m_first = m_cursor = null;
        }

        public bool IsEmpty()
        {
            return m_cursor == null;
        }

        public string GoBackwardAndGet()
        {
            m_cursor = m_cursor.m_backward;

            return m_cursor.m_data;
        }

        public string GoForwardAndGet()
        {
            m_cursor = m_cursor.m_forward;

            return m_cursor.m_data;
        }

        public bool ShowNext()
        {
            return m_cursor.m_showNext;
        }

        public bool HasBack()
        {
            return m_cursor != null && m_cursor.m_backward != null;
        }

        public bool HasNext()
        {
            return m_cursor != null && m_cursor.m_forward != null;
        }

        private class Node
        {
            public Node m_backward;
            public string m_data;
            public bool m_showNext;
            public Node m_forward;

            public Node(string data, bool showNext)
            {
                m_data = data;
                m_showNext = showNext;
            }
        }
    }
}
