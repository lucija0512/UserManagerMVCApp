﻿namespace UserManager.Application.Configuration
{
    public class EmailSettings
    {
        public const string EmailConfig = "EmailSettings";
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
    }
}
