using CsvHelper;
using CsvHelper.Configuration;
using SomNet.Models;
using System.Globalization;
using System.Xml.Linq;
using SomNet.Models;
using System.IO;

namespace SomNet.Utilities
{
    public class StorageToList
    {
        //CSV
        public IEnumerable<Sta70Data> GetFromCsv(string PATH)
        {
            //Config info for stream reader
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false,
                Comment = '#',
                AllowComments = true,
                Delimiter = ",",
                MissingFieldFound = null,
                TrimOptions = TrimOptions.None,
            };

            //THIS NEEDS CATCHING IF THE PATH CANNOT BE FOUND
            //Stream reader
            FileStream fileStream = new FileStream(PATH, FileMode.Open);
            fileStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(fileStream);
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            //
            var csvReader = new CsvReader(streamReader, csvConfig);
            //csvReader.ColumnCount
            Console.WriteLine("Column count: {0}", csvReader.ColumnCount);
            //
            var result = csvReader.GetRecords<Sta70Data>();

            //fileStream.Close();
            //streamReader.Close();
            return result;
        }

        ////SQL
        //public async Task<IEnumerable<MessageData>> ConnectToSql(string PATH)
        //{
        //}
    }
}
