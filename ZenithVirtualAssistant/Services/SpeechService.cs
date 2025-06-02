using System;
using System.IO;

#nullable enable
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text.Json;

namespace ZenithVirtualAssistant.Services
{
    public class SpeechService
    {
        private readonly SpeechRecognitionEngine _recognizer;
        private readonly SpeechSynthesizer _synthesizer;
        private readonly string _wakeUpCommand;
        private readonly string _commandPrefix;
        private bool _isAwake;
        private readonly OllamaService _ollamaService;

        public event Action<string>? OnCommandRecognized;

        public SpeechService(string configPath, OllamaService ollamaService)
        {
            _ollamaService = ollamaService;
            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));
            _wakeUpCommand = config.WakeUpCommand;
            _commandPrefix = config.CommandPrefix;

            _recognizer = new SpeechRecognitionEngine();
            _synthesizer = new SpeechSynthesizer();
            InitializeRecognizer();
        }

        private void InitializeRecognizer()
        {
            var grammar = new Grammar(new GrammarBuilder(new Choices(_wakeUpCommand, $"{_commandPrefix} *")));
            _recognizer.LoadGrammar(grammar);
            _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string command = e.Result.Text.ToLower();
            if (command == _wakeUpCommand)
            {
                _isAwake = true;
                Speak("Zenith is awake. How can I assist you?");
            }
            else if (_isAwake && command.StartsWith(_commandPrefix))
            {
                string actualCommand = command.Substring(_commandPrefix.Length).Trim();
                OnCommandRecognized?.Invoke(actualCommand);
            }
        }

        public void Speak(string text)
        {
            _synthesizer.SpeakAsync(text);
        }

        public void Stop()
        {
            _recognizer.RecognizeAsyncStop();
            _synthesizer.SpeakAsyncCancelAll();
        }

        private class Config
        {
            public string OllamaEndpoint { get; set; }
            public string ModelName { get; set; }
            public string WakeUpCommand { get; set; }
            public string CommandPrefix { get; set; }
        }
    }
}