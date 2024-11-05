using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace RobertHarrisonTODO
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();

        public MainPage()
        {
            InitializeComponent();
            Title = "";
            BindingContext = this;
        }

        async void AddTaskBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            string description = await DisplayPromptAsync("Task description", "Enter your task description");
            if (string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Error", "You have to enter a description to create a task", "OK");
                return;
            }
            Task newTask = new Task(description, false);
            Tasks.Add(newTask);
            checkIsListEmpty();
        }

        void checkIsListEmpty()
        {
            emptyListLabel.IsVisible = Tasks.Count == 0;
        }

        private void DeleteTask_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            var task = button?.BindingContext as Task;
            Tasks.Remove(task);
            checkIsListEmpty();
        }

        private async void EditTask_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Task task = button.BindingContext as Task;

            string newDescription = await DisplayPromptAsync("Edit Task", "Enter new description");
            if (newDescription == null)
            {
                await DisplayAlert("Error", "You must enter new description", "Ok");
            }
            //task.description = newDescription; for some reason it does not display new description in the list but description actually changes
            //for work around create new task with new description and remove the old one
            Console.Write(newDescription);
            Tasks.Remove(task);
            Task newTask = new Task(newDescription, false);
            Tasks.Add(newTask);
        }

        
    }
}
