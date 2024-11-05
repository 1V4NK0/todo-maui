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
            if (description == null)
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
            if (Tasks.Count > 0)
            {
                emptyListLabel.IsVisible = false;
            } else
            {
                emptyListLabel.IsVisible = true;
            }
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
            string newDescription = await DisplayPromptAsync("", "Enter new description");
            if (newDescription == null)
            {
                await DisplayAlert("Error", "Enter description", "Ok");
                return;
            }
            Button button = (Button)sender;
            Task task = button.BindingContext as Task;
            task.description = newDescription;
        }
    }
}
