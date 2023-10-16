using System;
using System.Collections.Generic;
using System.Text;

namespace GptApi.Models
{
	public class Request
	{
		public string Model { get; set; }
		public List<Message> Messages { get; set; }
	}
}
