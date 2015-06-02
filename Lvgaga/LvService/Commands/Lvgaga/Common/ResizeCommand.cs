using System;
using System.IO;
using System.Threading.Tasks;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;
using LvService.Commands.Common;

namespace LvService.Commands.Lvgaga.Common
{
    public class ResizeCommand : ICommand
    {
        private Stream _stream;
        private readonly int _size;
        private readonly ScalingBase _scaling;

        public ResizeCommand(ScalingType scalingType, int size)
        {
            _size = size;
            switch (scalingType)
            {
                case ScalingType.CropScaling:
                    _scaling = new CropScaling(_size, _size);
                    break;
                case ScalingType.FitScaling:
                    _scaling = new FitScaling(_size, _size);
                    break;
                case ScalingType.PadScaling:
                    _scaling = new PadScaling(_size, _size);
                    break;
            }
        }

        public bool CanExecute(dynamic p)
        {
            _stream = p.Stream;
            return _stream != null;
        }

        public Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return Task.FromResult(false);

            var image = new KalikoImage(_stream);
            if (Math.Max(image.Height, image.Width) < _size)
            {
                _stream.Position = 0;
                return Task.FromResult(false);
            }

            var thumb = image.Scale(_scaling);
            var ms = new MemoryStream();
            thumb.SaveJpg(ms, 99);
            ms.Position = 0;

            p.Stream = ms;

            return Task.FromResult(true);
        }

        public enum ScalingType
        {
            CropScaling,
            FitScaling,
            PadScaling
        }
    }
}