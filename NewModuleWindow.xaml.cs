using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace AdaptationManagement
{
    public partial class NewModuleWindow : Window
    {
        public AdaptationModule Result { get; private set; }

        public NewModuleWindow()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void LoadInitialData()
        {
            // Загрузка тестовых данных в комбобоксы
            PositionComboBox.ItemsSource = new[] { "Программист", "Менеджер", "Аналитик" };
        }

        private void AddDeveloper_Click(object sender, RoutedEventArgs e)
        {
            // Добавление тестового разработчика
            DevelopersListBox.Items.Add("Новый разработчик");
        }

        private void AddApprover_Click(object sender, RoutedEventArgs e)
        {
            // Добавление тестового согласующего
            ApproversListBox.Items.Add("Новый согласующий");
            MainApproverComboBox.Items.Add("Новый согласующий");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ModuleNameTextBox.Text))
            {
                MessageBox.Show("Введите название модуля");
                return;
            }

            Result = new AdaptationModule
            {
                Name = ModuleNameTextBox.Text,
                Position = PositionComboBox.SelectedItem?.ToString(),
                Developers = DevelopersListBox.Items.Cast<string>().ToList(),
                Approvers = ApproversListBox.Items.Cast<string>().ToList(),
                MainApprover = MainApproverComboBox.SelectedItem?.ToString(),
                DeadlineDate = DeadlineDatePicker.SelectedDate ?? DateTime.Now,
                Status = "New"
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}