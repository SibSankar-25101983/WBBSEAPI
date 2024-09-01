using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.Generic;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Areas.Admin.Models
{
    public class MBooks
    {
        public List<VMMstBooks> getMstBookList(int? page, int? limit, string sortBy, string direction, ref int total, string searchString)
        {
            DataTable dt = new DataTable();
            List<VMMstBooks> result = new List<VMMstBooks>();

            try
            {
                dt = new Books().GetMstBookList(searchString, page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstBooks
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   BookId = data.Field<string>("BookId"),
                                   BookName = data.Field<string>("BookName"),
                                   BookCode = data.Field<string>("BookCode"),
                                   Class = data.Field<string>("Class"),
                                   SchoolMediumId = data.Field<string>("SchoolMediumId"),
                                   SchoolMediumName = data.Field<string>("SchoolMediumName"),
                                   BookPrice = data.Field<string>("BookPrice")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetBookList/getMstBookList(MBooks)", "AdminBooksController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int saveMstBook(VMMstBooks data, ref string errDesc)
        {
            int err = 0, createdBy = 0, bookId = 0, mediumId = 0;
            float bookPrice = 0;
            string bookName = string.Empty, bookCode = string.Empty, subjectClass = string.Empty;
            string mode = string.Empty, ipAddress = string.Empty;

            try
            {
                mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                bookId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.BookId)));

                if (mode != Mode.DELETE)
                {
                    //book name
                    bookName = Sanitizer.GetSafeHtmlFragment((data.BookName ?? string.Empty).Trim());
                    if (string.IsNullOrEmpty(bookName))
                    {
                        err = 2;
                        errDesc = Message.Books.BookNameRequired;
                        return err;
                    }
                    if (!GblFunctions.chkDataFormat(RegexType.Alpha, bookName))
                    {
                        err = 2;
                        errDesc = Message.Books.InvalidBookName;
                        return err;
                    }

                    //book code
                    bookCode = Sanitizer.GetSafeHtmlFragment((data.BookCode ?? string.Empty).Trim());
                    if (string.IsNullOrEmpty(bookCode))
                    {
                        err = 2;
                        errDesc = Message.Books.BookCodeRequired;
                        return err;
                    }
                    if (!GblFunctions.chkDataFormat(RegexType.AlphaWithoutSpace, bookCode))
                    {
                        err = 2;
                        errDesc = Message.Books.InvalidBookCode;
                        return err;
                    }

                    //class
                    subjectClass = Sanitizer.GetSafeHtmlFragment((data.Class ?? string.Empty).Trim());
                    if (string.IsNullOrEmpty(subjectClass))
                    {
                        err = 2;
                        errDesc = Message.Books.SubjectClassRequired;
                        return err;
                    }
                    if (subjectClass != SubjectClass.V && subjectClass != SubjectClass.VI && subjectClass != SubjectClass.VII && subjectClass != SubjectClass.VIII && subjectClass != SubjectClass.IX && subjectClass != SubjectClass.X)
                    {
                        err = 2;
                        errDesc = Message.Books.InvalidSubjectClass;
                        return err;
                    }

                    //medium
                    try
                    {
                        mediumId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolMediumId)));

                        if(mediumId <= 0)
                        {
                            err = 2;
                            errDesc = Message.Books.SchoolMediumRequired;
                            return err;
                        }
                    }
                    catch
                    {
                        err = 2;
                        errDesc = Message.Books.InvalidSchoolMedium;
                        return err;
                    }

                    //price
                    try
                    {
                        bookPrice = Convert.ToSingle(Sanitizer.GetSafeHtmlFragment(data.BookPrice));

                        if(bookPrice <= 0)
                        {
                            err = 2;
                            errDesc = Message.Books.BookPriceRequired;
                            return err;
                        }
                    }
                    catch
                    {
                        err = 2;
                        errDesc = Message.Books.InvalidBookPrice;
                        return err;
                    }
                }

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new Books().SaveMstBook(bookId, bookName, bookCode, subjectClass, mediumId, bookPrice, mode, ipAddress, createdBy, ref errDesc);

                if(err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "BookList/saveMstBook(MBooks)", "AdminBooksController");
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "BookList/saveMstBook(MBooks)", "AdminBooksController");
            }

            return err;
        }
    }
}
