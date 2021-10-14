using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;

namespace BETSI
{
    public partial class Form1 : Form
    {
        private SpeechRecognitionEngine engine;
        private CultureInfo ci;
        public string BETSI_NAME = "BETSI";


        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            try
            {
                ci = new CultureInfo("pt_BR"); //Language
                engine = new SpeechRecognitionEngine(ci); // All voice recognition it'll be in PT_BR
                OutputSound.Speak("Carregando...");
                SpeechRec();

                OutputSound.Speak("Sistema carregado");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Init() ERROR, in line 26"); // Error in initialize the function Init();
            }
        }

        private void setText(string text)
        {
            this.labelText.Text = "Você: " + text;
        }

        private void setColor(string cor)
        {
            switch (cor)
            {
                case "green":
                    this.labelStatus.BackColor = Color.Green;
                    break;
                case "red":
                    this.labelStatus.BackColor = Color.Red;
                    break;
                default:
                    this.labelStatus.BackColor = Color.Black;
                    break;
            }
        }

        private List<Grammar> LoadGrammar()
        {
            List<Grammar> GrammarToSpeak = new List<Grammar>();

            #region Choices

            Choices CommandsOfSystem = new Choices(); // Choice

            CommandsOfSystem.Add(Grammars.What_Time_is_It.ToArray()); // List of grammars for Time(Hour)
            CommandsOfSystem.Add(Grammars.Which_todays_date.ToArray()); // List of grammars Date


            Choices Calculator = new Choices();
            Calculator.Add(Grammars.How_much_is_it.ToArray());

            Choices NValues = new Choices();
            NValues.Add(Grammars.Numbers.ToArray());

            Choices Operators = new Choices();
            Operators.Add(Grammars.Operators.ToArray());

            Choices SS = new Choices();
            SS.Add(Grammars.System_Stats.ToArray());

            #endregion

            #region GrammerBuilder

            GrammarBuilder CommandsOfSystem_GB = new GrammarBuilder();
            CommandsOfSystem_GB.Append(CommandsOfSystem);

            GrammarBuilder C_Calculator = new GrammarBuilder();
            C_Calculator.Append(Calculator);
            C_Calculator.Append(NValues);
            C_Calculator.Append(Operators);
            C_Calculator.Append(NValues);

            GrammarBuilder System_S = new GrammarBuilder();
            System_S.Append(SS);

            #endregion

            #region Grammar

            Grammar GrammarOfSystem = new Grammar(CommandsOfSystem_GB); // Require Grammar Builder 
            GrammarOfSystem.Name = "Sysem";

            Grammar G_Calc = new Grammar(C_Calculator);
            G_Calc.Name = "calc";

            Grammar SS_0 = new Grammar(System_S);
            SS_0.Name = "Betsi";

            #endregion

            GrammarToSpeak.Add(GrammarOfSystem);
            GrammarToSpeak.Add(G_Calc);
            GrammarToSpeak.Add(SS_0);

            return GrammarToSpeak;
        }

        private void SpeechRec()
        {
            try
            {
                List<Grammar> g = LoadGrammar();


                /// Region (IDE exclusive) ///
                #region Speech Recognition (Events)

                    engine.SetInputToDefaultAudioDevice();

                foreach(Grammar gr in g)
                {
                    engine.LoadGrammar(gr);
                }

                    engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Rec); // Capture the voice input
                    engine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(AudioLevel); // Update and processing the voice input
                    engine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(Rejected); // When the voice input doesen't understood

                #endregion

                engine.RecognizeAsync(RecognizeMode.Multiple); // Initialize Recognize (Is possible, overwrite speak)
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "SpeechRec() ERROR, in line 39"); // Error in initialize the function SpeechRec();
            }
        }

        #region Events of recognize
        private void Rec(object s, SpeechRecognizedEventArgs e) // Enter if recognized
        {
            string speak = e.Result.Text;

            setText(speak);
            setColor("green");

            switch (e.Result.Grammar.Name)
            {
                case "Sysem":
                    // All inside this's grammar of system

                    if (Grammars.What_Time_is_It.Any(f => f == speak)) // If what is said matches the system database in Grammars.cs
                    {
                        Executer.GetHours();
                    }
                    else if (Grammars.Which_todays_date.Any(f => f == speak))
                    {
                        Executer.GetDate();
                    }
                    else
                    {
                        return;
                    }

                    break;

                case "calc":
                    OutputSound.Speak(Calculator.Calc(speak));
                    break;

                case "Betsi":
                    if (Grammars.System_Stats.Any(f => f == speak))
                    {
                        Executer.System_Stats_Calc();
                    }
                    break;
            }
        }

        private void Rejected(object s, SpeechRecognitionRejectedEventArgs e) // voice input not understood
        {
            string speak = e.Result.Text;

            setText("-----------");
            setColor("red");
        }

        private void AudioLevel(object s, AudioLevelUpdatedEventArgs e) // Recognize the voice input
        {
            if (e.AudioLevel > AudioBar.Maximum) // If over Audio input
            {
                AudioBar.Value = AudioBar.Maximum; // Audio input dont over, normalizet
            }
            else if (e.AudioLevel < AudioBar.Minimum) // If vacuo input
            {
                AudioBar.Value = AudioBar.Minimum; // Audio input normalizet
            }
            else // If normal
            {
                AudioBar.Value = e.AudioLevel; // Is normal
            }


        }

        #endregion





        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
