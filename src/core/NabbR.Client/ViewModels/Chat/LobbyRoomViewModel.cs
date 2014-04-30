using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NabbR.ViewModels.Chat
{
    public class LobbyRoomViewModel : RoomViewModelBase
    {
        private Int32 count;
        private Boolean closed;

        public Int32 Count
        {
            get { return this.count; }
            set { this.Set(ref this.count, value); }
        }
        public Boolean Closed
        {
            get { return this.closed; }
            set { this.Set(ref this.closed, value); }
        }
    }
}
