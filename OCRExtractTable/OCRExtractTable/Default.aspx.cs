using MODI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OCRExtractTable
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOCRReader_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/uploads/" + Path.GetFileName(hdnUploadedImage.Value));
            
            string extractText = this.ExtractTextFromImage(filePath);
            lblText.Text = extractText.Replace(Environment.NewLine, "<br />");
        }

        private string ExtractTextFromImage(string filePath)
        {
            Document modiDocument = new Document();
            modiDocument.Create(filePath);
            modiDocument.OCR(MiLANGUAGES.miLANG_ENGLISH);
            MODI.Image modiImage = (modiDocument.Images[0] as MODI.Image);
            string extractedText = modiImage.Layout.Text;
            modiDocument.Close();
            return extractedText;
        }
    }
}