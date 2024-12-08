using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AdaptationManagement.Models;

namespace AdaptationManagement
{
    public partial class ConstructorPage : Page
    {
        private List<Employee> employees;
        private List<Department> departments;
        private List<Position> positions;
        private List<Mentor> mentors;
        private ObservableCollection<ModuleViewModel> modules;

        public ConstructorPage()
        {
            InitializeComponent();
            LoadDemoData();
            InitializeComboBoxes();
        }

        private void LoadDemoData()
        {
            departments = new List<Department>
            {
                new Department { Id = 1, Name = "IT отдел" },
                new Department { Id = 2, Name = "HR отдел" },
                new Department { Id = 3, Name = "Бухгалтерия" }
            };

            employees = new List<Employee>
            {
                new Employee { Id = 1, FullName = "Иванов Иван Иванович", Email = "ivanov@example.com" },
                new Employee { Id = 2, FullName = "Петров Петр Петрович", Email = "petrov@example.com" },
                new Employee { Id = 3, FullName = "Сидорова Анна Павловна", Email = "sidorova@example.com" }
            };

            positions = new List<Position>
            {
                new Position { Id = 1, Name = "Программист", Department = departments[0] },
                new Position { Id = 2, Name = "HR-менеджер", Department = departments[1] },
                new Position { Id = 3, Name = "Бухгалтер", Department = departments[2] }
            };

            mentors = new List<Mentor>
            {
                new Mentor { Id = 1, FullName = "Кузнецов Сергей Владимирович", Email = "kuznetsov@example.com", Department = departments[0] },
                new Mentor { Id = 2, FullName = "Морозова Елена Александровна", Email = "morozova@example.com", Department = departments[1] },
                new Mentor { Id = 3, FullName = "Васильев Дмитрий Николаевич", Email = "vasiliev@example.com", Department = departments[2] }
            };
        }

        private void InitializeComboBoxes()
        {
            EmployeeComboBox.ItemsSource = employees;
            DepartmentComboBox.ItemsSource = departments;
        }

        private void EmployeeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI();
        }

        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                PositionComboBox.ItemsSource = positions.Where(p => p.Department.Id == selectedDepartment.Id);
            }
            UpdateUI();
        }

        private void PositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PositionComboBox.SelectedItem is Position selectedPosition)
            {
                LoadModulesForPosition(selectedPosition);
            }
        }

        private void LoadModulesForPosition(Position position)
        {
            var adaptationModules = new List<AdaptationModule>
            {
                new AdaptationModule
                {
                    Id = 1,
                    Name = "Вводный инструктаж",
                    Status = "Принят в работу",
                    Position = position.Name,
                    DeadlineDate = DateTime.Now.AddDays(14),
                    MainApprover = "Иванов И.И.",
                    Developers = new List<string> { "Петров П.П.", "Сидоров С.С." },
                    Approvers = new List<string> { "Иванов И.И.", "Козлов К.К." }
                },
                new AdaptationModule
                {
                    Id = 2,
                    Name = "Обучение базовым навыкам",
                    Status = "Принят в работу",
                    Position = position.Name,
                    DeadlineDate = DateTime.Now.AddDays(30),
                    MainApprover = "Петров П.П.",
                    Developers = new List<string> { "Сидоров С.С.", "Козлов К.К." },
                    Approvers = new List<string> { "Петров П.П.", "Иванов И.И." }
                },
                new AdaptationModule
                {
                    Id = 3,
                    Name = "Специализированное обучение",
                    Status = "Новый",
                    Position = position.Name,
                    DeadlineDate = DateTime.Now.AddDays(45),
                    MainApprover = "Сидоров С.С.",
                    Developers = new List<string> { "Иванов И.И.", "Петров П.П." },
                    Approvers = new List<string> { "Сидоров С.С.", "Козлов К.К." }
                }
            };

            modules = new ObservableCollection<ModuleViewModel>(
                adaptationModules.Select(m => new ModuleViewModel
                {
                    Module = m,
                    AvailableMentors = mentors.Where(mentor => mentor.Department.Id == position.Department.Id).ToList()
                })
            );

            ModulesItemsControl.ItemsSource = modules;
        }

        private void GenerateProgram_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection())
                return;

            try
            {
                // Заглушка для создания Excel файла
                MessageBox.Show("Excel файл успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Заглушка для отправки email
                MessageBox.Show("Программа адаптации отправлена на email наставника и сотрудника", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Заглушка для сохранения локального файла
                string fileName = $"{DepartmentComboBox.Text}_{PositionComboBox.Text}_{EmployeeComboBox.Text}_{DateTime.Now:yyyy-MM-dd}.xlsx";
                MessageBox.Show($"Файл сохранен локально: {fileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании программы: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private bool ValidateSelection()
        {
            if (EmployeeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (DepartmentComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите отдел", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (PositionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите должность", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var selectedModules = modules?.Where(m => m.IsSelected).ToList();
            if (selectedModules == null || !selectedModules.Any())
            {
                MessageBox.Show("Выберите хотя бы один модуль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (selectedModules.Any(m => m.IsSelected && m.SelectedMentor == null))
            {
                MessageBox.Show("Выберите наставника для каждого выбранного модуля", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearSelection()
        {
            EmployeeComboBox.SelectedItem = null;
            DepartmentComboBox.SelectedItem = null;
            PositionComboBox.SelectedItem = null;
            ModulesItemsControl.ItemsSource = null;
        }

        private void UpdateUI()
        {
            ModulesItemsControl.ItemsSource = null;
        }
    }
}