using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskApp
{
    public partial class TaskWindow : Window
    {
        public TaskItem CurrentTask { get; private set; }

        public TaskWindow(TaskItem task = null)
        {
            InitializeComponent();
            DueDatePicker.SelectedDate = DateTime.Now;

            if (task != null) // Если передали задачу, значит это редактирование
            {
                TitleBox.Text = task.Title;
                DescBox.Text = task.Description;
                AssigneeBox.Text = task.Assignee;
                StatusCombo.Text = task.Status;
                DueDatePicker.SelectedDate = task.DueDate;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text) || string.IsNullOrWhiteSpace(AssigneeBox.Text))
            {
                MessageBox.Show("Заполните название и исполнителя!");
                return;
            }

            CurrentTask = new TaskItem
            {
                Title = TitleBox.Text,
                Description = DescBox.Text,
                Assignee = AssigneeBox.Text,
                Status = ((ComboBoxItem)StatusCombo.SelectedItem).Content.ToString(),
                DueDate = DueDatePicker.SelectedDate ?? DateTime.Now
            };

            DialogResult = true; // Закрываем окно с успешным результатом
        }
    }
}