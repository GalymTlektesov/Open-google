using System;
using System.Reflection; //Для ассемблера(тоже для автозагрузки)
using Microsoft.Win32;//Для автозагрузки
using System.Windows.Forms;
using Microsoft.Speech.Recognition;//Для голового управления
//using OpenQA.Selenium;//Для открытие браузера
using System.Diagnostics;//Для открытия хрома по ссылке

namespace Потом_удаить
{
    public partial class Form1 : Form
    {
       // static IWebDriver browser;

        public Form1()
        {
            InitializeComponent();
        }

        private bool SetAutorunValue(bool autorun, string puth)
            //Автозагурузка
        {
            const string name = "systems";
            string Exeputh = puth;
            RegistryKey reg;
                reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                try
                {
                    if (autorun)
                    {
                        reg.SetValue(name, Exeputh);
                    }
                    else
                    {
                        reg.DeleteValue(name);
                    }
                reg.Flush();
                reg.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
            //Автозагрузка
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetAutorunValue(true, Assembly.GetExecutingAssembly().Location);//Вызов автозагрузки
            MessageBox.Show("Open google", "ative", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static void sre_SpeachRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //голосовой вывод
            if (e.Result.Confidence > 0.82)
            {
                try
                {
                    Process.Start("https://www.google.kz/");
                    /*browser = new OpenQA.Selenium.Chrome.ChromeDriver();//отрыть хром
                    browser.Manage().Window.Maximize();//хром на весь экран
                    browser.Navigate().GoToUrl("https://www.google.kz/");//открыть хром*/
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            //голосовой вывод
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //Голосовой ввод
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();

            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeachRecognized);


            Choices google = new Choices();
            google.Add(new string[] { "Откройся гугл", "Окей гугл" });


            GrammarBuilder gb = new GrammarBuilder();
            //gb.Culture = ci;
            gb.Append(google);


            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            //Голосовой ввод
        }

    }
}
