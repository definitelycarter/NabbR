
namespace NabbR.Events
{
    public interface IHandle<T>
    {
        void Handle(T message);
    }
}
