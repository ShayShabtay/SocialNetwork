using Amazon;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Amazon.S3.Model;

namespace UI.Storage
{
    public class StorageHelper
    {
        private const string bucketName = "facelook-bucket";
        private const string keyName = "testfile.png";
        private const string filePath = "test1";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUWest1;
        //string hostURL = bucketRegion.GetEndpointForService("s3").Hostname;
        string hostUrl = "https://s3-eu-west-1.amazonaws.com/";

        private static IAmazonS3 s3Client { get; set; }

        public StorageHelper()
        {
            s3Client = new AmazonS3Client(RegionEndpoint.EUWest1);
        }

        public void Init(string bucketName, string keyName, string filePath)
        {

        }

        public string UploadImageToS3(Stream inputStream, string userId, string ImageKey)
        {
            try
            {
                using (s3Client)
                {
                    var request = new PutObjectRequest();
                    request.BucketName = bucketName + "/" + userId;
                    request.Key = ImageKey;
                    request.CannedACL = S3CannedACL.PublicReadWrite;

                    request.InputStream = inputStream;
                    var res = s3Client.PutObject(request);

                    if (res.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return hostUrl + "/" + bucketName + "/" + userId + "/" + ImageKey;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}