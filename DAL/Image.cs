using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Security.Application;
using System.IO;
using ViewModel;
using Common;

namespace DAL
{
    public class Image
    {
        public string GetImageList(string imageFor, string editYN, string deleteYN, ref int err)
        {
            SqlCommand oCmd = new SqlCommand();
            string data = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetImageList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pImageFor", imageFor);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pEditYN", editYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDeleteYN", deleteYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 20000, "@pData", "");
                DBUtility.Execute(oCmd);
                data = oCmd.Parameters["@pData"].Value.ToString();
                err = 0;
            }
            catch (Exception ex)
            {
                data = ex.Message;
                err = 1;
            }

            return data;
        }

        public int SaveImage(string imageId, string imageFor, string imagePath, string fileExtension, string mode, int createdBy, string ipAddress, ref string errDesc)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveImage";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pImageId", Convert.ToInt64(GblFunctions.Base64Decode((imageId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pImageFor", imageFor);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pImagePath", imagePath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pFileExtension", fileExtension);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                errDesc = oCmd.Parameters["@pErrDesc"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }
            finally
            {
                oCmd = null;
            }

            return err;
        }

        public int deleteImages(string path, string files, ref string errDesc)
        {
            int err = 0;

            try
            {
                string[] allfiles = files.Split(',');

                for (int i = 0; i < allfiles.Length; i++)
                {
                    if (File.Exists(path + allfiles[i]))
                    {
                        File.Delete(path + allfiles[i]);
                    }
                }

                err = 0;
                errDesc = string.Empty;
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }
            return err;
        }

        public string GetPhotogalleryList(string imageFor, ref int err, ref string pageLink)
        {
            SqlCommand oCmd = new SqlCommand();
            string data = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPhotogalleryList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pImageFor", imageFor);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pPageLink", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 20000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pageLink = oCmd.Parameters["@pPageLink"].Value.ToString();
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                data = ex.Message;
                err = 1;
            }

            return data;
        }
    }
}
