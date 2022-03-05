using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WestUs3_OCR_OPC.Models
{
    public class OcrRequest
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("modelVersion")]
        public string ModelVersion { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("readingOrder")]
        public string readingOrder { get; set; }
        [JsonProperty("nameToken")]
        public string NameToken { get; set; }
        [JsonProperty("scoreCardName")]
        public string ScoreCardName { get; set; }
    }
}
