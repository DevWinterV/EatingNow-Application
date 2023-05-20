using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Quotation;
using Falcon.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Word
{
    public interface IMailMergeService
    {
        string ExportQuotation(string templateName, QuotationRequest request);
        string ExportHopDong(string templateName, HopDongRequest request);

    }
    public class MailMergeService : IMailMergeService
    {
        private readonly DataContext _datacontext;
        private const string Model = "Model";
        private const string Tskt = "Thông số kỹ thuật";
        private const string Xuatxu = "Xuất xứ";
        public MailMergeService(DataContext datacontext)
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
        public string ExportHopDong(string templateName, HopDongRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            #region
            if (request.BenA_Name == "")
            {
                request.BenA_Name = " ";
            }
            if (request.BenA_Address == "")
            {
                request.BenA_Address = " ";
            }
            if (request.BenA_Address == "")
            {
                request.BenA_Address = " ";
            }
            if (request.BenA_Phone == "")
            {
                request.BenA_Phone = " ";
            }
            if (request.BenA_Fax == "")
            {
                request.BenA_Fax = " ";
            }
            if (request.BenA_MST == "")
            {
                request.BenA_MST = " ";
            }
            if (request.BenA_Banking == "")
            {
                request.BenA_Banking = " ";
            }

            if (request.BenA_ManagerName == "")
            {
                request.BenA_ManagerName = " ";
            }
            if (request.BenA_ManagerRole == "")
            {
                request.BenA_ManagerRole = " ";
            }
            if (request.BenB_Name == "")
            {
                request.BenB_Name = " ";
            }
            if (request.BenB_Address == "")
            {
                request.BenB_Address = " ";
            }
            if (request.BenB_Phone == "")
            {
                request.BenB_Phone = " ";
            }
            if (request.BenB_Fax == "")
            {
                request.BenB_Fax = " ";
            }
            if (request.BenB_MST == "")
            {
                request.BenB_MST = " ";
            }
            if (request.BenB_ManagerName == "")
            {
                request.BenB_ManagerName = " ";
            }
            if (request.BenB_ManagerRole == "")
            {
                request.BenB_ManagerRole = " ";
            }
            if (request.BenB_Banking == "")
            {
                request.BenB_Banking = " ";
            }
            #endregion
            var pDictionaryMerge = new Dictionary<string, string>
                {
                    {"SoHopDong", request.ContractNumber},
                    {"BenA", request.BenA_Name},
                    {"DiaChi_BenA", request.BenA_Address},
                    {"DienThoai_BenA", request.BenA_Phone},
                    {"Fax_BenA", request.BenA_Fax},
                    {"MST_BenA", request.BenA_MST},
                    {"DaiDien_BenA", request.BenA_ManagerName },
                    {"ChucVu_BenA", request.BenA_ManagerRole },
                    {"STK_BenA", request.BenA_Banking},

                    {"BenB", request.BenB_Name},
                    {"DiaChi_BenB", request.BenB_Address},
                    {"DienThoai_BenB", request.BenB_Phone},
                    {"Fax_BenB", request.BenB_Fax},
                    {"MST_BenB", request.BenB_MST},
                    {"DaiDien_BenB", request.BenB_ManagerName },
                    {"ChucVu_BenB", request.BenB_ManagerRole},
                    {"STK_BenB", request.BenB_Banking},

                    {"NgayThangHD", DateTime.Now.Day.ToString()},
                    {"Thang", DateTime.Now.Month.ToString()},
                    {"Nam", DateTime.Now.Year.ToString()},

                    {"SoTien_BangChu", NumberToText(request.TienSauThue)},



                    {"TienTruocThue", (request.TongTien).ToString("#,##0")},
                    {"VAT", (request.PhanTramThue).ToString("")},
                    {"TienThue", (request.Thue).ToString("#,##0")},
                    {"TongTien", (request.TienSauThue).ToString("#,##0")},
                    {"Dieu2", request.Dieu2},

                };
            var lstChiTiet = new List<CR_HopDong>();
            var i = 0;
            foreach (var line in request.dataItemCheck)
            {
                var item = _datacontext.WMS_Items.SingleOrDefault(x => x.Code == line.Code);
                var chitiet = new CR_HopDong();
                i++;
                var stt = i.ToString();
                chitiet.stt = "\n" + stt;
                chitiet.name = line.Name ?? " ";
                chitiet.xuatxu = item.CountryOfOriginCode ?? " ";
                chitiet.thongsokythuat = item.Specifications ?? " ";
                chitiet.chatluong = "";
                chitiet.model = line.Model;
                chitiet.specifications = line.Specifications;
                chitiet.qty = line.qty.Value.ToString("#,##0");
                chitiet.price = line.price.Value.ToString("#,##0");
                chitiet.amt = line.amt.Value.ToString("#,##0");
                chitiet.GhiChuDetail = "\n" + chitiet.name
                       + "\r " + Model + ": " + chitiet.model
                       + " \r" + Tskt + ": \n" + chitiet.thongsokythuat
                       + " \r" + Xuatxu + ": " + chitiet.xuatxu + "\n";
                lstChiTiet.Add(chitiet);
            }
            var arrColumn = new[] { "STT", "ThietBi","ChatLuong" , "SoLuong", "DonGia", "ThanhTien" };
            var arrTypes = new[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string)};
            var tblBaoGiaChiTiet = WordUltil.CreateDataTable(arrColumn, arrTypes);
            foreach (var chitiet in lstChiTiet)
            {
                tblBaoGiaChiTiet.Rows.Add(chitiet.stt, chitiet.GhiChuDetail , chitiet.chatluong, chitiet.qty, chitiet.price, chitiet.amt);

            }
            var word = new WordUltil(templateName, false);
            word.WriteFields(pDictionaryMerge);
            word.WriteTable_Contract(tblBaoGiaChiTiet, 2);
            string nameFileExport = $"HopDongChiTiet_{request.CustomerCode}_{DateTime.Now.Ticks}.doc";
            string downloadFolder = "/download/" + request.CustomerCode;
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }
            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            word.SaveAs(exportFilePath);
            return downloadFolder + "/" + nameFileExport;
        }
        public string ExportQuotation(string templateName, QuotationRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));

            #region validet request
            if (request.CustomerName == "")
            {
                request.CustomerName = " ";
            }

            if (request.CustomerAddress == "")
            {
                request.CustomerAddress = " ";
            }

            if (request.CustomerPhoneNo == "")
            {
                request.CustomerPhoneNo = " ";
            }

            if (request.CustomerEmail == "")
            {
                request.CustomerEmail = " ";
            }

            if (request.EmployeeName == "")
            {
                request.EmployeeName = " ";
            }

            if (request.EmployeePhoneNo == "")
            {
                request.EmployeePhoneNo = " ";
            }

            if (request.EmployeeEmail == "")
            {
                request.EmployeeEmail = " ";
            }

            if (request.ThanhToan == "")
            {
                request.ThanhToan = " ";
            }

            if (request.BaoHanh == "")
            {
                request.BaoHanh = " ";
            }

            if (request.GiaoNhan == "")
            {
                request.GiaoNhan = " ";
            }

            if (request.GhiChu == "")
            {
                request.GhiChu = " ";
            }
            #endregion

            var pDictionaryMerge = new Dictionary<string, string>
                {
                    {"CongTy", request.CustomerName},
                    {"DiaChi", request.CustomerAddress},
                    {"DienThoai", request.CustomerPhoneNo},
                    {"Email", request.CustomerEmail},
                    {"NguoiGui", request.EmployeeName},
                    {"NguoiGui_DienThoai", request.EmployeePhoneNo},
                    {"NguoiGui_Email", request.EmployeeEmail},
                    {"TienTruocThue", request.TongTien.ToString("#,##0") ?? " "},
                    {"VAT", request.PhanTramVAT.ToString("#,##0") ?? " "},
                    {"TienThue", request.TienVAT.ToString("#,##0") ?? " "},
                    {"TongTien", request.ConLai.ToString("#,##0") ?? " "},
                    {"GhiChu", request.GhiChu},
                    {"PTThanhToan", request.ThanhToan},
                    {"PTGiaoNhan", request.GiaoNhan},
                    {"PTBaoHanh", request.BaoHanh},
                    {"Ngay", DateTime.Now.Day.ToString()},
                    {"Thang", DateTime.Now.Month.ToString()},
                    {"Nam", DateTime.Now.Year.ToString()},
                };
            var lstChiTiet = new List<CR_BangBaoGia>();
            var i = 0;
            foreach (var line in request.dataItemCheck)
            {
                var item = _datacontext.WMS_Items.SingleOrDefault(x => x.Code == line.Code);
                var chitiet = new CR_BangBaoGia();
                i++;
                var stt = i.ToString();
                chitiet.stt = "\n" + stt ?? " ";
                chitiet.name = item.Name ?? " ";
                chitiet.model = item.Model ?? " ";
                chitiet.xuatxu = item.CountryOfOriginCode ?? " ";
                chitiet.thongsokythuat = item.Specifications ?? " ";
                chitiet.qty = "\n" + line.qty.Value.ToString("#,##0") ?? " ";
                chitiet.price = "\n" + line.price.Value.ToString("#,##0") ?? " ";
                chitiet.amt = "\n" + line.amt.Value.ToString("#,##0") ?? " ";
                chitiet.time = " ";
                chitiet.GhiChuDetail = "\n" + chitiet.name
                        + "\r " + Model + ": " + chitiet.model
                        + " \r" + Tskt + ": \n" + chitiet.thongsokythuat
                        + " \r" + Xuatxu + ": " + chitiet.xuatxu + "\n";
                chitiet.lstFileNameTemp = new List<string>();

                #region Lấy danh sách hình của báo giá
                var image = _datacontext.WMS_Item_Images.Where(x => x.ItemCode == item.Code).ToList();
                var lstIds = new List<int>();
                var lstHinhAnh = new List<byte[]>();

                if (image != null)
                {
                    lstIds = image.Select(aa => aa.Id).ToList();
                    lstHinhAnh = image.Select(aa => aa.Image).ToList();
                }
                #endregion
                #region Tạo file ảnh tạm lưu trên đĩa
                if (lstHinhAnh != null && lstHinhAnh.Count > 0)
                {
                    foreach (var hinhanh in lstHinhAnh)
                    {
                        var fileNameTemp = Path.ChangeExtension(Path.GetTempFileName(), ".jpg");
                        var ms = new MemoryStream(hinhanh);
                        var img = System.Drawing.Image.FromStream(ms);
                        img.Save(fileNameTemp);
                        chitiet.lstFileNameTemp.Add(fileNameTemp);
                    }
                }
                #endregion

                lstChiTiet.Add(chitiet);
            }
            var arrColumn = new[] { "STT", "ThietBi", "SoLuong", "DonGia", "ThanhTien", "ThoiGianGiaoHang", "lstFileNameTemp" };
            var arrTypes = new[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(List<string>) };
            var tblBaoGiaChiTiet = WordUltil.CreateDataTable(arrColumn, arrTypes);
            foreach (var chitiet in lstChiTiet)
            {
                tblBaoGiaChiTiet.Rows.Add(chitiet.stt, chitiet.GhiChuDetail, chitiet.qty, chitiet.price, chitiet.amt, chitiet.time, chitiet.lstFileNameTemp);
            }
            var word = new WordUltil(templateName, true);
            word.WriteFields(pDictionaryMerge);
            word.WriteTable(tblBaoGiaChiTiet, 2);
            string nameFileExport = $"BaoGia_{request.CustomerCode}_{DateTime.Now.Ticks}.doc";
            string downloadFolder = "/download/" + request.CustomerCode;
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }
            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            word.SaveAs(exportFilePath);
            return downloadFolder + "/" + nameFileExport;
        }
    }
   
    public class WordUltil
    {
        private readonly Microsoft.Office.Interop.Word.Application _app;
        readonly Microsoft.Office.Interop.Word.Document _doc;
        private object missing;
        private const string Model = "Model";
        private const string Tskt = "Thông số kỹ thuật";
        private const string Xuatxu = "Xuất xứ";
        public WordUltil(object vPath, bool vCreateApp)
        {
            _app = new Microsoft.Office.Interop.Word.Application();
            _app.Visible = vCreateApp;
            missing = System.Reflection.Missing.Value;
            _doc = _app.Documents.Add(ref vPath, ref missing, ref missing, ref missing);
        }
        public static DataTable CreateDataTable(IList<string> arrColumns, IList<Type> arrTypes)
        {
            var datatable = new DataTable();
            for (var i = 0; i < arrColumns.Count(); i++)
            {
                datatable.Columns.Add(arrColumns[i], arrTypes[i]);
            }
            return datatable;
        }
        public void WriteFields(Dictionary<string, string> vValues)
        {
            foreach (Microsoft.Office.Interop.Word.Field field in _doc.Fields)
            {
                try
                {
                    var fieldName = field.Code.Text.Substring(11, field.Code.Text.IndexOf("\\") - 12).Trim();
                    if (!vValues.ContainsKey(fieldName)) continue;

                    field.Select();
                    _app.Selection.TypeText(vValues[fieldName]);
                }
                catch (Exception)
                {

                }
            }
        }
        public void WriteTable(DataTable vDataTable, int vIndexTable)
        {
            var docTbl = _doc.Tables[vIndexTable];
            var lenrow = vDataTable.Rows.Count;
            var lencol = vDataTable.Columns.Count;

            for (var i = 0; i < lenrow; ++i)
            {
                object obj = System.Reflection.Missing.Value;
                docTbl.Rows.Add(ref obj);
                for (var j = 0; j < lencol; ++j)
                {
                    if (j == lencol - 2)// Cột thành tiền trong DataTable
                    {
                        //Ghi nội dung thời gian giao hàng
                        docTbl.Cell(i + 2, j + 1).Range.Text = vDataTable.Rows[i]["ThoiGianGiaoHang"].ToString();//Ghi cột giao hàng
                        //Chèn ds hình
                        var lstFileNameTemp = new List<string>();
                        if (vDataTable.Rows[i]["lstFileNameTemp"] != null)
                            lstFileNameTemp = (List<string>)vDataTable.Rows[i]["lstFileNameTemp"];

                        if (lstFileNameTemp != null && lstFileNameTemp.Any())
                        {
                            foreach (var filenametemp in lstFileNameTemp)
                            {
                                var shape = docTbl.Cell(i + 2, j + 1).Range.InlineShapes.AddPicture(filenametemp, ref obj, ref obj, ref obj);
                                var ratio = shape.Width / shape.Height;
                                if (shape.Width > 200)
                                    shape.Width = 200;
                                shape.Height = shape.Width / ratio;
                                shape.Select();
                                try
                                {
                                    shape.ConvertToShape(); //(!)
                                }
                                catch (Exception)
                                {
                                    shape.Width = 112 / lstFileNameTemp.Count;
                                    shape.Height = shape.Width / ratio;
                                }
                            }
                        }
                    }
                    else if (j == lencol - 1)// Cột thời gian giao hàng trong DataTable
                    {
                        //Do nothing
                    }
                    else
                    {
                        docTbl.Cell(i + 2, j + 1).Range.Text = vDataTable.Rows[i][j].ToString();
                        if (j == 1)
                        {
                            var sentence = docTbl.Cell(i + 2, j + 1).Range.Sentences;
                            try
                            {
                                sentence[2].Bold = 1;//sentence[2] là dòng tên SP
                            }
                            catch (Exception) { }
                            for (var k = 0; k < sentence.Count; k++)
                            {
                                if (sentence[k + 1].Text.Contains(Model))
                                    sentence[k + 1].Bold = 1;
                                if (sentence[k + 1].Text.Contains(Tskt) || sentence[k + 1].Text.Contains(Xuatxu))
                                {
                                    sentence[k + 1].Bold = 1;
                                    sentence[k + 1].Italic = 1;
                                }
                            }
                        }
                    }
                }
            }

            docTbl.Rows[docTbl.Rows.Count].Delete();//Xóa bỏ dòng trống cuối cùng
        }
        public void WriteTable_Contract(DataTable vDataTable, int vIndexTable)
        {
            var docTable = _doc.Tables[vIndexTable];
            var lenrow = vDataTable.Rows.Count;
            var lencol = vDataTable.Columns.Count;
            for (var i = 0; i < lenrow; ++i)
            {
                // object ob = System.Reflection.Missing.Value;
                object beforeRow = docTable.Rows[i + 2];
                docTable.Rows.Add(ref beforeRow);
                for (var j = 0; j < lencol; ++j)
                {
                    docTable.Cell(i + 2, j + 1).Range.Text = vDataTable.Rows[i][j].ToString();
                    if (j == 1)
                    {
                        var sentence = docTable.Cell(i + 2, j + 1).Range.Sentences;
                        try
                        {
                            sentence[2].Bold = 1;//sentence[2] là dòng tên SP
                        }
                        catch (Exception) { }
                        if (sentence != null && sentence.Count > 0)
                        {
                            for (var k = 0; k < sentence.Count; k++)
                            {
                                if (sentence[k + 1].Text.Contains(Model))
                                    sentence[k + 1].Bold = 1;
                                if (sentence[k + 1].Text.Contains(Tskt) || sentence[k + 1].Text.Contains(Xuatxu))
                                {
                                    sentence[k + 1].Bold = 1;
                                    sentence[k + 1].Italic = 1;
                                }
                            }
                        }
                    }
                }
            }
            docTable.Rows[docTable.Rows.Count - 3].Delete();//Xóa bỏ dòng trống cuối cùng
        }
        public void SaveAs(string filePath)
        {
            object filename = filePath;
            _doc.SaveAs2(ref filename);
            _doc.Close(ref missing, ref missing, ref missing);
            Marshal.ReleaseComObject(_doc);
            _app.Quit(ref missing, ref missing, ref missing);
            Marshal.ReleaseComObject(_app);
        }
        
    }
    
}
