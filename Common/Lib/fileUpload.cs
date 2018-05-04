using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Common.Lib
{
    public class fileUpload
    {
        #region 업로드 파일명이 중복될경우 새로운 이름을 주자
        public static string GetUniqueFileNameWithPath(string dirPath, string fileName, out string lastFileName)
        {
            int indexOfDot = fileName.LastIndexOf(".");             //파일이름에서 . 의 위치를 알아낸다.
            string strName = fileName.Substring(0, indexOfDot);     //파일명만을 떼어낸다
            string strExt = fileName.Substring(++indexOfDot);       //파일의 확장자만을 떼어낸다.

            bool bExist = true;   //같은 이름의 파일의 존재여부, 우선 있다고 가정.
            int fileCount = 0;

            while (bExist)
            {
                if (File.Exists(Path.Combine(dirPath, fileName)))
                {
                    fileCount++;
                    fileName = strName + "_" + fileCount + "." + strExt;
                }
                else
                {
                    bExist = false;
                }
            }
            lastFileName = fileName;
            return Path.Combine(dirPath, fileName);
        }
        #endregion

        #region 저장된폴더의 쓸데없는 임시 파일을 지운다.
        public static string DeleteTempFile(string fullPathWithfileName)
        {
            try
            {
                File.Delete(fullPathWithfileName);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region 저장된폴더의 이미지 파일을 -> 새로운 파일명으로 바꿔준다.
        public static string ReNameFile(string dirPath, string FullPahtOldFileName, string newFileName)
        {
            try
            {
                string fullPathWithNewFileName = dirPath + "\\" + newFileName;
                System.IO.File.Move(FullPahtOldFileName, fullPathWithNewFileName);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region 특정 파일의 size 를 알아온다.
        public static long GetFileSize(string fullPathWithfileName)
        {
            try
            {
                FileInfo finfo = new FileInfo(fullPathWithfileName);
                long fSize = finfo.Length;
                return fSize;
            }
            catch (Exception)
            {
                return -1; ;
            }
        }
        #endregion
    }

}
