namespace OutWeb.Models.Manage.ExportExcelModels
{
    public class ReplyBase
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        //public List<律師名錄> LayerData { get; set; } = new List<律師名錄>();

        public ExcelForm GetExcelFormType { get; set; }
    }

    public enum ExcelForm
    {
        EmptyForm1 = 1,
        EmptyForm2 = 2,
        EmptyForm3 = 3,
        EmptyForm4 = 4,
        EmptyForm5 = 5,
    }
}