﻿using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using LvService.Utilities;

namespace LvService.Commands.Tumblr
{
    public class CreateCommentCommand : CreateLvEntityCommand
    {
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
            _partitionKey = p.PartitionKey;
            _userId = p.UserId;
            _userName = p.UserName;
            _text = p.Text;
            return new[] { _partitionKey, _userId, _userName, _text }.AllNotNullOrEmpty();
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