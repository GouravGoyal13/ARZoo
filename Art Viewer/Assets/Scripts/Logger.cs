using System;
using System.IO;
class Logger
{
    public static bool WriteLog(string strMessage)
    {
        try
        {
            string strFileName="ConsoleLog";
            FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", Path.GetTempPath(), strFileName), FileMode.Append, FileAccess.Write);
            StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
            objStreamWriter.WriteLine(strMessage);
            objStreamWriter.Close();
            objFilestream.Close();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}