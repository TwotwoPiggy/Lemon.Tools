using System;
using System.Collections.Generic;
using System.Text;
using Google.GenAI.Types;

namespace Twotwo.Agent.Types
{
    public class GeminiResponse : AIResponse
    {
        public object SdkHttpResponse { get; set; }
        public List<Candidate> Candidates { get; set; }
        public object CreateTime { get; set; }
        public string ModelVersion { get; set; }
        public object PromptFeedback { get; set; }
        public string ResponseId { get; set; }
        public GenerateContentResponseUsageMetadata UsageMetadata { get; set; }
        public string Text { get; set; }
        public object FunctionCalls { get; set; }
        public object ExecutableCode { get; set; }
        public object CodeExecutionResult { get; set; }
        public List<Part> Parts { get; set; }

        public GeminiResponse(string response) : base(response) { }
    }
}
