namespace MilkTeaShop.Helper
{
    public  class DataResult<T>
    {
        public T? Data { get; set; }
        public  bool Status { get; set; }
        public  string? Message { get; set; }
        public string? Errors { get; set; }
        public DataResult() { }
        public DataResult(string mess, bool status, T? data, string? errors = null)
        {
            this.Data = data;
            this.Status = status;   
            this.Message = mess;
            this.Errors = errors;
        }
    }    
}
