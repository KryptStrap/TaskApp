using System;

namespace TaskApp
{
    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Assignee { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }

        public string RowColor
        {
            get
            {
                if (Status == "Выполнена") return "#d4edda";
                if (Status == "Новая") return "#f8d7da";
                return "#fff3cd";
            }
        }
    }
}