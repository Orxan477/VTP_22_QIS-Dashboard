﻿namespace VTP_22_Dashboard.ViewModels.Event
{
    public class UpdateEventVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentsId { get; set; }
        public DateTime Date { get; set; }
    }
}