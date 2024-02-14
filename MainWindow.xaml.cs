using System.Drawing;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Math;

namespace ChartWF
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly Series mainSeries, sinSeries;
		private readonly ChartArea area;

		private void Error(string message)
			=> System.Windows.MessageBox.Show(this, message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			#region Range

			if (!double.TryParse(stepBox.Text, out double step))
			{
				Error("Параметр шага задан неверно");
				return;
			}

			if (!double.TryParse(lowBoundBox.Text, out double low))
			{
				Error("Параметр шага задан неверно");
				return;
			}

			if (!double.TryParse(heightBoundBox.Text, out double height))
			{
				Error("Параметр шага задан неверно");
				return;
			}

			if (low >= height)
			{
				Error("Нижний порог должен быть меньше чем верхний порог");
				return;
			}
			#endregion

			#region Coefficients
			double k1, k2, k3, k4;
			
			if (!double.TryParse(k1Box.Text, out k1))
			{
				Error("Первый коэффициент не верен");
				return;
			}
			if (!double.TryParse(k1Box.Text, out k2))
			{
				Error("Второй коэффициент не верен");
				return;
			}
			if (!double.TryParse(k1Box.Text, out k3))
			{
				Error("Третий коэффициент не верен");
				return;
			}
			if (!double.TryParse(k1Box.Text, out k4))
			{
				Error("Четвёртый коэффициент не верен");
				return;
			}

			#endregion

			area.AxisX.Minimum = low;
			area.AxisX.Maximum = height;

			dataGridView.Rows.Clear();
			mainSeries.Points.Clear();
			for (double x = low; x <= height; x += step)
			{
				double y = Formula(x, k1, k2, k3, k4);
				string[] row = { x.ToString(), y.ToString("0.000") };
				mainSeries.Points.AddXY(x, y);
				dataGridView.Rows.Add(row);
			}
		}

		public MainWindow()
		{
			InitializeComponent();

			dataGridView.ColumnCount = 2;
			dataGridView.Columns[0].Name = "x";
			dataGridView.Columns[1].Name = "y";

			chart.BackColor = Color.CadetBlue;

			area = new ChartArea
			{
				BorderWidth = 3,
				BorderColor = Color.Blue,   
			};

			area.AxisX.Name = "x";
			area.AxisX.TextOrientation = TextOrientation.Horizontal;
			area.AxisX.Minimum = -20;
			area.AxisX.Maximum = 20;

			chart.ChartAreas.Add(area);

			Legend legend = new Legend
			{
				Alignment = StringAlignment.Center,
				Font = new Font("Utopia", 10),
				ForeColor = Color.White,
				BackColor = Color.FromArgb(40, Color.Black),
			};

			chart.Legends.Add(legend);

			mainSeries = new Series
			{
				ChartType = SeriesChartType.Spline,
				Enabled = true,
				Name = "y = k1 * sin(k2 * x + k3) + k4",
				Color = Color.MediumAquamarine,
				MarkerStyle = MarkerStyle.Diamond,
				MarkerColor = Color.Black,
				MarkerStep = 1,
			};
			sinSeries = new Series
			{
				ChartType = SeriesChartType.Spline,
				Enabled = true,
				Name = "y = sin(x)",
				Color = Color.OrangeRed,
				MarkerStep = 1,
			};
			for (double x = -200; x <= 200; x += 0.25)
			{
				sinSeries.Points.AddXY(x, Sin(x));
			}

			chart.Series.Add(mainSeries);
			chart.Series.Add(sinSeries);
		}

		private static double Formula(double x, double k1, double k2, double k3, double k4)
			=> k1 * Sin(k2 * x + k3) + k4;
	}
}
