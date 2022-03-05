using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OcrPoc01.Models
{
    public class OrcResponse
    {
        [JsonProperty("names")]
        public IList<string> Names { get; set; }

        [JsonProperty("lineText")]
        public IList<string> LineText { get; set; }
    }

    public class Score
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("scores")]
        public List<int> Scores { get; set; }
    }

}
