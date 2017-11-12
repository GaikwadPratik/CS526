using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using ImageSharingWithCloudStorage.Models;
using Microsoft.Azure;
using System.Configuration;
using System.Linq.Expressions;

namespace ImageSharingWithCloudStorage.DataAccessLayer
{
    public class LogContext : TableEntity
    {
        public const string LOG_TABLE_NAME = "imageviews";

        public static void AddLogEntry(string user, ImageViewModel image)
        {
            LogEntry entry = new LogEntry(image.Id);
            entry.UserId = user;
            entry.Caption = image.Caption;
            entry.ImageId = image.Id;
            entry.Uri = image.Uri;
            TableOperation insertObject = TableOperation.Insert(entry);
            CloudTable table = CreateTable();
            table.Execute(insertObject);
        }

        public static IEnumerable<LogEntry> Select(DateTime dateTaken)
        {
            CloudTable table = CreateTable();

            TableQuery<LogEntry> query = new TableQuery<LogEntry>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, dateTaken.ToString("MMddyyyy")));
            return table.ExecuteQuery(query);
        }

        protected static CloudTableClient GetClient()
        {
            CloudStorageAccount.TryParse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString, out CloudStorageAccount account);
            CloudTableClient client = account.CreateCloudTableClient();
            return client;
        }
        // Create Table
        public static CloudTable CreateTable()
        {
            CloudTableClient client = GetClient();
            CloudTable table = client.GetTableReference(LOG_TABLE_NAME);
            table.CreateIfNotExists();
            return table;
        }

        // Delete the log Details
        public static void DeleteLog()
        {
            CloudTableClient client = GetClient();
            CloudTable table = client.GetTableReference(LOG_TABLE_NAME);
            if (table.Exists())
                table.Delete();
        }
    }
}