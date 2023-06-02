using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonFileTimestampApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFolder = @"C:\S3Innovate\Aquilia\JSONFileReaderAndExtractor\JSONFileReaderAndExtractor\bin\2023-04-11";
            //string sourceFolder = @"D:\AquiliaTest\TempFolder";
            string outputFolderUTC = @"D:\AquiliaTest\2023-04-11_Output_inUTC0";
            string outputFolderUTC8 = @"D:\AquiliaTest\2023-04-11_Output_inUTC8";

            string[] testTimestamps = new string[]
            {
                "4:28:36 PM 4/5/2023",
                "6:00:40 AM 4/11/2023",
                "5:51:46 AM 4/11/2023",
                "5:28:45 AM 4/11/2023",
                "5:25:24 AM 4/11/2023",
                "5:16:51 AM 4/11/2023",
                "4:54:32 AM 4/11/2023",
                "4:24:09 AM 4/11/2023",
                "2:38:43 AM 4/11/2023",
                "11:37:10 PM 4/10/2023",
                "10:57:51 PM 4/10/2023",
                "9:57:24 PM 4/10/2023",
                "9:02:28 AM 4/11/2023",
                "9:02:31 AM 4/11/2023",
                "9:44:48 AM 4/11/2023",
                "9:44:50 AM 4/11/2023",
                "9:46:37 AM 4/11/2023",
                "9:46:39 AM 4/11/2023",
                "9:48:55 AM 4/11/2023",
                "9:46:57 AM 4/11/2023",
                "9:54:36 AM 4/11/2023",
                "9:54:38 AM 4/11/2023",
                "9:55:54 AM 4/11/2023",
                "9:55:56 AM 4/11/2023",
                "1:10:23 AM 4/11/2023",
                "1:10:26 AM 4/11/2023",
                "4:40:25 AM 4/11/2023",
                "4:40:28 AM 4/11/2023",
                "4:40:28 AM 4/11/2023"
            };
            List<DataModel> dataModels = new List<DataModel>();
            List<DataModel> dataModelsUTC8 = new List<DataModel>();

            foreach (string timestamp in testTimestamps)
            {
                foreach (string filePath in Directory.GetFiles(sourceFolder, "*.json"))
                {
                    JArray jsonArray = JArray.Parse(File.ReadAllText(filePath));
                    Console.WriteLine(filePath + " is parsing");

                    foreach (JObject jsonObject in jsonArray)
                    {
                        DateTime jsonTimestamp = jsonObject.Value<DateTime>("Timestamp"); 

                        DateTime formattedTestTimestamp = DateTime.Parse(timestamp);
                        DateTime formattedTestTimestampUTC8 = formattedTestTimestamp.AddHours(-8);

                        var jsonTimeStampInString = jsonTimestamp.ToString();
                        var formattedTestTimestampInString = formattedTestTimestamp.ToString();
                         

                        if (formattedTestTimestampInString == jsonTimeStampInString)
                        {
                            DataModel dataModel = jsonObject.ToObject<DataModel>(); 
                            dataModels.Add(dataModel);

                            //File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(jsonObject));
                        }

                        var formattedTestTimestampUTC8InString = formattedTestTimestampUTC8.ToString();

                        if (formattedTestTimestampUTC8InString == jsonTimeStampInString)
                        {
                            DataModel dataModel = jsonObject.ToObject<DataModel>();
                            dataModelsUTC8.Add(dataModel);
                        }
                    }
                }
            }
            string outputFilePath = Path.Combine(outputFolderUTC,"output.json");
            File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(dataModels));

            string outputFilePathUTC8 = Path.Combine(outputFolderUTC8, "output.json");
            File.WriteAllText(outputFilePathUTC8, JsonConvert.SerializeObject(dataModelsUTC8));
        }
    }
    class DataModel
    {
        public string Id { get; set; }
        public string CameraId { get; set; }
        public string CameraName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public string State { get; set; }
        public string Timestamp { get; set; }
        public string Type { get; set; }
        public string Tag { get; set; }
        public string Vendor { get; set; }
    }
}
