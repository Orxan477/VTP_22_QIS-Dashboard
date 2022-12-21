namespace VTP_22_Dashboard.ViewModels
{
    public class Paginate<T>
    {
        public Paginate(List<T> item, int currentPage, int pageCount)
        {
            Items = item;
            CurrentPage = currentPage;
            PageCount = pageCount;
        }
        public List<T> Items { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
