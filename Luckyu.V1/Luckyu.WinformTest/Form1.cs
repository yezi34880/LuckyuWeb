using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luckyu.WinformTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSpeak_Click(object sender, EventArgs e)
        {
            var txt = textBox1.Text;
            if (string.IsNullOrWhiteSpace(txt))
            {
                return;
            }
            using (var synthesizer = new SpeechSynthesizer())
            {
                var voices = synthesizer.GetInstalledVoices();
                synthesizer.Speak(txt);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var txt = textBox1.Text;
            if (string.IsNullOrWhiteSpace(txt))
            {
                return;
            }
            var dialog = new SaveFileDialog();
            dialog.Filter = "音频文件(*.mp3)|*.mp3|所有文件(*.*)|*.*";
            dialog.FileName = $"{DateTime.Now.Ticks}.mp3";
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (var synthesizer = new SpeechSynthesizer())
            {
                synthesizer.SetOutputToWaveFile(dialog.FileName);
                synthesizer.Speak(txt);
                synthesizer.SetOutputToDefaultAudioDevice();
            }
        }
    }
}
