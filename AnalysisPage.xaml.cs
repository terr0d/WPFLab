using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AdaptationManagement.Models;

namespace AdaptationManagement
{
    public partial class AnalysisPage : Page
    {
        private List<AdaptationAnalysisData> allData;
        private List<AdaptationAnalysisData> filteredData;
        private bool isChartInitialized = false;

        public AnalysisPage()
        {
            InitializeComponent();
            LoadTestData();
            InitializeFilters();
            UpdateReport();
            ChartTypeComboBox.SelectedIndex = 0;
            ChartCanvas.SizeChanged += ChartCanvas_SizeChanged;
        }

        private void LoadTestData()
        {
            allData = new List<AdaptationAnalysisData>
            {
                new AdaptationAnalysisData { Department = "IT", Position = "Программист", Quarter = "1 квартал", TotalPrograms = 10, CompletedPrograms = 8, ErrorsCount = 3, EmployedAfterProgram = 7 },
                new AdaptationAnalysisData { Department = "IT", Position = "Аналитик", Quarter = "1 квартал", TotalPrograms = 5, CompletedPrograms = 4, ErrorsCount = 1, EmployedAfterProgram = 4 },
                new AdaptationAnalysisData { Department = "HR", Position = "Менеджер", Quarter = "1 квартал", TotalPrograms = 3, CompletedPrograms = 3, ErrorsCount = 0, EmployedAfterProgram = 3 },
                new AdaptationAnalysisData { Department = "IT", Position = "Программист", Quarter = "2 квартал", TotalPrograms = 8, CompletedPrograms = 6, ErrorsCount = 2, EmployedAfterProgram = 5 },
                new AdaptationAnalysisData { Department = "HR", Position = "Менеджер", Quarter = "2 квартал", TotalPrograms = 4, CompletedPrograms = 3, ErrorsCount = 1, EmployedAfterProgram = 3 }
            };

            filteredData = new List<AdaptationAnalysisData>(allData);
        }

        private void InitializeFilters()
        {
            if (allData == null || !allData.Any())
            {
                MessageBox.Show("Нет данных для анализа", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var departments = allData.Select(x => x.Department).Distinct().ToList();
                var positions = allData.Select(x => x.Position).Distinct().ToList();

                DepartmentFilterComboBox.ItemsSource = departments;
                PositionFilterComboBox.ItemsSource = positions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации фильтров: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void DiagramTab_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isChartInitialized)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateChart();
                    isChartInitialized = true;
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }

        private void ChartCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (isChartInitialized)
            {
                UpdateChart();
            }
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateReport();
        }

        private void ChartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChart();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            DepartmentFilterComboBox.SelectedItem = null;
            PositionFilterComboBox.SelectedItem = null;
            PeriodComboBox.SelectedItem = null;
            UpdateReport();
        }

        private void UpdateReport()
        {
            if (allData == null || !allData.Any())
                return;

            try
            {
                filteredData = allData.ToList();

                if (DepartmentFilterComboBox?.SelectedItem != null)
                {
                    string department = DepartmentFilterComboBox.SelectedItem.ToString();
                    filteredData = filteredData.Where(x => x.Department == department).ToList();
                }

                if (PositionFilterComboBox?.SelectedItem != null)
                {
                    string position = PositionFilterComboBox.SelectedItem.ToString();
                    filteredData = filteredData.Where(x => x.Position == position).ToList();
                }

                if (PeriodComboBox?.SelectedItem != null)
                {
                    string quarter = (PeriodComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                    filteredData = filteredData.Where(x => x.Quarter == quarter).ToList();
                }

                ReportGrid.ItemsSource = filteredData;
                UpdateChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении отчета: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void UpdateChart()
        {
            try
            {
                if (ChartCanvas == null || filteredData == null || !filteredData.Any())
                    return;

                ChartCanvas.Children.Clear();

                // Установка минимальных размеров канваса
                if (ChartCanvas.ActualWidth <= 0)
                    ChartCanvas.Width = 800;
                if (ChartCanvas.ActualHeight <= 0)
                    ChartCanvas.Height = 300;

                double canvasWidth = ChartCanvas.ActualWidth;
                double canvasHeight = ChartCanvas.ActualHeight;

                // Проверка минимальных размеров
                if (canvasWidth < 100 || canvasHeight < 100)
                    return;

                double margin = 50;
                // Вычисление ширины столбца с проверкой минимального значения
                double barWidth = Math.Max(20, (canvasWidth - 2 * margin) / filteredData.Count);

                var selectedType = (ChartTypeComboBox?.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (string.IsNullOrEmpty(selectedType))
                    selectedType = "Процент выполнения";

                var chartData = selectedType switch
                {
                    "Процент выполнения" => filteredData.Select(x => x.CompletionPercentage).ToList(),
                    "Количество ошибок" => filteredData.Select(x => (double)x.ErrorsCount).ToList(),
                    "Процент трудоустройства" => filteredData.Select(x => x.EmploymentRate).ToList(),
                    _ => filteredData.Select(x => x.CompletionPercentage).ToList()
                };

                if (!chartData.Any())
                    return;

                double maxValue = chartData.Max();
                double scale = (canvasHeight - 2 * margin) / (maxValue == 0 ? 1 : maxValue);

                for (int i = 0; i < filteredData.Count; i++)
                {
                    double barHeight = chartData[i] * scale;

                    // Проверка минимальных значений для столбца
                    var bar = new Rectangle
                    {
                        Width = Math.Max(10, barWidth - 10),
                        Height = Math.Max(1, barHeight),
                        Fill = new SolidColorBrush(Colors.Blue),
                        Opacity = 0.7
                    };

                    Canvas.SetLeft(bar, margin + i * barWidth);
                    Canvas.SetBottom(bar, margin);
                    ChartCanvas.Children.Add(bar);

                    var label = new TextBlock
                    {
                        Text = $"{chartData[i]:F1}",
                        TextAlignment = TextAlignment.Center,
                        Width = Math.Max(10, barWidth - 10)
                    };

                    Canvas.SetLeft(label, margin + i * barWidth);
                    Canvas.SetBottom(label, margin + barHeight + 5);
                    ChartCanvas.Children.Add(label);

                    var categoryLabel = new TextBlock
                    {
                        Text = $"{filteredData[i].Department}\n{filteredData[i].Position}\n{filteredData[i].Quarter}",
                        TextAlignment = TextAlignment.Center,
                        Width = Math.Max(10, barWidth - 10),
                        TextWrapping = TextWrapping.Wrap
                    };

                    Canvas.SetLeft(categoryLabel, margin + i * barWidth);
                    Canvas.SetBottom(categoryLabel, 5);
                    ChartCanvas.Children.Add(categoryLabel);
                }

                // Добавление осей и сетки
                var yAxis = new Line
                {
                    X1 = margin,
                    Y1 = margin,
                    X2 = margin,
                    Y2 = canvasHeight - margin,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                ChartCanvas.Children.Add(yAxis);

                var xAxis = new Line
                {
                    X1 = margin,
                    Y1 = canvasHeight - margin,
                    X2 = canvasWidth - margin,
                    Y2 = canvasHeight - margin,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                ChartCanvas.Children.Add(xAxis);

                // Метки по оси Y
                int numYLabels = 5;
                for (int i = 0; i <= numYLabels; i++)
                {
                    double value = maxValue * i / numYLabels;
                    var yLabel = new TextBlock
                    {
                        Text = $"{value:F1}",
                        TextAlignment = TextAlignment.Right
                    };

                    double yPos = canvasHeight - margin - (value * scale);
                    Canvas.SetLeft(yLabel, 5);
                    Canvas.SetTop(yLabel, yPos - 10);
                    ChartCanvas.Children.Add(yLabel);

                    var gridLine = new Line
                    {
                        X1 = margin,
                        Y1 = yPos,
                        X2 = canvasWidth - margin,
                        Y2 = yPos,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 0.5,
                        StrokeDashArray = new DoubleCollection(new double[] { 4, 4 })
                    };
                    ChartCanvas.Children.Add(gridLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении диаграммы: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
    }
}