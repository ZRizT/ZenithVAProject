using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace ZenithVirtualAssistant.Services
{
    public class SpeechService : IDisposable
    {
        private readonly SpeechRecognitionEngine _recognizer;
        private readonly SpeechSynthesizer _synthesizer;
        private readonly OllamaService _ollamaService;
        private readonly SystemControlService _systemService;
        private readonly DatabaseService _dbService;
        private bool _isListeningForCommand;

        public SpeechService()
        {
            _recognizer = new SpeechRecognitionEngine();
            _synthesizer = new SpeechSynthesizer();
            _ollamaService = new OllamaService();
            _systemService = new SystemControlService();
            _dbService = new DatabaseService();

            // Set up speech recognition
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            // Load grammar for "Zenith" wake word
            var wakeGrammar = new Grammar(new GrammarBuilder("Zenith"));
            _recognizer.LoadGrammar(wakeGrammar);

            // Load grammar for commands after wake word
            var commandChoices = new Choices("wake up", "open notepad", "open terminal", "open cmd", "open powershell", "open wsl", "open gitbash");
            var commandGrammar = new Grammar(new GrammarBuilder(new GrammarBuilder("Zenith") { Culture = System.Globalization.CultureInfo.InvariantCulture }, commandChoices));
            _recognizer.LoadGrammar(commandGrammar);
        }

        public void StartListening()
        {
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private async void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string command = e.Result.Text.ToLower();

            if (command == "zenith")
            {
                _isListeningForCommand = true;
                Speak("I'm listening. What's your command?");
                return;
            }

            if (!_isListeningForCommand) return;

            _isListeningForCommand = false;

            string response = "";
            if (command == "zenith wake up")
            {
                response = await _ollamaService.ProcessCommand("Initialize AI assistant");
                Speak(response);
            }
            else if (command.StartsWith("zenith open "))
            {
                response = _systemService.ExecuteCommand(command);
                Speak(response);
            }
            else
            {
                response = await _ollamaService.ProcessCommand(command);
                Speak(response);
            }

            // Log to database
            _dbService.LogCommand(command, response);
        }

        public void Speak(string text)
        {
            _synthesizer.SpeakAsync(text);
        }

        public void Dispose()
        {
            _recognizer.Dispose();
            _synthesizer.Dispose();
        }
    }
}