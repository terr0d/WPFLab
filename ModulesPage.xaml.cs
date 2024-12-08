using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AdaptationManagement
{
    public partial class ModulesPage : Page
    {
        private ObservableCollection<AdaptationModule> newModules;
        private ObservableCollection<AdaptationModule> draftModules;
        private ObservableCollection<AdaptationModule> activeModules;

        public ModulesPage()
        {
            InitializeComponent();
            InitializeCollections();
            LoadPositions();
            SetupDataGrids();
        }

        private void InitializeCollections()
        {
            newModules = new ObservableCollection<AdaptationModule>();
            draftModules = new ObservableCollection<AdaptationModule>();
            activeModules = new ObservableCollection<AdaptationModule>();

            // Привязка коллекций к DataGrid
            NewModulesGrid.ItemsSource = newModules;
            DraftModulesGrid.ItemsSource = draftModules;
            ActiveModulesGrid.ItemsSource = activeModules;
        }

        private void LoadPositions()
        {
            // Временное наполнение комбобокса должностей для демонстрации
            // В реальном приложении данные должны загружаться из базы данных
            var positions = new[]
            {
                "Программист",
                "Менеджер",
                "Аналитик",
                "Дизайнер"
            };
            PositionFilter.ItemsSource = positions;
        }

        private void SetupDataGrids()
        {
            // Новые модули
            newModules.Add(new AdaptationModule
            {
                Name = "Введение в работу программиста",
                Position = "Программист",
                Developers = new List<string> { "Иванов И.И.", "Петров П.П." },
                Approvers = new List<string> { "Сидоров С.С." },
                MainApprover = "Сидоров С.С.",
                DeadlineDate = DateTime.Now.AddDays(30),
                Status = "New"
            });

            newModules.Add(new AdaptationModule
            {
                Name = "Основы менеджмента проектов",
                Position = "Менеджер",
                Developers = new List<string> { "Смирнов А.А.", "Козлов К.К." },
                Approvers = new List<string> { "Волков В.В." },
                MainApprover = "Волков В.В.",
                DeadlineDate = DateTime.Now.AddDays(45),
                Status = "New"
            });

            // Черновики
            draftModules.Add(new AdaptationModule
            {
                Name = "Аналитика бизнес-процессов",
                Position = "Аналитик",
                Developers = new List<string> { "Морозов М.М." },
                Approvers = new List<string> { "Волков В.В.", "Сидоров С.С." },
                MainApprover = "Волков В.В.",
                DeadlineDate = DateTime.Now.AddDays(20),
                Status = "Draft"
            });

            draftModules.Add(new AdaptationModule
            {
                Name = "Базы данных для начинающих",
                Position = "Программист",
                Developers = new List<string> { "Петров П.П." },
                Approvers = new List<string> { "Иванов И.И." },
                MainApprover = "Иванов И.И.",
                DeadlineDate = DateTime.Now.AddDays(15),
                Status = "Draft"
            });

            // Активные модули
            activeModules.Add(new AdaptationModule
            {
                Name = "Управление командой",
                Position = "Менеджер",
                Developers = new List<string> { "Волков В.В." },
                Approvers = new List<string> { "Сидоров С.С." },
                MainApprover = "Сидоров С.С.",
                DeadlineDate = DateTime.Now.AddDays(60),
                Status = "Active"
            });

            activeModules.Add(new AdaptationModule
            {
                Name = "Системный анализ",
                Position = "Аналитик",
                Developers = new List<string> { "Морозов М.М.", "Смирнов А.А." },
                Approvers = new List<string> { "Волков В.В." },
                MainApprover = "Волков В.В.",
                DeadlineDate = DateTime.Now.AddDays(25),
                Status = "Active"
            });

            activeModules.Add(new AdaptationModule
            {
                Name = "Разработка на C#",
                Position = "Программист",
                Developers = new List<string> { "Иванов И.И." },
                Approvers = new List<string> { "Сидоров С.С." },
                MainApprover = "Сидоров С.С.",
                DeadlineDate = DateTime.Now.AddDays(40),
                Status = "Active"
            });
        }

        private void CreateNewModule_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewModuleWindow();
            if (dialog.ShowDialog() == true)
            {
                var newModule = dialog.Result;

                // Добавляем модуль в список модулей "В работе"
                activeModules.Add(newModule);

                // Отправка уведомлений
                SendNotifications(newModule);
                GenerateAndSendPdfOrder(newModule);

                MessageBox.Show("Модуль успешно создан и добавлен в работу", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SendNotifications(AdaptationModule module)
        {
            MessageBox.Show("Уведомления отправлены всем участникам");
        }

        private void GenerateAndSendPdfOrder(AdaptationModule module)
        {
            MessageBox.Show("PDF приказ сгенерирован и отправлен");
        }

        private void PositionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PositionFilter.SelectedItem == null)
            {
                // Если ничего не выбрано, показываем все
                NewModulesGrid.Items.Filter = null;
                DraftModulesGrid.Items.Filter = null;
                ActiveModulesGrid.Items.Filter = null;
                return;
            }

            string selectedPosition = PositionFilter.SelectedItem.ToString();

            // Создаем предикат для фильтрации
            Predicate<object> filter = obj =>
            {
                if (obj is AdaptationModule module)
                {
                    return module.Position == selectedPosition;
                }
                return false;
            };

            // Применяем фильтр ко всем гридам
            NewModulesGrid.Items.Filter = filter;
            DraftModulesGrid.Items.Filter = filter;
            ActiveModulesGrid.Items.Filter = filter;
        }

        private void FilterModulesByPosition(string position)
        {
            // Применение фильтра ко всем гридам
            NewModulesGrid.Items.Filter = item =>
            {
                var module = item as AdaptationModule;
                return module?.Position == position;
            };

            DraftModulesGrid.Items.Filter = item =>
            {
                var module = item as AdaptationModule;
                return module?.Position == position;
            };

            ActiveModulesGrid.Items.Filter = item =>
            {
                var module = item as AdaptationModule;
                return module?.Position == position;
            };
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            PositionFilter.SelectedItem = null;
            NewModulesGrid.Items.Filter = null;
            DraftModulesGrid.Items.Filter = null;
            ActiveModulesGrid.Items.Filter = null;
        }
    }
}