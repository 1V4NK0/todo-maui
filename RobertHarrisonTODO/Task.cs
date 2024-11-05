using System;
namespace RobertHarrisonTODO
{
	public class Task
	{
		public string description { get; set; }
		public bool completed { get; set; }

		public Task(string description, bool completed)
		{
			this.description = description;
			this.completed = completed;
		}
	}
}

