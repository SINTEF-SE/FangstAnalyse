using System.Text;

namespace Fiskinfo.Fangstanalyse.API.Formatters
{
    public class CsvFormatterOptions
    {
        public bool UseSingleLineHeaderInCsv { get; set; } = true;
        public string CsvDelimiter { get; set; } = ",";
        public Encoding Encoding { get; set; } = Encoding.Default;
        public bool IncludeExcelDelimiterHeader { get; set; } = false;
    }
}