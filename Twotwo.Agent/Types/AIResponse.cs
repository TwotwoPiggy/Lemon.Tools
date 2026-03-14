﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Twotwo.Agent.Types
{
    /// <summary>
    /// AI 响应基类，包含通用的文本回复
    /// </summary>
    public class AIResponse
    {
        public string OriginalResponse { get; set; }

        public AIResponse() { }

        public AIResponse(string text)
        {
            OriginalResponse = text;
        }
    }

}
