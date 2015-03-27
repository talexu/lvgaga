﻿using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class DownloadToStreamCommand : DownloadToStreamCommandChain
    {
        protected CloudBlobContainer Container;
        protected string BlobName;

        public DownloadToStreamCommand()
        {

        }

        public DownloadToStreamCommand(IDownloadToStreamCommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                Container = p.Container;
                BlobName = p.BlobName;
                return Container != null && !String.IsNullOrEmpty(BlobName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<Stream> ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return null;

            var blockBlob = Container.GetBlockBlobReference(BlobName);
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    await blockBlob.DownloadToStreamAsync(memoryStream);
                    return memoryStream;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}