namespace SharpEnd.Utility
{
    public sealed class Doublet<T1, T2>
    {
        public T1 First;
        public T2 Second;

        public Doublet(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}
