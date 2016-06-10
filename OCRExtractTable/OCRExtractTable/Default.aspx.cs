using MODI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
            if (!IsPostBack)
            {
            }
        }

        protected void btnOCRReader_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/uploads/" + Path.GetFileName(hdnUploadedImage.Value));

            // Crop Image Here & Save
            string fileName = Path.GetFileName(filePath);
            string cropFileName = "";
            string cropFilePath = "";
            if (File.Exists(filePath))
            {
                System.Drawing.Image orgImg = System.Drawing.Image.FromFile(filePath);
                Rectangle CropArea = new Rectangle(
                    Convert.ToInt32(X.Value),
                    Convert.ToInt32(Y.Value),
                    Convert.ToInt32(W.Value),
                    Convert.ToInt32(H.Value));
                try
                {
                    Bitmap bitMap = new Bitmap(CropArea.Width, CropArea.Height);
                    using (Graphics g = Graphics.FromImage(bitMap))
                    {
                        g.DrawImage(orgImg, new Rectangle(0, 0, bitMap.Width, bitMap.Height), CropArea, GraphicsUnit.Pixel);
                    }
                    cropFileName = "crop_" + fileName;
                    cropFilePath = Path.Combine(Server.MapPath("~/uploads"), cropFileName);
                    bitMap.Save(cropFilePath);
                    //Response.Redirect("~/UploadImages/" + cropFileName, false);
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }
            string extractText = this.ExtractTextFromImage(cropFilePath);
            lblText.Text = extractText.Replace(Environment.NewLine, "<br />");
        }

        private string ExtractTextFromImage(string filePath)
        {
            Document modiDocument = new Document();
            modiDocument.Create(filePath);
            modiDocument.OCR(MiLANGUAGES.miLANG_ENGLISH);
            MODI.Image modiImage = (modiDocument.Images[0] as MODI.Image);

            DataTable dt = new DataTable();
           
            int totalColumns = 0;
            int.TryParse(txtColumns.Text, out totalColumns);

            for (int i = 1; i <= totalColumns; i++)
			{
                dt.Columns.Add("Column" + i);
			}
            List<string> lstdata = new List<string>();
            lstdata = modiImage.Layout.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            for (int i = 0; i < lstdata.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < totalColumns; j++)
                {
                    dr[j] = lstdata[i];
                    i++;

                    if(i>=lstdata.Count)
                    {
                        break;
                    }
                }
                i--;
                dt.Rows.Add(dr);
            }

            GenerateReport(dt);
            string extractedText = modiImage.Layout.Text;
            modiDocument.Close();
            return extractedText;
        }



        #region--Generate Excel--
        protected void GenerateReport(DataTable dt)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ToDaysOfferAddedReport" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xls"));
            Response.ContentType = "application/ms-excel";

            string tab = string.Empty;

            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }

          
            Response.End();
        }
        #endregion


    }
}