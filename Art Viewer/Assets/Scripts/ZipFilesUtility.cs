using System;
using System.IO;
// using Ionic.Zlib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

public class ZipFilesUtility
{
    private string m_InputDir;
    private string m_CompressedDir;
	public ZipFilesUtility(string inDir)
    {
        m_InputDir = inDir;
    }
	public void UnZip(out string outDirPath)
	{
		outDirPath = null;
		ZipFile zf = null;
		string m_CompressedDir = null;
		try
		{
			FileStream fs = File.OpenRead (m_InputDir);
			zf = new ZipFile(fs);
			outDirPath = Path.Combine (Path.GetDirectoryName (zf.Name),Path.GetDirectoryName (zf[0].Name)); 
			
			foreach (ZipEntry zipEntry in zf) 
			{
				if (!zipEntry.IsFile) 
				{
					continue;			// Ignore directories
				}
				String entryFileName = zipEntry.Name;
//				entryFileName = Path.GetFileName(entryFileName);
				
				byte[] buffer = new byte[4096];		// 4K is optimum
				Stream zipStream = zf.GetInputStream(zipEntry);
				m_CompressedDir = Path.GetDirectoryName (m_InputDir);
				// Manipulate the output filename here as desired.
				String fullZipToPath = Path.Combine(m_CompressedDir, entryFileName);
				string directoryName = Path.GetDirectoryName(fullZipToPath);
				if (directoryName.Length > 0)
					Directory.CreateDirectory(directoryName);
				// The "using" will close the stream even if an exception occurs.
				using (FileStream streamWriter = File.Create(fullZipToPath)) 
				{
					StreamUtils.Copy(zipStream, streamWriter, buffer);
				}
			}
		} 
		finally 
		{
			if (zf != null) 
			{
				zf.IsStreamOwner = true; // Makes close also shut the underlying stream
				zf.Close(); // Ensure we release resources
			}
		}
	}
}
