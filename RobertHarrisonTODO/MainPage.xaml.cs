using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace RobertHarrisonTODO
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
        private const string TasksKey = "tasks"; // Key for saving the tasks

        public MainPage()
        {
            InitializeComponent();
            Title = "";
            BindingContext = this;
            LoadTasks(); // load all saved tasks on app opening
                         
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // get the checkbox that triggered the event
            CheckBox checkBox = (CheckBox)sender;

            // get the associated task from the BindingContext
            Task task = (Task)checkBox.BindingContext;

            // update the task's completed property based on the checkbox state
            task.completed = e.Value;

            SaveTasks();
        }


        async void AddTaskBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            //getting a task description from user if input empty you get pop up
            string description = await DisplayPromptAsync("Task description", "Enter your task description");
            if (string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Error", "You have to enter a description to create a task", "OK");
                return;
            }
            //creating new task based on input and adding to the list, saving 
            Task newTask = new Task(description, false);
            Tasks.Add(newTask);
            SaveTasks(); 
            checkIsListEmpty();
        }

        void checkIsListEmpty()
        {
            //if tasks list empty label "lets add something" goes away
            emptyListLabel.IsVisible = Tasks.Count == 0;
        }

        private void DeleteTask_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            var task = button?.BindingContext as Task;
            Tasks.Remove(task);
            SaveTasks(); 
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
                return;
            }
            //seems weird but for some reason task description in UI is not being changed while it is underneath so in order to update UI
            //you delete current task and create a new one based on old with updated description and substitute it
            Tasks.Remove(task);
            Task newTask = new Task(newDescription, false);
            Tasks.Add(newTask);
            SaveTasks(); // save the updated tasks collection
        }

        private void SaveTasks()
        {
            // in order to save task object you have to basically convert it into string and then save
            string tasksJson = JsonSerializer.Serialize(Tasks);
            Preferences.Set(TasksKey, tasksJson);
        }

        private void LoadTasks()
        {
            checkIsListEmpty();

            // Retrieve the JSON string from preferences
            string jsonTasks = Preferences.Get(TasksKey, null);
            if (jsonTasks != null)
            {
                // converting string task obj into an actual obj which will be used
                var tasks = JsonSerializer.Deserialize<ObservableCollection<Task>>(jsonTasks);
                foreach (var task in tasks)
                {
                    Tasks.Add(task); // add each task to the ObservableCollection
                }
            }
        }
    }
}
