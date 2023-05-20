using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Quotation;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Falcon.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Wp = DocumentFormat.OpenXml.Wordprocessing;
using OXML = DocumentFormat.OpenXml;

namespace DaiPhucVinh.Services.MainServices.Word
{
    public class TableData
    {
        public TableData()
        {
            Headers = new List<string>();
            DataRows = new DataTable();
        }
        public List<string> Headers { get; set; }
        public DataTable DataRows { get; set; }
    }
    public interface IWordStreamService
    {
        string ExportQuotation(string templateName, WordStreamResponse request);
        string ExportHopDong(string templateName, WordStreamHopDongResponse request);
        string ExportHopDongMuaBan(string templateName, WordStreamHopDongResponse request);
    }
    public class WordStreamService : IWordStreamService
    {
        private readonly DataContext _datacontext;
        private const string Model = "Model";
        private const string Tskt = "Thông số kỹ thuật";
        private const string Xuatxu = "Xuất xứ";
        private const string GioiThieu = "Giới thiệu";
        private const string Link = "Link Video";
        public WordStreamService(DataContext datacontext)
        {
            _datacontext = datacontext;
        }
        public string NumberToText(double inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return result + (suffix ? " đồng " : "");
        }
        public string ExportQuotation(string templateName, WordStreamResponse request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            DataTable dt = new DataTable();
            var dataCols = new List<string>
            {
                "CongTy",
                "DiaChi",
                "DienThoai",
                "Email",
                "NguoiGui",
                "NguoiGui_ChucVu",
                "NguoiGui_DienThoai",
                "NguoiGui_Email",
                "TienTruocThue",
                "VAT",
                "TienThue",
                "TongTien",
                "GhiChu",
                "PTThanhToan",
                "PTGiaoNhan",
                "PTBaoHanh",
                "Ngay",
                "Thang",
                "Nam",
            };
            dataCols.ForEach(cols => dt.Columns.Add(cols));
            TableData tableData = new TableData
            {
                Headers = new List<string>
                {
                    "STT",
                    "ThietBi",
                    "SoLuong",
                    "DonGia",
                    "ThanhTien",
                    "ThoiGianGiaoHang",
                    "lstFileNameTemp",

                }
            };


            DataRow nr = dt.NewRow();
            nr["CongTy"] = request.CustomerName ?? " ";
            nr["DiaChi"] = request.CustomerAddress ?? " ";
            nr["DienThoai"] = request.CustomerPhoneNo ?? " ";
            nr["Email"] = request.CustomerEmail ?? " ";
            nr["NguoiGui"] = request.EmployeeName ?? " ";
            //nr["NguoiGui_ChucVu"] = request.Position ?? " ";
            nr["NguoiGui_ChucVu"] = request.LocationName ?? " ";
            nr["NguoiGui_DienThoai"] = request.EmployeePhoneNo ?? " ";
            nr["NguoiGui_Email"] = request.EmployeeEmail ?? " ";
            nr["TienTruocThue"] = request.TongTien.ToString("#,##0") ?? " ";
            nr["VAT"] = request.PhanTramVAT.ToString("#,##0") ?? " ";
            nr["TienThue"] = request.TienVAT.ToString("#,##0") ?? " ";
            nr["TongTien"] = request.ConLai.ToString("#,##0") ?? " ";
            nr["GhiChu"] = request.GhiChu ?? " ";
            nr["PTThanhToan"] = request.ThanhToan ?? " ";
            nr["PTGiaoNhan"] = request.GiaoNhan ?? " ";
            nr["PTBaoHanh"] = request.BaoHanh ?? " ";
            nr["Ngay"] = DateTime.Now.Day.ToString();
            nr["Thang"] = DateTime.Now.Month.ToString();
            nr["Nam"] = DateTime.Now.Year.ToString();
            tableData.Headers.ForEach(cols => tableData.DataRows.Columns.Add(cols));
            var quotation_line = request.Quotation_Line.OrderBy(x => x.OrderBy).ToList();
            foreach (var line in quotation_line)
            {
                if (line.price == null)
                {
                    line.price = 0;
                }
                if (line.amt == null)
                {
                    line.amt = 0;
                }
                var item = _datacontext.WMS_Items.SingleOrDefault(x => x.Code == line.Code);
                #region Lấy danh sách hình của báo giá
                var image = _datacontext.WMS_Item_Images.Where(x => x.ItemCode == item.Code && x.IsHiden != true).ToList();
                var lstHinhAnh = new List<byte[]>();
                if (image.Count > 0)
                {
                    lstHinhAnh = image.Select(aa => aa.Image).ToList();
                }
                #endregion
                #region Tạo file ảnh tạm lưu trên đĩa
                List<string> fileNames = new List<string>();
                if (lstHinhAnh != null && lstHinhAnh.Count > 0)
                {
                    foreach (var hinhanh in lstHinhAnh)
                    {
                        var fileNameTemp = Path.ChangeExtension(Path.GetTempFileName(), ".jpg");
                        var ms = new MemoryStream(hinhanh);
                        var img = System.Drawing.Image.FromStream(ms);
                        img.Save(fileNameTemp);
                        fileNames.Add(fileNameTemp);
                    }
                }
                #endregion
                tableData.DataRows.Rows.Add(
                    quotation_line.IndexOf(line) + 1,    // STT
                    item.Name
                    + "\r " + Model + ": " + item.Model
                    + " \r" + GioiThieu + ": " + item.Description
                    + " \r" + Link + ": "  + item.LinkVideo
                    + " \r" + Tskt + ": " + "\n" + item.Specifications
                    + " \r" + Xuatxu + ": " + item.CountryOfOriginCode + "\n", //ThietBi
                    line.qty.Value.ToString("#,##0") ?? " ", //SoLuong
                    line.price.Value.ToString("#,##0") ?? " ",//DonGia
                    line.amt.Value.ToString("#,##0") ?? " ", //ThanhTien
                    line.Description ?? " " ,//TG giao hàng
                    string.Join(";", fileNames) //DS hình ảnh
                );
            }
            string nameFileExport = $"BaoGia_{request.CustomerCode}_{DateTime.Now.Ticks}.docx"; 
            //string nameFileExport = $"{request.DocumentNo}.docx";
            string downloadFolder = "/download/" + request.CustomerCode;
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }
            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            Mailmerge(templateName, exportFilePath, nr, dt.Columns, tableData);
            return downloadFolder + "/" + nameFileExport;
        }
        public string ExportHopDong(string templateName, WordStreamHopDongResponse request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            DataTable dt = new DataTable();
            var dataCols = new List<string>
            {
                "SoHopDong",
                "NgayThangHD",
                "Thang",
                "Nam",
                "BenA",
                "DiaChi_BenA",
                "DienThoai_BenA",
                "Fax_BenA",
                "MST_BenA",
                "DaiDien_BenA",
                "ChucVu_BenA",
                "STK_BenA",

                "BenB",
                "DiaChi_BenB",
                "DienThoai_BenB",
                "Fax_BenB",
                "MST_BenB",
                "DaiDien_BenB",
                "ChucVu_BenB",
                "STK_BenB",


                "TienTruocThue",
                "VAT",
                "TienThue",
                "TongTien",

                "Dieu2",
                "Dieu3",
                "Dieu4",
                "Dieu5",
                "Dieu6",

                "SoTien_BangChu",
            };
            dataCols.ForEach(cols => dt.Columns.Add(cols));
            TableData tableData = new TableData
            {
                Headers = new List<string>
                {
                    "STT",
                    "ThietBi",
                    "ChatLuong",
                    "SoLuong",
                    "DonGia",
                    "ThanhTien",
                    "",
                }
            };


            DataRow nr = dt.NewRow();
            nr["SoHopDong"] = request.SoHopDong ?? " ";
            nr["NgayThangHD"] = DateTime.Now.Day.ToString();
            nr["Thang"] = DateTime.Now.Month.ToString();
            nr["Nam"] = DateTime.Now.Year.ToString();
            nr["BenA"] = request.BenA_TenCongTy ?? " ";
            nr["DiaChi_BenA"] = request.BenA_DiaChi ?? " ";
            nr["DienThoai_BenA"] = request.BenA_SoDienThoai ?? " ";
            nr["Fax_BenA"] = request.BenA_Fax ?? " ";
            nr["MST_BenA"] = request.BenA_MST ?? " ";
            nr["DaiDien_BenA"] = request.BenA_NguoiDaiDien ?? " ";
            nr["ChucVu_BenA"] = request.BenA_ChucVu ?? " ";
            nr["STK_BenA"] = request.BenA_SoTaiKhoanNganHang ?? " ";

            nr["BenB"] = request.BenB_TenCongTy ?? " ";
            nr["DiaChi_BenB"] = request.BenB_DiaChi ?? " ";
            nr["DienThoai_BenB"] = request.BenB_SoDienThoai ?? " ";
            nr["Fax_BenB"] = request.BenB_Fax ?? " ";
            nr["MST_BenB"] = request.BenB_MST ?? "";
            nr["DaiDien_BenB"] = request.BenB_NguoiDaiDien ?? "";
            nr["ChucVu_BenB"] = request.BenB_ChucVu ?? "";
            nr["STK_BenB"] = request.BenB_SoTaiKhoanNganHang ?? "";


            nr["TienTruocThue"] = request.TongTien_TruocThue.ToString("#,##0") ?? " ";
            nr["VAT"] = request.PhanTramThue.ToString("#,##0") ?? "";
            nr["TienThue"] = request.TienThue.ToString("#,##0") ?? "";
            nr["TongTien"] = request.TongTien.ToString("#,##0") ?? "";

            nr["Dieu2"] = request.Dieu2 ?? "";
            nr["Dieu3"] = request.Dieu3 ?? "";
            nr["Dieu4"] = request.Dieu4 ?? "";
            nr["Dieu5"] = request.Dieu5 ?? "";
            nr["Dieu6"] = request.Dieu6 ?? "";

            nr["SoTien_BangChu"] = NumberToText(request.TongTien);

            tableData.Headers.ForEach(cols => tableData.DataRows.Columns.Add(cols));
            var hodong_chitiet = request.HopDong_ChiTiet.OrderBy(x => x.OrderBy).ToList();
            foreach (var line in hodong_chitiet)
            {
                var item = _datacontext.WMS_Items.SingleOrDefault(x => x.Code == line.Code);
                tableData.DataRows.Rows.Add(
                    hodong_chitiet.IndexOf(line) + 1,    // STT
                    item.Name
                    + "\r " + Model + ": " + item.Model
                    + " \r" + Tskt + ": " + "\n" + item.Specifications
                    + " \r" + Xuatxu + ": " + item.CountryOfOriginCode + "\n", //ThietBi
                    line.ChatLuongName ?? " ", //ChatLuong
                    line.qty.Value.ToString("#,##0") ?? " ", //SoLuong
                    line.price.Value.ToString("#,##0") ?? " ", //DonGia
                    line.amt.Value.ToString("#,##0") ?? " ",//ThanhTien
                    ""
                );
            }
            string nameFileExport = $"HopDongKinhTe_{request.KhachHang_Code}_{DateTime.Now.Ticks}.docx";
            string downloadFolder = "/download/" + request.KhachHang_Code;
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }

            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            Mailmerge(templateName, exportFilePath, nr, dt.Columns, tableData);
            return downloadFolder + "/" + nameFileExport;
        }
        public string ExportHopDongMuaBan(string templateName, WordStreamHopDongResponse request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            DataTable dt = new DataTable();
            var dataCols = new List<string>
            {
                "SoHopDong",
                "NgayThangHD",
                "Thang",
                "Nam",
                "BenA",
                "DiaChi_BenA",
                "DienThoai_BenA",
                "Fax_BenA",
                "MST_BenA",
                "DaiDien_BenA",
                "ChucVu_BenA",
                "STK_BenA",

                "TienTruocThue",
                "VAT",
                "TienThue",
                "TongTien",


                "SoTien_BangChu",
            };
            dataCols.ForEach(cols => dt.Columns.Add(cols));
            TableData tableData = new TableData
            {
                Headers = new List<string>
                {
                    "STT",
                    "ThietBi",
                    "ChatLuong",
                    "SoLuong",
                    "DonGia",
                    "ThanhTien",
                    "",
                }
            };


            DataRow nr = dt.NewRow();
            nr["SoHopDong"] = request.SoHopDong ?? " ";
            nr["NgayThangHD"] = DateTime.Now.Day.ToString();
            nr["Thang"] = DateTime.Now.Month.ToString();
            nr["Nam"] = DateTime.Now.Year.ToString();
            nr["BenA"] = request.BenA_TenCongTy ?? " ";
            nr["DiaChi_BenA"] = request.BenA_DiaChi ?? " ";
            nr["DienThoai_BenA"] = request.BenA_SoDienThoai ?? " ";
            nr["Fax_BenA"] = request.BenA_Fax ?? " ";
            nr["MST_BenA"] = request.BenA_MST ?? " ";
            nr["DaiDien_BenA"] = request.BenA_NguoiDaiDien ?? " ";
            nr["ChucVu_BenA"] = request.BenA_ChucVu ?? " ";
            nr["STK_BenA"] = request.BenA_SoTaiKhoanNganHang ?? " ";


            nr["TienTruocThue"] = request.TongTien_TruocThue.ToString("#,##0") ?? " ";
            nr["VAT"] = request.PhanTramThue.ToString("#,##0") ?? "";
            nr["TienThue"] = request.TienThue.ToString("#,##0") ?? "";
            nr["TongTien"] = request.TongTien.ToString("#,##0") ?? "";

            nr["SoTien_BangChu"] = NumberToText(request.TongTien);


            tableData.Headers.ForEach(cols => tableData.DataRows.Columns.Add(cols));
            var hodong_chitiet = request.HopDong_ChiTiet.OrderBy(x => x.OrderBy).ToList();
            foreach (var line in hodong_chitiet)
            {
                var item = _datacontext.WMS_Items.SingleOrDefault(x => x.Code == line.Code);
                tableData.DataRows.Rows.Add(
                    hodong_chitiet.IndexOf(line) + 1,    // STT
                    item.Name
                    + "\r " + Model + ": " + item.Model
                    + " \r" + Tskt + ": " + "\n" + item.Specifications
                    + " \r" + Xuatxu + ": " + item.CountryOfOriginCode + "\n", //ThietBi
                    line.ChatLuongName ?? " ", //ChatLuong
                    line.qty.Value.ToString("#,##0") ?? " ", //SoLuong
                    line.price.Value.ToString("#,##0") ?? " ", //DonGia
                    line.amt.Value.ToString("#,##0") ?? " ",//ThanhTien
                    ""
                );
            }
            string nameFileExport = $"HopDongMuaBan_{request.KhachHang_Code}_{DateTime.Now.Ticks}.docx";
            string downloadFolder = "/download/" + request.KhachHang_Code;
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }

            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            Mailmerge(templateName, exportFilePath, nr, dt.Columns, tableData);
            return downloadFolder + "/" + nameFileExport;
        }

        #region Word core
        public static void dotx2docx(string sourceFile, string targetFile)
        {
            MemoryStream documentStream;
            using (Stream tplStream = File.OpenRead(sourceFile))
            {
                documentStream = new MemoryStream((int)tplStream.Length);
                CopyStream(tplStream, documentStream);
                documentStream.Position = 0L;
            }

            using (WordprocessingDocument template = WordprocessingDocument.Open(documentStream, true))
            {
                template.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = template.MainDocumentPart;
                mainPart.DocumentSettingsPart.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate",
                   new Uri(targetFile, UriKind.Absolute));

                mainPart.Document.Save();
            }
            File.WriteAllBytes(targetFile, documentStream.ToArray());
        }
        public static void CopyStream(Stream source, Stream target)
        {
            if (source != null)
            {
                MemoryStream mstream = source as MemoryStream;
                if (mstream != null) mstream.WriteTo(target);
                else
                {
                    byte[] buffer = new byte[2048];
                    int length = buffer.Length, size;
                    while ((size = source.Read(buffer, 0, length)) != 0)
                        target.Write(buffer, 0, size);
                }
            }
        }
        public static void Mailmerge(string templatePath, string DestinatePath, DataRow dr, DataColumnCollection columns, TableData tableData = null)
        {
            try
            {
                dotx2docx(templatePath, DestinatePath);
            }
            catch (Exception ec) //incase the server does not support MS Office Word 2003 / 2007 / 2010
            {
                File.Copy(templatePath, DestinatePath, true);
            }
            using (WordprocessingDocument doc = WordprocessingDocument.Open(DestinatePath, true))
            {
                var allParas = doc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>();
                Text PreItem = null;
                string PreItemConstant = null;
                bool FindSingleAnglebrackets = false;
                bool breakFlag = false;
                List<Text> breakedFiled = new List<Text>();
                foreach (Text item in allParas)
                {
                    foreach (DataColumn cl in columns)
                    {
                        //<Today>
                        if (item.Text.Contains("«" + cl.ColumnName + "»") || item.Text.Contains("<" + cl.ColumnName + ">"))
                        {
                            item.Text = item.Text.Replace("<" + cl.ColumnName + ">", dr[cl.ColumnName].ToString())
                                                 .Replace("«" + cl.ColumnName + "»", dr[cl.ColumnName].ToString());
                            FindSingleAnglebrackets = false;
                            breakFlag = false;
                            breakedFiled.Clear();
                        }
                        else if //<Today
                        (item.Text != null
                            && (
                                    (item.Text.Contains("<") && !item.Text.Contains(">"))
                                    || (item.Text.Contains("«") && !item.Text.Contains("»"))
                                )
                            && (item.Text.Contains(cl.ColumnName))
                        )
                        {
                            FindSingleAnglebrackets = true;
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"\<" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"\«" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                        }
                        else if //Today> or Today
                        (
                            PreItemConstant != null
                            && (
                                    (PreItemConstant.Contains("<") && !PreItemConstant.Contains(">"))
                                    || (PreItemConstant.Contains("«") && !PreItemConstant.Contains("»"))
                                )
                            && (item.Text.Contains(cl.ColumnName))
                        )
                        {
                            if (item.Text.Contains(">") || item.Text.Contains("»"))
                            {
                                FindSingleAnglebrackets = false;
                                breakFlag = false;
                                breakedFiled.Clear();
                            }
                            else
                            {
                                FindSingleAnglebrackets = true;
                            }
                            if (PreItemConstant == "<" || PreItemConstant == "«")
                            {
                                PreItem.Text = "";
                            }
                            else
                            {
                                PreItem.Text = global::System.Text.RegularExpressions.Regex.Replace(PreItemConstant, @"\<" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                                PreItem.Text = global::System.Text.RegularExpressions.Regex.Replace(PreItemConstant, @"\«" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                            }
                            if (PreItemConstant.Contains("<") || PreItemConstant.Contains("«")) // pre item is like '[blank]«'
                            {
                                PreItem.Text = PreItem.Text.Replace("<", "");
                                PreItem.Text = PreItem.Text.Replace("«", "");
                            }
                            if (item.Text.Contains(cl.ColumnName + ">") || item.Text.Contains(cl.ColumnName + "»"))
                            {
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"\>", dr[cl.ColumnName].ToString());
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"\»", dr[cl.ColumnName].ToString());

                            }
                            else
                            {
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                            }
                        }
                        else if (FindSingleAnglebrackets && (item.Text.Contains("»") || item.Text.Contains(">")))
                        {
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"\>", dr[cl.ColumnName].ToString());
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"\»", dr[cl.ColumnName].ToString());
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"^\s*\>", "");
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"^\s*\»", "");
                            FindSingleAnglebrackets = false;
                            breakFlag = false;
                            breakedFiled.Clear();
                        }
                        else if (item.Text.Contains("<") || item.Text.Contains("«")) // no ColumnName
                        {

                        }
                    } //end of each columns
                    PreItem = item;
                    PreItemConstant = item.Text;
                    if (breakFlag
                        || (item.Text.Contains("<") && !item.Text.Contains(">"))
                        || (item.Text.Contains("«") && !item.Text.Contains("»"))
                       )
                    {
                        breakFlag = true;
                        breakedFiled.Add(item);
                        string combinedfiled = "";
                        foreach (Text t in breakedFiled)
                        {
                            combinedfiled += t.Text;
                        }
                        foreach (DataColumn cl in columns)
                        {
                            //<Today>
                            if (combinedfiled.Contains("«" + cl.ColumnName + "»") || combinedfiled.Contains("<" + cl.ColumnName + ">"))
                            {
                                //for the first part, remove the last '<' and tailing content
                                breakedFiled[0].Text = global::System.Text.RegularExpressions.Regex.Replace(breakedFiled[0].Text, @"<\w*$", "");
                                breakedFiled[0].Text = global::System.Text.RegularExpressions.Regex.Replace(breakedFiled[0].Text, @"<\w*$", "");

                                //remove middle parts
                                foreach (Text t in breakedFiled)
                                {
                                    if (!t.Text.Contains("<") && !t.Text.Contains("«") && !t.Text.Contains(">") && !t.Text.Contains("»"))
                                    {
                                        t.Text = "";
                                    }
                                }

                                //for the last part(as current item), remove leading content till the first '>' 
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"^\s*\>", dr[cl.ColumnName].ToString());
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"^\s*\»", dr[cl.ColumnName].ToString());

                                FindSingleAnglebrackets = false;
                                breakFlag = false;
                                breakedFiled.Clear();
                                break;
                            }
                        }
                    }
                }//end of each item
                #region go through footer
                MainDocumentPart mainPart = doc.MainDocumentPart;
                foreach (FooterPart footerPart in mainPart.FooterParts)
                {
                    Footer footer = footerPart.Footer;
                    var allFooterParas = footer.Descendants<Text>();
                    foreach (Text item in allFooterParas)
                    {
                        foreach (DataColumn cl in columns)
                        {
                            if (item.Text.Contains("«" + cl.ColumnName + "»") || item.Text.Contains("<" + cl.ColumnName + ">"))
                            {
                                item.Text = (string.IsNullOrEmpty(dr[cl.ColumnName].ToString()) ? " " : dr[cl.ColumnName].ToString());
                                FindSingleAnglebrackets = false;
                            }
                            else if (PreItem != null && (PreItem.Text == "<" || PreItem.Text == "«") && (item.Text.Trim() == cl.ColumnName))
                            {
                                FindSingleAnglebrackets = true;
                                PreItem.Text = "";
                                item.Text = (string.IsNullOrEmpty(dr[cl.ColumnName].ToString()) ? " " : dr[cl.ColumnName].ToString());
                            }
                            else if (FindSingleAnglebrackets && (item.Text == "»" || item.Text == ">"))
                            {
                                item.Text = "";
                                FindSingleAnglebrackets = false;
                            }
                        }
                        PreItem = item;
                    }
                }
                #endregion

                #region replace \v to new Break()
                var body = doc.MainDocumentPart.Document.Body;

                var paras = body.Elements<Paragraph>();
                foreach (var para in paras)
                {
                    foreach (var run in para.Elements<Run>())
                    {
                        foreach (var text in run.Elements<Text>())
                        {
                            if (text.Text.Contains("MS_Doc_New_Line") || text.Text.Contains("\n") || text.Text.Contains("https") || text.Text.Contains("http"))
                            {
                                string[] ss = text.Text.Split(new string[] { "MS_Doc_New_Line", "\n" }, StringSplitOptions.None);
                                text.Text = text.Text = "";
                                int n = 0;
                                foreach (string s in ss)
                                {
                                    var checkHttps = s.Contains("https");
                                    var checkHttp = s.Contains("http");
                                    //check return true if text is link, another return false
                                    if (checkHttps || checkHttp)
                                    {
                                        n++;
                                        string[] separators = new string[] { ": " };
                                        string[] tokens = s.Split(separators, StringSplitOptions.None);
                                        string character = ":  ";
                                        string links = tokens[1];
                                        System.Uri uri = new Uri(links);
                                        HyperlinkRelationship rel = mainPart.AddHyperlinkRelationship(uri, true);
                                        string relationshipId = rel.Id;
                                        Paragraph newParagraph = new Paragraph(
                                            new Hyperlink(
                                                new ProofError() { Type = ProofingErrorValues.GrammarStart },
                                                new Run(
                                                    new Text(tokens[0] + character),
                                                    new RunProperties(
                                                        new RunStyle() { Val = "Hyperlink" },
                                                        new Italic()),
                                                    new Text(links)
                                                    )
                                                )
                                            { History = OnOffValue.FromBoolean(true), Id = relationshipId });
                                        para.AppendChild<Paragraph>(newParagraph);
                                    }
                                    else
                                    {
                                        n++;
                                        run.AppendChild(new Text(s));
                                        if (n != ss.Length)
                                        {
                                            run.AppendChild(new Break());
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                if (tableData != null && tableData.Headers.Any())
                {
                    var table = body.Elements<Table>().FirstOrDefault(d => d.InnerText.Contains(tableData.Headers.FirstOrDefault()));
                    var rows = table.Descendants<TableRow>().ToList();
                    var templateRow = (TableRow)rows[1].Clone();
                    rows[1].Remove();
                    int maxRows = tableData.DataRows.Rows.Count;
                    for (int i = 0; i < maxRows; i++)
                    {
                        var tr = new TableRow();
                        for (int j = 0; j < tableData.DataRows.Rows[i].ItemArray.Length; j++)
                        {
                            var cellValue = tableData.DataRows.Rows[i].ItemArray[j].ToString();
                            var cell = new TableCell();
                            cell.Append(new TableCellProperties(
                                new TableBorders(
                                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                                )
                            ));
                            //format right for columns number
                            if (new List<string>() { "SoLuong", "DonGia", "ThanhTien" }.Contains(tableData.Headers[j].ToString()))
                            {
                                cell.Append(new Paragraph(new Run(new Break(),new Text(cellValue)))
                                {
                                    ParagraphProperties = new ParagraphProperties()
                                    {
                                        Justification = new Justification()
                                        {
                                            Val = new EnumValue<JustificationValues>(JustificationValues.Right)
                                        }
                                    }
                                });
                            }
                            else if (new List<string>() { "ThietBi" }.Contains(tableData.Headers[j].ToString()))
                            {
                                var chars = cellValue.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
                                foreach (var str in chars)
                                {
                                    if (chars.IndexOf(str) == 0 )
                                    {
                                        cell.Append(new Paragraph(new Run(new Break(),
                                            new Text(str))
                                        {
                                            RunProperties = new RunProperties()
                                            {
                                                Bold = new Bold()
                                            }
                                        }));
                                    }
                                    else if (str.Contains("Model:"))
                                    {
                                        cell.Append(new Paragraph(new Run( new Text(str))
                                        {
                                            RunProperties = new RunProperties()
                                            {
                                                Bold = new Bold()
                                            }
                                        }));
                                    }
                                    else if (str.Contains("Giới thiệu:"))
                                    {
                                        cell.Append(new Paragraph(new Run(new Text(str))
                                        {
                                            RunProperties = new RunProperties()
                                            {
                                                Italic = new Italic(),
                                                Color = new Color() { Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF" }
                                            }
                                        }));
                                    }
                                    else if (str.Contains("Link Video:"))
                                    {
                                        cell.Append(new Paragraph(new Run(new Text(str))
                                        {
                                            RunProperties = new RunProperties()
                                            {
                                                Italic = new Italic(),
                                                Underline = new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single },
                                                Color = new Color() { Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF" }
                                            }
                                        }));
                                    }
                                    else if (str.Contains("Giới thiệu:"))
                                    {
                                        cell.Append(new Paragraph(new Run(new Text(str))
                                        {
                                            RunProperties = new RunProperties()
                                            {
                                                Italic = new Italic(),
                                                Color = new Color() { Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF" }
                                            }
                                        }));
                                    }
                                    else if (str.Contains("Thông số kỹ thuật:") || str.Contains("Xuất xứ:"))
                                    {
                                        cell.Append(new Paragraph(new Run(new Text(str))
                                        {
                                            RunProperties = new RunProperties()
                                            {
                                                Bold = new Bold(),
                                                Italic = new Italic()
                                            }
                                        }));
                                    }
                                    else
                                    {
                                        cell.Append(new Paragraph(new Run( new Text(str))));
                                    }
                                }
                            }
                            else if (new List<string>() { "lstFileNameTemp" }.Contains(tableData.Headers[j].ToString()))
                            {
                                var listImages = tableData.DataRows.Rows[i].ItemArray[j].ToString().Split(';').ToList();
                                foreach (var imageFile in listImages)
                                {
                                    if (imageFile != "")
                                    {
                                        ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                                        using (FileStream stream = new FileStream(imageFile, FileMode.Open))
                                        {
                                            imagePart.FeedData(stream);
                                        }
                                        AddImageToCell_Anchor(tr.Elements<TableCell>().ElementAt(j - 1), mainPart.GetIdOfPart(imagePart), imageFile);
                                        //if (File.Exists(imageFile))
                                        //{
                                        //    File.Delete(imageFile);
                                        //}
                                    }
                                }

                            }
                            else
                            {
                                cell.Append(new Paragraph(new Run(new Break(), new Text(cellValue))));
                            }
                            if (j != tableData.DataRows.Rows[i].ItemArray.Length - 1)
                                tr.Append(cell);
                        }
                        //table.Append(tr);
                        var firstSummaryRow = table.Descendants<TableRow>().ToList()[i+1];
                        table.InsertBefore(tr, firstSummaryRow);
                    }
                }
                #endregion
                doc.MainDocumentPart.Document.Save();
            }
        }
        public static void MergeDocuments(params string[] filepaths)
        {
            //filepaths = new[] { "D:\\one.docx", "D:\\two.docx", "D:\\three.docx", "D:\\four.docx", "D:\\five.docx" };
            if (filepaths != null && filepaths.Length > 1)

                using (WordprocessingDocument myDoc = WordprocessingDocument.Open(@filepaths[0], true))
                {
                    MainDocumentPart mainPart = myDoc.MainDocumentPart;

                    for (int i = 1; i < filepaths.Length; i++)
                    {
                        string altChunkId = "AltChunkId" + i;
                        AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                            AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                        using (FileStream fileStream = File.Open(@filepaths[i], FileMode.Open))
                        {
                            chunk.FeedData(fileStream);
                        }
                        DocumentFormat.OpenXml.Wordprocessing.AltChunk altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                        altChunk.Id = altChunkId;
                        //new page, if you like it...
                        mainPart.Document.Body.AppendChild(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        //next document
                        mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
                    }
                    mainPart.Document.Save();
                    myDoc.Close();
                }
        }
        private static void AddImageToCell_Anchor(TableCell cell, string relationshipId, string imageFile)
        {
            System.Drawing.Image dimg = System.Drawing.Image.FromFile(imageFile);
            Wp.Drawing _drawing = new Wp.Drawing();
            DW.Anchor _anchor = new DW.Anchor()
            {
                DistanceFromTop = (OXML.UInt32Value)0U,
                DistanceFromBottom = (OXML.UInt32Value)0U,
                DistanceFromLeft = (OXML.UInt32Value)0U,
                DistanceFromRight = (OXML.UInt32Value)0U,
                SimplePos = false,
                RelativeHeight = (OXML.UInt32Value)0U,
                BehindDoc = false,
                Locked = false,
                LayoutInCell = true,
                AllowOverlap = true,
                EditId = "44CEF5E4",
                AnchorId = "44803ED1"
            };
            DW.SimplePosition _spos = new DW.SimplePosition()
            {
                X = 0L,
                Y = 0L,
            };
            DW.HorizontalPosition _hp = new DW.HorizontalPosition()
            {
                RelativeFrom = DW.HorizontalRelativePositionValues.Column
            };
            DW.PositionOffset _hPO = new DW.PositionOffset();
            _hPO.Text = "-1620000";
            _hp.Append(_hPO);
            DW.VerticalPosition _vp = new DW.VerticalPosition()
            {
                RelativeFrom = DW.VerticalRelativePositionValues.Paragraph
            };
            DW.PositionOffset _vPO = new DW.PositionOffset();
            _vPO.Text = "83820";
            _vp.Append(_vPO);
            DW.Extent _e = new DW.Extent();
            if (dimg.Width > 300 || dimg.Height > 300)
            {
                _e.Cx = (long)((dimg.Width / dimg.HorizontalResolution) * 190000L);
                _e.Cy = (long)((dimg.Height / dimg.VerticalResolution) * 190000L);
            }
            else
            {
                _e.Cx = (long)((dimg.Width / dimg.HorizontalResolution) * 914400L);
                _e.Cy = (long)((dimg.Height / dimg.VerticalResolution) * 914400L);
            }
            
            DW.EffectExtent _ee = new DW.EffectExtent()
            {
                LeftEdge = 0L,
                TopEdge = 0L,
                RightEdge = 0L,
                BottomEdge = 0L
            };
            DW.WrapNone _wp = new DW.WrapNone();
            DW.DocProperties _dp = new DW.DocProperties()
            {
                Id = 1U,
                Name = "Test Picture"
            };
            DW.NonVisualGraphicFrameDrawingProperties nonVis = new DW.NonVisualGraphicFrameDrawingProperties();
            A.GraphicFrameLocks _Gf = new A.GraphicFrameLocks()
            {
                NoChangeAspect = true
            };
            nonVis.Append(_Gf);
            OXML.Drawing.Graphic _g = new OXML.Drawing.Graphic();
            OXML.Drawing.GraphicData _gd = new OXML.Drawing.GraphicData() { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };


            OXML.Drawing.Pictures.Picture _pic = new OXML.Drawing.Pictures.Picture();
            OXML.Drawing.Pictures.NonVisualPictureProperties _nvpp = new OXML.Drawing.Pictures.NonVisualPictureProperties();
            OXML.Drawing.Pictures.NonVisualDrawingProperties _nvdp = new OXML.Drawing.Pictures.NonVisualDrawingProperties()
            {
                Id = 0,
                Name = "Picture"
            };
            OXML.Drawing.Pictures.NonVisualPictureDrawingProperties _nvpdp = new OXML.Drawing.Pictures.NonVisualPictureDrawingProperties();
            _nvpp.Append(_nvdp);
            _nvpp.Append(_nvpdp);


            OXML.Drawing.Pictures.BlipFill _bf = new OXML.Drawing.Pictures.BlipFill();
            OXML.Drawing.Blip _b = new OXML.Drawing.Blip()
            {
                Embed = relationshipId,
                CompressionState = OXML.Drawing.BlipCompressionValues.Print
            };
            _bf.Append(_b);
            OXML.Drawing.Stretch _str = new OXML.Drawing.Stretch();
            OXML.Drawing.FillRectangle _fr = new OXML.Drawing.FillRectangle();
            _str.Append(_fr);
            _bf.Append(_str);

            OXML.Drawing.Pictures.ShapeProperties _shp = new OXML.Drawing.Pictures.ShapeProperties();
            OXML.Drawing.Transform2D _t2d = new OXML.Drawing.Transform2D();
            OXML.Drawing.Offset _os = new OXML.Drawing.Offset()
            {
                X = 0L,
                Y = 0L
            };
            OXML.Drawing.Extents _ex = new OXML.Drawing.Extents();
            if (dimg.Width > 300 || dimg.Height > 300)
            {
                _ex.Cx = (long)((dimg.Width / dimg.HorizontalResolution) * 190000L);
                _ex.Cy = (long)((dimg.Height / dimg.VerticalResolution) * 190000L);
            }
            else
            {
                _ex.Cx = (long)((dimg.Width / dimg.HorizontalResolution) * 914400L);
                _ex.Cy = (long)((dimg.Height / dimg.VerticalResolution) * 914400L);
            }

            _t2d.Append(_os);
            _t2d.Append(_ex);

            OXML.Drawing.PresetGeometry _preGeo = new OXML.Drawing.PresetGeometry()
            {
                Preset = OXML.Drawing.ShapeTypeValues.Rectangle
            };
            OXML.Drawing.AdjustValueList _adl = new OXML.Drawing.AdjustValueList();

            _preGeo.Append(_adl);

            _shp.Append(_t2d);
            _shp.Append(_preGeo);

            _pic.Append(_nvpp);
            _pic.Append(_bf);
            _pic.Append(_shp);

            _gd.Append(_pic);
            _g.Append(_gd);

            _anchor.Append(_spos);
            _anchor.Append(_hp);
            _anchor.Append(_vp);
            _anchor.Append(_e);
            _anchor.Append(_ee);
            _anchor.Append(_wp);
            _anchor.Append(_dp);
            _anchor.Append(nonVis);
            _anchor.Append(_g);


            _drawing.Append(_anchor);

            cell.Append(new Paragraph(new Run(_drawing)));
        }
        private static void AddImageToCell(TableCell cell, string relationshipId)
        {
            var element =
              new Drawing(
                new DW.Inline(
                  new DW.Extent() { Cx = 990000L, Cy = 792000L },
                  new DW.EffectExtent()
                  {
                      LeftEdge = 0L,
                      TopEdge = 0L,
                      RightEdge = 0L,
                      BottomEdge = 0L
                  },
                  new DW.DocProperties()
                  {
                      Id = (UInt32Value)1U,
                      Name = "Picture 1"
                  },
                  new DW.NonVisualGraphicFrameDrawingProperties(
                  new A.GraphicFrameLocks() { NoChangeAspect = true }),
                  new A.Graphic(
                    new A.GraphicData(
                      new PIC.Picture(
                        new PIC.NonVisualPictureProperties(
                          new PIC.NonVisualDrawingProperties()
                          {
                              Id = (UInt32Value)0U,
                              Name = "New Bitmap Image.jpg"
                          },
                          new PIC.NonVisualPictureDrawingProperties()),
                        new PIC.BlipFill(
                          new A.Blip(
                            new A.BlipExtensionList(
                              new A.BlipExtension()
                              {
                                  Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
                              })
                           )
                          {
                              Embed = relationshipId,
                              CompressionState =
                              A.BlipCompressionValues.Print
                          },
                          new A.Stretch(
                            new A.FillRectangle())),
                          new PIC.ShapeProperties(
                            new A.Transform2D(
                              new A.Offset() { X = 0L, Y = 0L },
                              new A.Extents() { Cx = 990000L, Cy = 792000L }),
                            new A.PresetGeometry(
                              new A.AdjustValueList()
                            )
                            { Preset = A.ShapeTypeValues.Rectangle }))
                    )
                    { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                )
                {
                    DistanceFromTop = (OXML.UInt32Value)0U,
                    DistanceFromBottom = (OXML.UInt32Value)0U,
                    DistanceFromLeft = (OXML.UInt32Value)0U,
                    DistanceFromRight = (OXML.UInt32Value)0U,
                    EditId = "44CEF5E4",
                    AnchorId = "44803ED1"
                });
            cell.Append(new Paragraph(new Run(element)));
        }
        #endregion

    }
}
