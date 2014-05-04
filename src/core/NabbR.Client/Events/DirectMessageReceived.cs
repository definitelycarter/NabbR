using System;

namespace NabbR.Events
{
    public class DirectMessageReceived
    {
        public String From { get; set; }
        public String To { get; set; }
        public String Message { get; set; }
    }
}
