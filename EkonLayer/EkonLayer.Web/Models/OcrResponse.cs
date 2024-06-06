namespace EkonLayer.Web.Models
{
    public class OcrResponse
    {
        public List<string> DetectedText { get; set; }
        public string ProcessedImageUrl { get; set; }
        public string AudioFileUrl { get; set; }
    }
}
