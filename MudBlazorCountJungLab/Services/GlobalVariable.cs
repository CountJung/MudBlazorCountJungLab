﻿namespace MudBlazorCountJungLab.Services
{
    public class GlobalVariable
    {
        public static GlobalVariable? Instance { get; set; }
        public GlobalVariable() 
        {
            Instance = this;
        }
        public bool DarkMode { get; set; }
    }
}
