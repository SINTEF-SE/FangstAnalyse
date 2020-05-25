using System.Text;

namespace Fiskinfo.Fangstanalyse.API2.Formatters
{
    public class CsvFormatterOptions
    {
        public bool UseSingleLineHeaderInCsv { get; set; } = true;
        public string CsvDelimiter { get; set; } = ",";
        public Encoding Encoding { get; set; } = Encoding.Default;
        public bool IncludeExcelDelimiterHeader { get; set; } = false;
    }
}