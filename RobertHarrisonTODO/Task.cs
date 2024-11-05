using System;
namespace RobertHarrisonTODO
{
	public class Task
	{
		string description { get; set; }
		bool completed { get; set; }

		public Task(string description, bool completed)
		{
			this.description = description;
			this.completed = completed;
		}
	}
}

