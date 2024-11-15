namespace Fushigi.ui
{
    public interface IRevertable
    {
        string Name { get; }

        IRevertable Revert();
    }
}
