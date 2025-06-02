using System;

namespace ZenithVirtualAssistant.Models
{
    public class CommandHistory
    {
        public int Id { get; set; }
        public string Command { get; set; }
        public string Response { get; set; }
        public DateTime Timestamp { get; set; }
    }
}