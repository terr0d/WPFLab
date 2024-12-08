using System;
using System.Windows;
using System.Windows.Controls;

namespace AdaptationManagement
{
    public partial class MainWindow : Window
    {
        private bool[] tabsInitialized = new bool[3]; // Флаги для отслеживания инициализации вкладок

        public MainWindow()
        {
            InitializeComponent();
            // Инициализируем первую вкладку при запуске
            LoadTabContent(0);
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex >= 0)
            {
                LoadTabContent(MainTabControl.SelectedIndex);
            }
        }

        private void LoadTabContent(int tabIndex)
        {
            if (tabsInitialized[tabIndex])
                return;

            try
            {
                switch (tabIndex)
                {
                    case 0: // Адаптационные модули
                        ModulesFrame.Navigate(new ModulesPage());
                        break;

                    case 1: // Конструктор
                        ConstructorFrame.Navigate(new ConstructorPage());
                        break;

                    case 2: // Анализ адаптационных мероприятий
                        AnalysisFrame.Navigate(new AnalysisPage());
                        break;
                }

                tabsInitialized[tabIndex] = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке содержимого вкладки: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
    }
}