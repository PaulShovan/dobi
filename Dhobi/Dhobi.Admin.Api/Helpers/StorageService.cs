﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Configuration;
using System.IO;

namespace Dhobi.Admin.Api.Helpers
{
    public class StorageService
    {
        private IAmazonS3 client = null;

        public StorageService()
        {
            string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            string secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            if (this.client == null)
            {
                this.client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USEast2); // Amazon.AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, RegionEndpoint.);
            }
        }

        public bool UploadFile(string awsBucketName, string key, Stream stream)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                BucketName = awsBucketName,
                CannedACL = S3CannedACL.PublicRead,
                Key = key
            };

            TransferUtility fileTransferUtility = new TransferUtility(this.client);
            fileTransferUtility.Upload(uploadRequest);
            return true;
        }
    }
}