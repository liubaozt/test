using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace BaseCommon.Data
{
    public class FileCombine
    {
        //public void CombineFile(ArrayList infileName, String outfileName)
        //{
        //    int b;
        //    int n = infileName.Count;
        //    FileStream[] fileIn = new FileStream[n];
        //    using (FileStream fileOut = new FileStream(outfileName, FileMode.Create))
        //    {
        //        for (int i = 0; i < n; i++)
        //        {
        //            try
        //            {
        //                fileIn[i] = new FileStream(infileName[i].ToString(),FileMode.Open);
        //                while ((b = fileIn[i].ReadByte()) != -1)
        //                    fileOut.WriteByte((byte)b);
        //            }
        //            catch (System.Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //            finally
        //            {
        //                fileIn[i].Close();
        //            }

        //        }
        //    }
        //}

        public void CombineFile(ArrayList infileName, String outfileName)
        {
            int n = infileName.Count;
            FileStream[] fileIn = new FileStream[n];
            StreamWriter sw = new StreamWriter(outfileName, false, Encoding.Default);
            try
            {
                for (int i = 0; i < n; i++)
                {

                    String fileString = File.ReadAllText(infileName[i].ToString(), Encoding.Default);
                    sw.WriteLine(fileString);

                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sw.Close();
            }

        }
    }
}
