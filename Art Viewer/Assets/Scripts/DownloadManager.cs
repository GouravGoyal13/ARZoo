using System.Net;
using System.ComponentModel;
using System.IO;
using System;
using UnityEngine;

public class DownloadManager : MonoBehaviour
{
    static readonly string ResourcesRootPath = "Assets/Resources/";
    string remoteAssetBasePath = "https://griptonite.s3.amazonaws.com/deerhunt2";
    private WebClient client;
    public bool isExistOnDisk;
    string uri;
    public string newFileName = "example.zip";
    //  string localAssetPath;
    string assetDirPath;
    string localZipPath;
    public string ResourceFullPath
    {
        get
        {
            return Path.GetFullPath(ResourcesRootPath);
        }
    }

    public void Start()
    {
        DownloadFile(
            "Unused/crocodile.fbx",
            (path) =>
            {
                Debug.Log("DownloadComplete " + path);
            },
            (progress) =>
            {

                Debug.Log("DownloadProgress" + progress.ProgressPercentage);
            });
    }

    void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            Debug.Log("Download Canceled");
        }
        else
        {
            if (e.Error == null)
            {
                Debug.Log("Download Completed");
            }
            else
            {
                string error = e.Error.ToString();
                Debug.Log(error);
            }
        }
    }

    bool CheckIfFileExist(string filePath)
    {
        return Directory.Exists(filePath);
        // string directory = Path.GetDirectoryName(filePath);
        // {
        //     if (directory == Path.GetDirectoryName(filePath))
        //     {
        //         return true;
        //     }
        // return false;
    }

    public static bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            {
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }
    public void DownloadFile(string filePath, Action<string> OnDownloadCompleted, Action<DownloadProgressChangedEventArgs> downloadProgress)
    {
        uri = Path.Combine(remoteAssetBasePath, filePath);
        uri = Path.ChangeExtension(uri, "zip");
        newFileName = Path.GetFileName(uri);
        string dirName = Path.GetFileNameWithoutExtension(newFileName);

        assetDirPath = Path.Combine(Path.GetFullPath(ResourcesRootPath), dirName);
        localZipPath = assetDirPath + "/" + newFileName;
        isExistOnDisk = CheckIfFileExist(assetDirPath);
        if (isExistOnDisk)
        {
            OnDownloadCompleted(Path.GetFileNameWithoutExtension(localZipPath));
            return;
        }
        else
        {
            Directory.CreateDirectory(Path.Combine(ResourcesRootPath, Path.GetFileNameWithoutExtension(newFileName)));
            DownloadFile((path) =>
            {
                Debug.Log("DownloadComplete " + path);
                Debug.Log("Start UnZipping");
                string outDir;
                ZipFilesUtility zipUtil = new ZipFilesUtility(path);
                zipUtil.UnZip(out outDir);
                Debug.Log("Unzipped at" + outDir);
                string dirPath = Path.GetDirectoryName(localZipPath);
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                }
                OnDownloadCompleted(path);
            },
                        downloadProgress);
        }
    }
    public void DownloadFile(Action<string> OnDownloadCompleted, Action<DownloadProgressChangedEventArgs> downloadProgress)
    {
        client = new WebClient();

        client.DownloadFileAsync(new System.Uri(uri), localZipPath);

        client.DownloadProgressChanged += (sender, e) =>
        {
            downloadProgress(e);
        };
        client.DownloadFileCompleted += (sender, e) =>
        {
            if (e.Error == null)
            {
                OnDownloadCompleted(Path.GetFileNameWithoutExtension(localZipPath));
            }
            else
            {
                string dirPath = Path.GetDirectoryName(localZipPath);
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                }
                Debug.Log(e.Error.Message);
                if (client != null)
                {
                    client.CancelAsync();
                }
            }
        };
    }
    void OnApplicationQuit()
    {
        if (client != null)
        {
            client.CancelAsync();
        }
        string dirPath = Path.GetDirectoryName(localZipPath);
        if (Directory.Exists(dirPath))
        {
            Directory.Delete(dirPath, true);
        }
    }
}
