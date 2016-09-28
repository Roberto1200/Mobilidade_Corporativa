using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Manager.Helpers
{
    public class FuncoesDiversas
    {
        public string RemoverAcentuacao(string text)
        {
            return new string(text
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }
        public string RemoverCaracteres(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public DateTime PrimeiraHora(DateTime data)
        {
            return DateTime.Parse(data.ToString("dd/MM/yyyy 00:00:00"));
        }

        public DateTime UltimaHora(DateTime data)
        {
            return DateTime.Parse(data.ToString("dd/MM/yyyy 23:59:59"));
        }
    }
}