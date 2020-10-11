namespace Nord.Client.Engine
{
    public interface IEquatableByRef<T>
    {
        bool Equals(ref T other);
    }
}
