using JabbR.Client.Models;
using NabbR.ViewModels.Chat;

namespace NabbR
{
    static class JabbRExtensions
    {
        public static RoomViewModel AsViewModel(this Room room)
        {
            return new RoomViewModel
            {
                 Name = room.Name,
                 Topic = room.Topic
            };
        }

        public static UserViewModel AsViewModel(this User user)
        {
            return new UserViewModel
            {
                Name = user.Name,
                Hash = user.Hash,
                Note = user.Note,
                Status = user.Status,
            };
        }

        public static LobbyRoomViewModel AsLobbyRoomViewModel(this Room room)
        {
            return new LobbyRoomViewModel
            {
                Name = room.Name,
                Topic = room.Topic,
                Count = room.Count,
                Closed = room.Closed,
            };
        }

        public static MessageViewModel AsViewModel(this Message message)
        {
            return new MessageViewModel
            {
                Content = message.Content,
                MessageDateTime = message.When,
                HtmlEncoded = message.HtmlEncoded,
            };
        }

        public static UserMessageViewModel AsUserMessageViewModel(this Message message, UserViewModel user)
        {
            return new UserMessageViewModel
            {
                User = user,
                Content = message.Content,
                MessageDateTime = message.When,
                HtmlEncoded = message.HtmlEncoded,
            };

        }
    }
}
