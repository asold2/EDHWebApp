﻿namespace EDHWebApi.Model;

public class Account
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public int userId { get; set; }
    public bool RememberMe { get; set; }
}