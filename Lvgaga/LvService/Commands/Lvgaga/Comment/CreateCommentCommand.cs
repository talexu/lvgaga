using System.Threading.Tasks;
using LvService.Commands.Lvgaga.Common;
using LvService.Utilities;

namespace LvService.Commands.Lvgaga.Comment
{
    public class CreateCommentCommand : AbstractCreateLvEntityCommand
    {
        private string _partitionKey;
        private string _userId;
        private string _userName;
        private string _text;

        public override bool CanExecute(dynamic p)
        {
            _partitionKey = p.PartitionKey;
            _userId = p.UserId;
            _userName = p.UserName;
            _text = p.Text;
            return new[] { _partitionKey, _userId, _userName, _text }.AllNotNullOrEmpty();
        }

        public override Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return Task.FromResult(false);

            // Create CommentEntity
            var commentEntity = TableEntityFactory.CreateCommentEntity(p);
            if (commentEntity == null) return Task.FromResult(false);
            p.Entity = commentEntity;

            return Task.FromResult(true);
        }
    }
}