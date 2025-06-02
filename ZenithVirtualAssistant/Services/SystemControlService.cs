using System;
using System.Diagnostics;

namespace ZenithVirtualAssistant.Services
{
    public class SystemControlService
    {
        public void ExecuteCommand(string command)
        {
            command = command.ToLower().Trim();
            if (command.StartsWith("open "))
            {
                string target = command.Substring(5).Trim();
                OpenApplicationOrTerminal(target);
            }
            else if (command == "shutdown")
            {
                Process.Start("shutdown", "/s /t 0");
            }
        }

        private void OpenApplicationOrTerminal(string target)
        {
            try
            {
                switch (target)
                {
                    case "notepad":
                        Process.Start("notepad.exe");
                        break;
                    case "cmd":
                        Process.Start("cmd.exe");
                        break;
                    case "powershell":
                        Process.Start("powershell.exe");
                        break;
                    case "gitbash":
                        Process.Start("C:\\Program Files\\Git\\git-bash.exe");
                        break;
                    case "wsl":
                        Process.Start("wsl.exe");
                        break;
                    default:
                        Process.Start(target);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to open {target}: {ex.Message}");
            }
        }
    }
}