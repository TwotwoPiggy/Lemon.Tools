using System;
using System.Collections.Generic;
using System.Text;

namespace GptApi.Models
{
	public class GptModel
	{
		public static readonly string GPT4 = "gpt-4";
		public static readonly string GPT4_0314 = "gpt-4-0314";
		public static readonly string GPT4_32k = "gpt-4-32k";
		public static readonly string GPT4_32k_0314 = "gpt-4-32k-0314";
		public static readonly string GPT3_5_turbo = "gpt-3.5-turbo";
		public static readonly string GPT3_5_turbo0301 = "gpt-3.5-turbo-0301";
	}

	public class Role
	{
		public static readonly string System = "system";
		public static readonly string User = "user";
		public static readonly string Assistant= "assistance";
	}
}
