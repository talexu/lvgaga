namespace LvModel.View.Tumblr
{
    public class TumblrViewModel
    {
        public string Text { get; set; }
        public TumblrCategory Category { get; set; }
    }

    public enum TumblrCategory
    {
        C1 = 1,
        C2 = 2,
        C3 = 3,
        C4 = 4,
        C5 = 5
    }
}