using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class ExceptionLogModel
    {

        [Key]
        public int LogId { get; set; }
        public DateTime ErrorDate { get; set; }
        public string ErrorMethodName { get; set; }
        public string ErrorMessage { get; set; }

        public ExceptionLogModel(Exception ex)
        {
            ErrorDate = DateTime.Now;
            ErrorMessage = ex.Message;
            ErrorMethodName = GetRelevantStackTrace(ex);
        }

        private string GetRelevantStackTrace(Exception ex)
        {
            string stackTrace = ex.StackTrace;
            return stackTrace;
        }

        public override string ToString()
        {
            return $"{ErrorDate} - Error: {ErrorMessage}\nStackTrace: {ErrorMethodName}\n";
        }
    }
}
