namespace LvModel.Common
{
    public class Constants
    {
        public const string PartitionKey = "PartitionKey";
        public const string TumblrTableName = "tumblrs";
        public static readonly string MediaTypeImage = MediaType.Image.ToString("G");
    }

    public enum MediaType
    {
        Image = 0,
        Audio = 1,
        Video = 2,
        Gif = 3
    }
}