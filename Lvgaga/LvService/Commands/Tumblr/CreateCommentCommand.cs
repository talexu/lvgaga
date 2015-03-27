using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using LvService.Factories.Azure.Storage;

namespace LvService.Commands.Tumblr
{
    public class CreateCommentCommand : CommandChain
    {
        public ITableEntityFactory TableEntityFactory { get; set; }

        private string _partitionKey;
        private string _userId;
        private string _userName;
        private string _text;

        public CreateCommentCommand()
        {

        }

        public CreateCommentCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                _partitionKey = p.PartitionKey;
                _userId = p.UserId;
                _userName = p.UserName;
                _text = p.Text;
                return !String.IsNullOrEmpty(_partitionKey) &&
                       !String.IsNullOrEmpty(_userId) &&
                       !String.IsNullOrEmpty(_userName) &&
                       !String.IsNullOrEmpty(_text);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            // Create CommentEntity
            var commentEntity = TableEntityFactory.CreateCommentEntity(p);
            if (commentEntity == null) return;
            p.Entity = commentEntity;
        }
    }
}