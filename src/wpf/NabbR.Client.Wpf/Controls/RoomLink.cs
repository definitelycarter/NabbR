using FirstFloor.ModernUI.Presentation;
using System;

namespace NabbR.Controls
{
    class RoomLink : Link
    {
        private String roomName;
        private Int32 unreadCount;

        public String RoomName
        {
            get { return this.roomName; }
            set
            {
                if (this.roomName != value)
                {
                    this.roomName = value;
                    this.UpdateDisplayName();
                }
            }
        }

        public Int32 UnreadCount
        {
            get { return this.unreadCount; }
            set
            {
                if (this.unreadCount != value)
                {
                    this.unreadCount = value;
                    this.UpdateDisplayName();
                }
            }
        }

        private void UpdateDisplayName()
        {
            if (this.UnreadCount > 0)
            {
                this.DisplayName = String.Format("{0} ({1:n0})", this.roomName, this.unreadCount);
            }
            else
            {
                this.DisplayName = this.RoomName;
            }
        }
    }
}
