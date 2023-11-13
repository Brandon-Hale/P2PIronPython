namespace DataWebService.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Result { get; set; }

        public Job()
        {
            Id = 0;
            Status = "";
            Code = "";
            Result = "";
        }
    }
}
