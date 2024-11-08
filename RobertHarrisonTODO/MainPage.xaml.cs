using System.Collections.ObjectModel;
//dotnet build -t:Run -f net8.0-maccatalyst
using System.Text.Json;
namespace RobertHarrisonTODO
{
    public partial class MainPage : ContentPage
    {
        // ObservableCollection to hold the tasks
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();

        // Key used to store and retrieve tasks from preferences
        private const string TasksKey = "tasks";

        public MainPage()
        {
            InitializeComponent();
            Title = "";  // Set title (empty in this case)
            BindingContext = this;  // Set the binding context to the current page
            LoadTasks();  // Load tasks from saved preferences when the page loads
        }

        // Event handler for when the checkbox is checked or unchecked
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
                CheckBox checkBox = (CheckBox)sender;
                Task task = (Task)checkBox.BindingContext;
                task.completed = e.Value;  // Update the task's completed status
                //SortTasks();  // Sort tasks after status change
                SaveTasks();  // Save updated tasks
            //});
        }

        // Event handler for adding a new task
        async void AddTaskBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            // Prompt the user for a task description
            string description = await DisplayPromptAsync("Task description", "Enter your task description");
            if (string.IsNullOrWhiteSpace(description))  // Check if description is empty or null
            {
                await DisplayAlert("Error", "You have to enter a description to create a task", "OK");
                return;
            }
            Task newTask = new Task(description, false);  // Create a new task with description and incomplete status
            Tasks.Add(newTask);  // Add the new task to the collection
            SaveTasks();  // Save tasks after adding the new one
            checkIsListEmpty();  // Check if the list is empty and show/hide the empty list label
        }

        // Checks if the task list is empty and shows the appropriate label
        void checkIsListEmpty()
        {
            emptyListLabel.IsVisible = Tasks.Count == 0;  // Show the empty list label if no tasks are present
        }

        // Event handler for deleting a task
        private void DeleteTask_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            var task = button?.BindingContext as Task;
            Tasks.Remove(task);  // Remove the task from the collection
            SaveTasks();  // Save tasks after deletion
            checkIsListEmpty();  // Check if the list is empty and update the label
        }

        // Event handler for editing a task
        private async void EditTask_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Task task = button.BindingContext as Task;
            string newDescription = await DisplayPromptAsync("Edit Task", "Enter new description");
            if (newDescription == null)  // Check if the new description is null or empty
            {
                await DisplayAlert("Error", "You must enter new description", "Ok");
                return;
            }

            Tasks.Remove(task);  // Remove the old task
            Task newTask = new Task(newDescription, false);  // Create a new task with the new description
            Tasks.Add(newTask);  // Add the new task to the collection
            SaveTasks();  // Save tasks after editing
        }

        // Save the current tasks collection to preferences in JSON format
        private void SaveTasks()
        {
            string tasksJson = JsonSerializer.Serialize(Tasks.ToList());  // Serialize the tasks to JSON
            Preferences.Set(TasksKey, tasksJson);  // Save the serialized tasks to preferences
        }

        // Load tasks from preferences and display them
        private void LoadTasks()
        {
            checkIsListEmpty();  // Check if the list is empty when loading
            string jsonTasks = Preferences.Get(TasksKey, null);  // Retrieve the saved tasks from preferences
            if (jsonTasks != null)  // If tasks exist in preferences
            {
                var tasks = JsonSerializer.Deserialize<List<Task>>(jsonTasks);  // Deserialize the JSON to a list of tasks
                foreach (var task in tasks)  // Add each task to the ObservableCollection
                {
                    Tasks.Add(task);
                }
            }
            checkIsListEmpty();  // Check if the list is empty after loading
        }

        // Sort tasks by their completion status, putting completed tasks at the bottom
        private void SortTasks()
        {
            // Sort the tasks by 'completed' status, placing completed tasks at the bottom
            var sortedTasks = Tasks.OrderBy(task => task.completed).ToList();

            //Clear the existing tasks and re-add them in sorted order not sure why but without running on main thread thing app crashes
            Device.BeginInvokeOnMainThread(() =>
            {
                Tasks.Clear();
                foreach (var task in sortedTasks)
                {
                    Tasks.Add(task);
                }
            });

            SaveTasks();  // Save tasks after sorting


        }
    }
}
