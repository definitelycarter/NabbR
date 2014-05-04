using System;
using System.Threading.Tasks;

namespace NabbR
{
    public interface IRoom
    {
        String Name { get; }
        Task<Boolean> SendMessage(String message);
    }
}
