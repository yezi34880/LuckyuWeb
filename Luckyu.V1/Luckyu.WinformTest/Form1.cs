using Luckyu.Utility;
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

        #region 转语音
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


        #endregion

        #region 缩略图
        private void button2_Click(object sender, EventArgs e)
        {
            int width = 200;
            using (var imgBmp = new Bitmap(@"C:\Users\YY\Downloads\1111.jpeg"))
            {
                //找到新尺寸
                int oWidth = imgBmp.Width;
                int oHeight = imgBmp.Height;
                int height = oHeight;
                if (width > oWidth)
                {
                    width = oWidth;
                }
                else
                {
                    height = width * oHeight / oWidth;
                }
                var newImg = new Bitmap(imgBmp, width, height);
                //保存到本地
                newImg.Save(@"C:\Users\YY\Downloads\2222.jpeg");
                newImg.Dispose();
            }
        }


        #endregion

        #region 测试动态加载程序集
        private void button3_Click(object sender, EventArgs e)
        {
            var str = ClassTest.DynamicLoad.GetTestString();
            MessageBox.Show(str);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var name = @"D:\MyProject\OpenSource\LuckyuWeb\Luckyu.V1\Luckyu.ClassTest\bin\Debug\netcoreapp3.1\Luckyu.ClassTest.dll";
            System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(name);
        }
        #endregion

    }
}
