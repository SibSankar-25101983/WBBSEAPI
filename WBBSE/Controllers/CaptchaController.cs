using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Common;

namespace WBBSE.Controllers
{
    /*BASIC FUNCTION: HANDELS CAPTCHA IMAGE GENERATION AT SERVER END*/
    public class CaptchaController : Controller
    {
        /*GENERATES THE CAPTCHA IMAGE AND STORE THE VALUE IN SESSION AND RETURN THE CODE AS IMAGE(.JPEG) WITH SOME NOISE*/
        public ActionResult CaptchaImage()
        {
            /*NOISE PARAMETER TO CONTROL THE NOISE*/
            bool noisy = true;
            var rand = new Random((int)DateTime.Now.Ticks);
            //GENERATE NEW QUESTION 
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            //var captcha = string.Format("{0} + {1} = ?", a, b);

            var captcha = GblFunctions.GenerateRandomCode();

            //STORE ANSWER IN SESSION
            Session[SessionNames.Captcha] = captcha;

            //IMAGE STREAM 
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(180, 40))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //ADD NOISE
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, (x - r), (y - r), r, r);

                    }
                }

                //ADD QUESTION 
                gfx.DrawString(captcha, new Font("Century Schoolbook", 15), Brushes.Gray, 42, 6);

                //RENDER AS JPEG FROM BITMAP FORMAT
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            /*RETURNING THE FILE CONTENT TO THE SPECIFIC VIEW*/
            return Content("data:image/jpeg;base64," + Convert.ToBase64String(img.FileContents));
        }
    }
}
