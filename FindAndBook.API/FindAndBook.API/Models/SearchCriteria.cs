using System.ComponentModel.DataAnnotations;

namespace FindAndBook.API.Models
{
    public class SearchCriteria
    {
        public string SearchBy { get; set; }

        public string Pattern { get; set; }
    }
}