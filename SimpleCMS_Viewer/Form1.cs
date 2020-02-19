using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SimpleCMSDBDLL;
using Steema.TeeChart.Styles;

namespace SimpleCMS_Viewer
{
    public partial class Form1 : Form
    {
        const string connectionString = @"Server=.;database=SimpleCMSDB;uid=sa;password=rootroot;";
        LineDrawer lineDrawWave;
        LineDrawer lineDrawFFT;
        List<LineDrawer> trendList;

        public Form1()
        {
            InitializeComponent();

            trendList = new List<LineDrawer>();
            lineDrawWave = new LineDrawer(tChartWave, new Steema.TeeChart.Styles.Line());
            lineDrawFFT = new LineDrawer(tChartFFT, new Steema.TeeChart.Styles.Line());
            chartStartTime.ShowUpDown = true;
            chartStartTime.Format = DateTimePickerFormat.Custom;
            chartStartTime.CustomFormat = "M'월' d'일' H'시' m'분' s'초'";
            chartEndTime.ShowUpDown = true;
            chartEndTime.Format = DateTimePickerFormat.Custom;
            chartEndTime.CustomFormat = "M'월' d'일' H'시' m'분' s'초'";

            tChartTrends.ClickSeries += trendChart_ClickSeries;

            using (var db = new SimpleCmsDBClassDataContext(connectionString))
            {
                if (!db.DatabaseExists())
                {
                    MessageBox.Show("database does not exist");
                    return;
                }



                // 채널 갯수에 따라 TabControl 확장 예정
                var channels = db.Channel.Select(channel => channel).FirstOrDefault();
                //tabPage1.Text = channels.name;
                tabControl.TabPages[0].Text = channels.name;

                foreach (var item in DBController.GetAllTrendType())
                {
                    trendListBox.Items.Add(item.name);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tChartTrends.ShowEditor();
        }

        private void tChartTrends_Click(object sender, EventArgs e)
        {
        }
        private void trendChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var clickedTime = DateTime.FromOADate(s.XValues[valueIndex]);
                DrawWaveAndSpecturm(clickedTime);
            }
        }
            public void DrawWaveAndSpecturm(DateTime trendTime)
        {
            using (var db = new SimpleCmsDBClassDataContext(connectionString))
            {
                //WaveData wave = new WaveData();
                var wave = db.WaveData.Where(w => DateTime.Compare(w.time, trendTime) == 0).FirstOrDefault();
                wave.ToFloatArray();
                Spectrum spectrum = new Spectrum();
                spectrum.GetFFT(wave);

                lineDrawWave.DrawLine(wave, true);
                lineDrawFFT.DrawLine(spectrum.fft, true);
            }
        }

        private void btn_GetRange_Click(object sender, EventArgs e)
        {
            //tChartTrends.Clear();
            foreach (var item in trendListBox.SelectedItems)
            {
                var trendArray = DBController.GetTrendDatas(item.ToString(), chartStartTime.Value, chartEndTime.Value);
                Line line = new Line();
                tChartTrends.Series.Add(line);
                line.Add(trendArray.Select(w=>w.Time).ToArray(), trendArray.Select(w=>(float) w.Value).ToArray());
            }
        }

        private void chartStartTime_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                SendKeys.Send("{UP}");
            }
            else
            {
                SendKeys.Send("{DOWN}");
            }
        }
        private void chartEndTime_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                SendKeys.Send("{UP}");
            }
            else
            {
                SendKeys.Send("{DOWN}");
            }
        }
    }
}
