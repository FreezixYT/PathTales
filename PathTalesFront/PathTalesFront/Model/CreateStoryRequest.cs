namespace PathTalesFront.Model
{
    public class CreateStoryRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string[] Categories { get; set; }   
    }
}