using System;
using System.Collections.Generic;
using System.Text;

namespace GptApi.Models
{
	public class Response
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Created { get; set; }
        public List<Choice> Choices { get; set; }
        public Usage Usage { get; set; }              
    }

    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        public string Finish_reason { get; set; }
    }

    public class Usage
    {
        public int Prompt_tokens { get; set; }
        public int Completion_tokens { get; set; }
        public int Total_tokens { get; set; }
    }
}
