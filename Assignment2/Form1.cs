using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Assignment2
{
    public partial class Form1 : Form
    {

        private Color[] colors = new Color[]
        {
            Color.Red, Color.Blue, Color.Green,
            Color.Yellow, Color.Purple, Color.Orange,
            Color.Cyan, Color.Brown, Color.Magenta
        };

        public Form1()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeChart()
        {
            chart1.Series.Clear();
            chart1.Legends.Clear();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            var input = textValue.Text;

            // Check if input is empty
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter some values separated by commas.");
                return;
            }

            var values = input.Split(',')
                .Select(v => v.Trim())
                .Select(v =>
                {
                    if (int.TryParse(v, out int result))
                        return result;
                    else
                    {
                        MessageBox.Show($"Invalid input: '{v}'. Only integers are allowed.");
                        return (int?)null;
                    }
                })
                .Where(v => v.HasValue)
                .Select(v => v.Value)
                .ToArray();

            if (values.Length == 0)
            {
                MessageBox.Show("Please enter valid integer values.");
                return;
            }

            DisplayPieChart(values);
        }

        private void DisplayPieChart(int[] values)
        {
            chart1.Series.Clear();

            Series series = new Series
            {
                ChartType = SeriesChartType.Pie,
                Name = "Values"
            };

            // Calculate the total
            int total = values.Sum();

            for (int i = 0; i < values.Length; i++)
            {
                series.Points.AddXY($"Slice {i + 1}", values[i]);
                series.Points[i].Color = colors[i % colors.Length];

                // Set the slice label to just the slice name
                series.Points[i].Label = $"Slice {i + 1}";

                // Add the legend entry with percentage
                double percentage = total > 0 ? (values[i] / (double)total) * 100 : 0;
                series.Points[i].LegendText = $"{percentage:F1}%"; // Set percentage in the legend
            }

            // Add the series to the chart
            chart1.Series.Add(series);

            // Add legend
            chart1.Legends.Clear();
            chart1.Legends.Add(new Legend("Legend"));

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.Legends.Clear();

            // Clear the input field
            textValue.Clear();
        }
    }
}
