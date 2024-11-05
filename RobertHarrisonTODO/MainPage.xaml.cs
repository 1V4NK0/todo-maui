using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RobertHarrisonTODO;
//dotnet build -t:Run -f net8.0-ios
//dotnet build -t:Run -f net8.0-maccatalyst 
public partial class MainPage : ContentPage
{
    public ObservableCollection<Task> Tasks = new ObservableCollection<Task>();

	public MainPage()
	{
		InitializeComponent();
		Title = "";
        
	}

    

    async void AddTaskBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        string description = await DisplayPromptAsync("Task description", "Enter your task description");
        if (description == null)
        {
            await DisplayAlert("Error","You have to enter description to create a task", "OK");
            return;
        }
        Task newTask = new Task(description, false);
        Tasks.Add(newTask);
    }
}


