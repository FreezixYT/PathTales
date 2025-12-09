namespace PathTalesFront.Model
{
    public class CreateStoryRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public string userId { get; set; }
        public string paragraphs { get; set; } = null;
        public string status { get; set; } = "hidden";
        public string categories { get; set; }
        public int readCount { get; set; } = 0;
    }
}