using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace SharpEnd.Game.Scripting
{
    public sealed class Script
    {
        private readonly string m_contents;
        private readonly ScriptEngine m_engine;
        private readonly ScriptScope m_scope;

        public Script(string contents)
        {
            m_contents = contents;
            m_engine = Python.CreateEngine();
            m_scope = m_engine.CreateScope();
        }

        public bool ContainsVariable(string name)
        {
            return m_scope.ContainsVariable(name);
        }

        public dynamic GetVariable(string name)
        {
            return m_scope.GetVariable(name);
        }

        public void SetVariable(string name, object value)
        {
            m_scope.SetVariable(name, value);
        }

        public void Execute()
        {
            m_engine.Execute(m_contents, m_scope);
        }
    }
}
