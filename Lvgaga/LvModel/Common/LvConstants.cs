namespace LvModel.Common
{
    public class LvConstants
    {
        #region Blob

        public const string ContainerNameOfImage = "images";
        public const string ContainerNameOfThumbnail = "thumbnails";

        #endregion

        #region Storage Table

        public const string PartitionKey = "PartitionKey";
        public const string RowKey = "RowKey";
        public const string TableNameOfTumblr = "tumblrs";
        public const string TableNameOfComment = "comments";
        public const string TableNameOfFavorite = "favorites";
        public static readonly string PartitionKeyOfAll = MediaType.All.ToString("D");
        public static readonly string PartitionKeyOfImage = MediaType.Image.ToString("D");

        #endregion
    }

    public enum MediaType
    {
        All = 0,
        Image = 1,
        Audio = 2,
        Video = 3,
        Gif = 4
    }
}