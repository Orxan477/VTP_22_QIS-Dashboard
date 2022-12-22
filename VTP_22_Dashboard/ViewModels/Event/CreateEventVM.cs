namespace VTP_22_Dashboard.ViewModels.Event
{
    public class CreateEventVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentsId { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Link { get; set; }
    }
}
