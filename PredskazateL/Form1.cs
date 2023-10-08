using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace PredskazateL
{
    public partial class Form1 : Form
    {
        private string APP_NAME = "ULTIMATE PREDICTOR";
        private readonly string PREDICTIONS_CONFIG_FILE = $"{Environment.CurrentDirectory}\\predictionCONFIG.json";
        private string[] _predictions; 
        private Random _random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private async void bPedict_Click(object sender, EventArgs e)
        {
            bPedict.Enabled = false; 
            await Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    this.Invoke(new Action(() =>
                    {
                        UpdateProgressBar(i);
                        this.Text = $"{i}%";
                    }));
                    Thread.Sleep(20);
                }
            });

            var index = _random.Next(0, _predictions.Length);
            var prediction = _predictions[index];
            MessageBox.Show($"{prediction}!");

            progressBar1.Value = 0;
            this.Text = APP_NAME;

            bPedict.Enabled = true;
        }

        private void UpdateProgressBar(int i)
        {
            if (i == progressBar1.Maximum)
            {
                progressBar1.Maximum = i + 1;
                progressBar1.Value = i + 1;
                progressBar1.Maximum = i;
            }
            else
            {
                progressBar1.Value = i + 1;
            }
            progressBar1.Value = i; //Модификация данных из другого потока
        }
        private void Form_Load(object sender, EventArgs e)
        {
            this.Text = APP_NAME;

            try
            {
                var data = File.ReadAllText(PREDICTIONS_CONFIG_FILE);
                _predictions = JsonConvert.DeserializeObject<string[]>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                if(_predictions == null)
                {
                    Close();
                }
                else if (_predictions.Length == 0)
                {
                    MessageBox.Show("На сегодня предсказания закончились");
                    Close();
                }
            }
        }
    }
}
