# Zenith Virtual Assistant

A C# desktop virtual assistant for Windows 11, built with WPF and .NET 8.0.

## Features
- Runs as a background process with system tray integration.
- Voice command recognition with "Zenith wake up" and "Zenith" prefix.
- AI integration using local Ollama instance.
- System control (open apps, terminals, shutdown).
- Widget mode with system monitoring (CPU, RAM, disk).
- Local SQLite database for command history.
- Text-to-Speech (TTS) for responses.
- Modern, simple WPF-based UI.

## Prerequisites
- Visual Studio 2022 with .NET 8.0 SDK.
- Ollama installed locally (`ollama pull llama3`).
- SQLite NuGet package.
- Git Bash and WSL (optional for terminal support).

## Setup
1. Clone the repository.
2. Open in Visual Studio 2022.
3. Install NuGet packages: `Microsoft.Data.Sqlite`, `System.Speech`, `System.Text.Json`.
4. Ensure Ollama is running (`ollama run llama3`).
5. Build and run the project.

## Usage
- Say "Zenith wake up" to activate the assistant.
- Use commands like "Zenith open notepad", "Zenith shutdown", or "Zenith what is the weather?".
- Right-click the system tray icon to show the main window, widget, or exit.