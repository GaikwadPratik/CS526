﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Configuration;

namespace ImageSharingWithCloudStorage.DataAccessLayer
{
    public class ImageStorage
    {
        // Blob storage = true
        public static bool USE_BLOB_STORAGE = bool.Parse(ConfigurationManager.AppSettings["USE_BLOB_STORAGE"]);
        // Account details saved in web config
        public static string ACCOUNT = ConfigurationManager.AppSettings["AccountName"];
        // Account key saved in web config
        public static string AccountKey = ConfigurationManager.AppSettings["AccountKey"];
        public static string CONTAINER = ConfigurationManager.AppSettings["Container"];

        // Give access details to save images.
        public static void SaveFile(HttpServerUtilityBase server, HttpPostedFileBase imageFile, int imageId)
        {
            if (USE_BLOB_STORAGE)
            {
                StorageCredentials credentials = new StorageCredentials(ACCOUNT, AccountKey);
                CloudStorageAccount cs = new CloudStorageAccount(credentials, true);
                CloudStorageAccount.TryParse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString, out CloudStorageAccount account);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                CloudBlockBlob blob = container.GetBlockBlobReference(FilePath(null, imageId));
                blob.DeleteIfExists();
                blob.UploadFromStream(imageFile.InputStream);
            }
            else
            {
                imageFile.SaveAs(FilePath(server, imageId));
            }
        }

        public static string FilePath(HttpServerUtilityBase server, int imageId)
        {
            if (USE_BLOB_STORAGE)
            {
                return FileName(imageId);
            }
            else
            {
                string imgFileName = server.MapPath("~/Content/Images/" + FileName(imageId));
                return imgFileName;
            }
        }

        public static string FileName(int imageId)
        {
            return $"{imageId}.jpg";
        }

        // Image URI
        public static string ImageURI(UrlHelper urlHelper, int imageId)
        {
            string _strRtnVal = USE_BLOB_STORAGE
                ? $"https://{ACCOUNT}.blob.core.windows.net/{CONTAINER}/{ FileName(imageId)}"
                : urlHelper.Content("~/Content/Images/" + FileName(imageId));
            return _strRtnVal;
        }

        public static void DeleteAllBlob()
        {
            if (USE_BLOB_STORAGE)
            {
                StorageCredentials credentials = new StorageCredentials(ACCOUNT, AccountKey);
                CloudStorageAccount cs = new CloudStorageAccount(credentials, true);
                CloudStorageAccount.TryParse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString, out CloudStorageAccount account);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                System.Threading.Tasks.Parallel.ForEach(container.ListBlobs().Where(y => y.Uri.Segments.Last() != "1.jpg"), x => ((CloudBlob)x).Delete());
            }
        }

        // Delete Blob from azure account
        public static bool DeleteBlobs(List<int> lstImageIds, HttpServerUtilityBase server)
        {
            if (USE_BLOB_STORAGE)
            {
                StorageCredentials credentials = new StorageCredentials(ACCOUNT, AccountKey);
                CloudStorageAccount cs = new CloudStorageAccount(credentials, true);
                CloudStorageAccount.TryParse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString, out CloudStorageAccount account);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                foreach (var item in lstImageIds) { container.GetBlockBlobReference(FilePath(server, item)).DeleteIfExists(); }
            }
            return true;
        }

        public static bool DeleteBlob(int nImageIds, HttpServerUtilityBase server)
        {
            if (USE_BLOB_STORAGE)
            {
                StorageCredentials credentials = new StorageCredentials(ACCOUNT, AccountKey);
                CloudStorageAccount cs = new CloudStorageAccount(credentials, true);
                CloudStorageAccount.TryParse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString, out CloudStorageAccount account);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                container.GetBlockBlobReference(FilePath(server, nImageIds)).DeleteIfExists();
            }
            return true;
        }
    }
}