﻿namespace ArtworkSharing.Core.Interfaces.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false);
}