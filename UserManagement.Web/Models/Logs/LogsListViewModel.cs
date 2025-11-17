namespace UserManagement.Web.Models.Logs
{
    public class LogsListViewModel
    {
        public List<LogEntryViewModel> Logs { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; } = "";
        public string SortOrder { get; set; } = "asc";
    }
}
