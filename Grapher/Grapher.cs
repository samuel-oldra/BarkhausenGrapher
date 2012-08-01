using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Grapher
{
    public partial class Grapher : Form
    {
        #region Constructors

        public Grapher(string[] args)
        {
            if (DateTime.Now > new DateTime(2013, 06, 01)) return;

            bool legend = false;
            bool serie01 = false;
            bool serie02 = false;
            string filePath = string.Empty;
            if (args.Length >= 1)
                filePath = args[0];
            if (args.Length >= 2)
            {
                serie01 = args[1] == "/S1";
                serie02 = args[1] == "/S2";
                legend = args[1] == "/LEGEND";
            }

            InitializeComponent();

            var chartArea01 = new ChartArea("ChartArea 01");
            Chart.ChartAreas.Add(chartArea01);

            var legend01 = new Legend("Legend 01");
            if (legend) Chart.Legends.Add(legend01);

            // SERIE 01
            var series01 = new Series("Series 01");
            series01.ChartType = SeriesChartType.FastLine;
            series01.ChartArea = chartArea01.Name;
            series01.Legend = legend01.Name;
            series01.Color = Color.Blue;

            // SERIE 02
            var series02 = new Series("Series 02");
            series02.ChartType = SeriesChartType.FastLine;
            series02.ChartArea = chartArea01.Name;
            series02.Legend = legend01.Name;
            series02.Color = Color.Red;

            int count = 1;
            if (!string.IsNullOrEmpty(filePath))
            {
                using (var reader = new StreamReader(filePath))
                {
                    string linha = string.Empty;
                    while ((linha = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(linha)) continue;
                        string[] colunas = linha.Split('\t');

                        if (colunas.Length < 3) continue;
                        double pri = Convert.ToDouble(colunas[1].Replace('.', ','));
                        double sec = Convert.ToDouble(colunas[2].Replace('.', ','));

                        series01.Points.Add(new DataPoint(count, pri));
                        series02.Points.Add(new DataPoint(count, sec));
                        count++;
                    }
                }
            }

            if (serie01)
                Chart.Series.Add(series01);
            else if (serie02)
                Chart.Series.Add(series02);
            else
            {
                Chart.Series.Add(series01);
                Chart.Series.Add(series02);
            }
        }

        #endregion
    }
}