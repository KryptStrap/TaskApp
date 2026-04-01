using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TaskApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TaskItem> allTasks = new ObservableCollection<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();
            TasksGrid.ItemsSource = allTasks;
            UpdateStats();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow window = new TaskWindow();
            if (window.ShowDialog() == true)
            {
                allTasks.Add(window.CurrentTask);
                ApplyFilters();
                UpdateStats();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is TaskItem selectedTask)
            {
                TaskWindow window = new TaskWindow(selectedTask);
                if (window.ShowDialog() == true)
                {
                    // Обновляем данные
                    int index = allTasks.IndexOf(selectedTask);
                    allTasks[index] = window.CurrentTask;
                    ApplyFilters();
                    UpdateStats();
                }
            }
            else MessageBox.Show("Выберите задачу для редактирования!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is TaskItem selectedTask)
            {
                allTasks.Remove(selectedTask);
                ApplyFilters();
                UpdateStats();
            }
            else MessageBox.Show("Выберите задачу для удаления!");
        }

        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (TasksGrid == null) return; // Защита при инициализации окна

            var filtered = allTasks.AsEnumerable();

            // Поиск по названию
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                filtered = filtered.Where(t => t.Title.ToLower().Contains(SearchBox.Text.ToLower()));

            // Фильтр по статусу
            if (FilterStatusCombo.SelectedItem is ComboBoxItem statusItem && statusItem.Content.ToString() != "Все")
                filtered = filtered.Where(t => t.Status == statusItem.Content.ToString());

            // Фильтр по исполнителю
            if (!string.IsNullOrWhiteSpace(FilterAssigneeBox.Text))
                filtered = filtered.Where(t => t.Assignee.ToLower().Contains(FilterAssigneeBox.Text.ToLower()));

            TasksGrid.ItemsSource = new ObservableCollection<TaskItem>(filtered);
        }

        private void UpdateStats()
        {
            if (StatsText == null) return;

            int completed = allTasks.Count(t => t.Status == "Выполнена");

            // Группируем задачи по исполнителям, чтобы посчитать их количество
            var assigneeStats = allTasks.GroupBy(t => t.Assignee)
                                        .Select(g => $"{g.Key}: {g.Count()}");

            string statsString = $"Завершено задач: {completed} | По исполнителям: {string.Join(", ", assigneeStats)}";
            StatsText.Text = statsString;
        }
    }
}