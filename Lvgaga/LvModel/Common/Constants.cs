namespace LvModel.Common
{
    public class Constants
    {
        #region Blob

        public const string ImageContainerName = "images";

        #endregion


        #region Storage Table

        public const string PartitionKey = "PartitionKey";
        public const string RowKey = "RowKey";
        public const string TumblrTableName = "tumblrs";
        public static readonly string ImagePartitionKey = MediaType.Image.ToString("G");

        #endregion
    }

    public enum MediaType
    {
        Image = 0,
        Audio = 1,
        Video = 2,
        Gif = 3
    }
}