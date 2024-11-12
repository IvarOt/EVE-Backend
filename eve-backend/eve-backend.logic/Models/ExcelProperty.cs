namespace eve_backend.logic.Models
{
    public class ExcelProperty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueStyle {  get; set; }
        public int ExcelObjectId { get; set; }

        public ExcelProperty() { }
    }
}
