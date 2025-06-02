using System.Diagnostics;

namespace ZenithVirtualAssistant.Services
{
    public class SystemControlService
    {
        public string ExecuteCommand(string command)
        {
            try
            {
                if (command.Contains("open notepad"))
                {
                    Process.Start("notepad.exe");
                    return "Opening Notepad";
                }
                else if (command.Contains("open cmd"))
                {
                    Process.Start("cmd.exe");
                    return "Opening Command Prompt";
                }
                else if (command.Contains("open powershell"))
                {
                    Process.Start("powershell.exe");
                    return "Opening PowerShell";
                }
                else if (command.Contains("open wsl"))
                {
                    Process.Start("wsl.exe");
                    return "Opening WSL";
                }
                else if (command.Contains("open gitbash"))
                {
                    Process.Start("C:\\Program Files\\Git\\git-bash.exe");
                    return "Opening Git Bash";
                }
                else if (command.Contains("open terminal"))
                {
                    Process.Start("wt.exe"); // Windows Terminal
                    return "Opening Windows Terminal";
                }
                return "Unknown command";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}