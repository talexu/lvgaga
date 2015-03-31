using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class CreateCommentCommandTests : AzureStorageTestsBase
    {
        public CreateCommentCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {

        }

        [Fact]
        public async Task CreateComment_Return_CreatedEntity()
        {
            const string partitionKey = "Test pk";
            const string userId = "Test uid";
            const string userName = "Test uname";
            const string text = "Test text";

            dynamic pc = new ExpandoObject();
            pc.PartitionKey = partitionKey;
            pc.UserId = userId;
            pc.UserName = userName;
            pc.Text = text;

            await Fixture.CreateCommentCommand.ExecuteAsync(pc);
            CommentEntity entity = pc.Entity;
            Assert.NotNull(entity);
            Assert.Equal(partitionKey, entity.PartitionKey);
            Assert.Equal(userId, entity.UserId);
            Assert.Equal(userName, entity.UserName);
            Assert.Equal(text, entity.Text);
            Assert.Equal(DateTimeHelper.GetInvertedTicks(entity.CommentTime), entity.RowKey);
        }
    }
}