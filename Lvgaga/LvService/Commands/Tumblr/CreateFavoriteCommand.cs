using System;
using LvService.Commands.Common;

namespace LvService.Commands.Tumblr
{
    public class CreateFavoriteCommand : CreateLvEntityCommand
    {
        public CreateFavoriteCommand()
        {

        }

        public CreateFavoriteCommand(ICommand command)
            : base(command)
        {

        }
    }
}