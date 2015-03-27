namespace LvModel.Common
{
    public class LvConstants
    {
        #region Blob

        public const string ContainerNameOfImage = "images";

        #endregion


        #region Storage Table

        public const string PartitionKey = "PartitionKey";
        public const string RowKey = "RowKey";
        public const string TableNameOfTumblr = "tumblrs";
        public static readonly string PartitionKeyOfImage = MediaType.Image.ToString("G");

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