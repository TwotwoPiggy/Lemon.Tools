namespace Twotwo.Agent.Configuration
{
    public class AIConfig
    {
        public string? ApiKey { get; set; }
        public string? ModelName { get; set; }
        /// <summary>
        /// 每分钟请求数
        /// </summary>
        public int RPM { get; set; } = 5;
        /// <summary>
        /// 每分钟token数(输入)
        /// </summary>
        /// <remarks>default 250K</remarks>
        public long TPM { get; set; } = 250 * 1000;
        /// <summary>
        /// 每日请求数
        /// </summary>
        public int RPD { get; set; } = 20;
        public ProxyConfig? Proxy { get; set; }
    }

    public class ProxyConfig
    {
        public bool Enabled { get; set; }
        public string? Address { get; set; }
    }
}
