using NabbR.ViewModels.Chat;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NabbR.Services
{
    public interface IJabbRContext
    {
        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        String UserId { get; }
        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <value>
        /// The rooms.
        /// </value>
        ObservableCollection<RoomViewModel> Rooms { get; }
        /// <summary>
        /// Logs in the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<Boolean> LoginAsync(String username, String password);
        Task<Boolean> SendMessage(String message, String roomName);
    }
}
