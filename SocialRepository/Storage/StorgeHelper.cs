using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Amazon.S3;
//using Amazon.S3.Transfer;
using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;

namespace SocialRepository.Storage
{
    public class StorgeHelper
    {
        private const string bucketName = "omer-buckets";
        private const string keyName = "testfile.png";
        private const string filePath = "test1";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
        private static IAmazonS3 s3Client;

        public StorgeHelper()
        {
            s3Client = new AmazonS3Client(RegionEndpoint.USEast2);
        }

        public void init(string bucketName, string keyName, string filePath)
        {

        }

        public void uploadTest(byte[] bArray)
        {

            byte[] imageData = File.ReadAllBytes("C://‏‏darkstar.PNG");
            bArray = imageData;
            string base64String = Convert.ToBase64String(bArray);

            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);

                using (s3Client)
                {
                    var request = new PutObjectRequest();
                    request.BucketName = "omer-buckets";
                    //request.ContentType = ;
                    request.Key = keyName;
                    //request.InputStream = file.InputStream;

                    using (var ms = new MemoryStream(bytes))
                    {
                        request.InputStream = ms;
                       s3Client.PutObject(request);
                      
                        //Debug.WriteLine(x.ToString());
                       // var res=await s3Client.PutObjectAsync(request);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AWS Fail");
            }
        }
    }
}
