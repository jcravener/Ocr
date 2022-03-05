using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WestUs3_OCR_OPC.Models;
using System.Text.RegularExpressions;

namespace WestUs3_OCR_OPC.Processors
{
    public class ScoreCard
    {
        public ScoreCard(ReadOperationResult readOperationResult, OcrRequest ocrRequest)
        {
            OcrRequest = ocrRequest;
            BindLines(readOperationResult);
            Names = FindNames();
        }

        private OcrRequest OcrRequest { get; set; }
        private IList<Line> Lines { get; set; }
        private IEnumerable<string> Names { get; set; }

        private void BindLines(ReadOperationResult readOperationResult)
        {
            var lines = from page in readOperationResult.AnalyzeResult.ReadResults
                        from line in page.Lines
                        select line;
            Lines = lines.ToList();
        }

        private IEnumerable<string> FindNames()
        {
            var names = from line in Lines
                        where line.Text.Contains(OcrRequest.NameToken)
                        select line.Text;
            return names.ToList();
        }

        private string StripNameToken(string name)
        {
            string pat = @"\s*" + OcrRequest.NameToken + @"\s*$";
            return Regex.Replace(name, pat, "");
        }

        public IList<string> GetCleanNames()
        {
            var cleanNames = from name in FindNames()
                             select StripNameToken(name);
            return cleanNames.ToList();
        }

        public Dictionary<string, int> GetNameIndexes()
        {
            int count = 0;
            Dictionary<string, int> NameIndexes = new Dictionary<string, int>();

            foreach (var line in Lines)
            {
                if (Names.Contains(line.Text))
                {
                    NameIndexes.Add(line.Text, count);
                }
                count++;
            }

            return NameIndexes;
        }
    }
}
