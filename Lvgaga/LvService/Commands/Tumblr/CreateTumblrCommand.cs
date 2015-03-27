using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.Factories.Azure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class CreateTumblrCommand : CommandChain
    {
        public ITableEntityFactory TableEntityFactory { get; set; }

        private string _partitionKey;
        private string _mediaUri;
        private TumblrText _tumblrText;

        public CreateTumblrCommand()
        {

        }

        public CreateTumblrCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                _partitionKey = p.PartitionKey;
                _mediaUri = p.MediaUri;
                _tumblrText = p.TumblrText;
                return !String.IsNullOrEmpty(_partitionKey) &&
                       !String.IsNullOrEmpty(_mediaUri) &&
                       _tumblrText != null;
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

            // Create TumblrEntity
            var tumblrEntity = TableEntityFactory.CreateTumblrEntity(p);
            if (tumblrEntity == null) return;
            p.Entity = tumblrEntity;

            // Create the copy of TumblrEntity with the category of all
            if (_tumblrText.Category.Equals(TumblrCategory.All))
            {
                p.Entities = new List<ITableEntity> { tumblrEntity };
            }
            else
            {
                var tempCategory = _tumblrText.Category;
                _tumblrText.Category = TumblrCategory.All;
                var tumblrEntityCateOfAll = TableEntityFactory.CreateTumblrEntity(p);
                if (tumblrEntityCateOfAll == null) return;
                p.Entities = new List<ITableEntity> { tumblrEntity, tumblrEntityCateOfAll };
                _tumblrText.Category = tempCategory;
            }
        }
    }
}