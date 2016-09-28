using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Excel;
using System.IO;
using OfficeOpenXml;

namespace Manager.Helpers
{
    public interface IExporterImporter
    {
        IEnumerable<DataRow> Open(string path);
        bool Download(DataTable data, string fileName);
    }

    public class ExcelImportExport : IExporterImporter
    {
        public bool success { get; set; }
        public string message { get; set; }

        private void SetEmptyFileMsg()
        {
            success = false;
            message = "Selecione arquivo para Upload";
        }

        private void SetInvalidFile()
        {
            success = false;
            message = "Formato de arquivo não suportado.";
        }

        // ExcelDataReader works with the binary Excel file, so it needs a FileStream
        // to get started. This is how we avoid dependencies on ACE or Interop:
        private IExcelDataReader LoadExcelStream(string fileName, Stream pStream)
        {
            if (fileName.EndsWith(".xls"))            
                return ExcelReaderFactory.CreateBinaryReader(pStream);
            
            else if (fileName.EndsWith(".xlsx"))            
                return ExcelReaderFactory.CreateOpenXmlReader(pStream);
            
            SetInvalidFile();
            return null;

        }

        // Read data from Stream and returns a DataRow Collection
        private IEnumerable<DataRow> ReadData(IExcelDataReader reader)
        {

            reader.IsFirstRowAsColumnNames = true;

            DataSet dsExcel = reader.AsDataSet();
            DataTable dt = dsExcel.Tables[0];

            reader.Close();

            IEnumerable<DataRow> rows = from DataRow row in dt.Rows
                                        select row;

            success = true;
            message = "Arquivo excel carregado com sucesso";

            return rows;

        }

        //import from Excel

        public IEnumerable<DataRow> Open(HttpPostedFileBase pExcelFile)
        {
            IExcelDataReader reader = null;
            if (pExcelFile != null && pExcelFile.ContentLength > 0)
            {
                reader = LoadExcelStream(pExcelFile.FileName, pExcelFile.InputStream); // We return the interface, so that

                if(reader != null)
                    return ReadData(reader);
            }                
            else
                SetEmptyFileMsg();

            return null;

        }

        public IEnumerable<DataRow> Open(string path)
        {
            IExcelDataReader reader = null;

            if (File.Exists(path))
            {
                Stream stream = (Stream)File.Open(path, FileMode.Open);
                reader = LoadExcelStream(path, stream); // We return the interface, so that
                return ReadData(reader);
            }
            else
                SetEmptyFileMsg();

            return null;

        }

        public bool Download(DataTable data, string fileName)
        {
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Plan1");

            workSheet.Cells[1, 1].LoadFromDataTable(data, true);
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

            using (var memoryStream = new MemoryStream())
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}.xlsx", fileName));
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

            return true;
        }
    }
}