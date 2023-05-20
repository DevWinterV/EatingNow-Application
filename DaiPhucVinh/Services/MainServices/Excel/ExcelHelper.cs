using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using DaiPhucVinh.Shared.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace DaiPhucVinh.Services.MainServices.Excel
{
    public static class ExcelHelper
    {
        #region Contructor
        public enum RowOrCol { Row, Column };
        public static IEnumerable<int> GetColumnOrRowEmpty(Range usedRange, RowOrCol rowOrCol)
        {
            var listIndex = new List<int>();
            var count = rowOrCol == RowOrCol.Column ? usedRange.Columns.Count : usedRange.Rows.Count;
            for (var i = count; i > 0; i--)
            {
                Range curRange;
                if (rowOrCol == RowOrCol.Column)
                    curRange = (Range)usedRange.Columns[i];
                else
                    curRange = (Range)usedRange.Rows[i];

                var isEmpty = curRange.Cells.Cast<Range>().All(cell => cell.Value == null);

                if (!isEmpty)
                    continue;
                listIndex.Add(i);
            }

            return listIndex;
        }
        public static void DeleteCols(int[] colsToDelete, _Worksheet worksheet)
        {
            // the cols are sorted high to low - so index's wont shift
            //Get non Empty Cols
            var nonEmptyCols = Enumerable.Range(1, colsToDelete.Max()).ToList().Except(colsToDelete).ToList();
            if (nonEmptyCols.Max() < colsToDelete.Max())
            {

                // there are empty rows after the last non empty row
                var cell1 = (Range)worksheet.Cells[1, nonEmptyCols.Max() + 1];
                var cell2 = (Range)worksheet.Cells[1, nonEmptyCols.Max()];
                //Delete all empty rows after the last used row
                worksheet.Range[cell1, cell2].EntireColumn.Delete(XlDeleteShiftDirection.xlShiftToLeft);
            }//else last non empty column = worksheet.Columns.Count

            foreach (var colIndex in colsToDelete.Where(x => x < nonEmptyCols.Max()))
            {
                ((Range)worksheet.Columns[colIndex])?.Delete();
            }
        }

        #endregion

        #region khách hàng
        public static List<CustomerModel> ReadDataFromExcel_KhachHang(string pathFile)
        {
            var xlApp = new Application();
            try
            {
                Workbook workBook = xlApp.Workbooks.Open(pathFile,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                var values = new List<CustomerModel>();
                var numSheets = workBook.Sheets.Count + 1;
                List<string> ColumnsDefined = new List<string>
                {
                    "Tên khách hàng",
                    "Mã đối tác của Phoenix",
                    "Thuộc chi nhánh",
                    "Địa chỉ",
                    "Điện thoại",
                    "Mã thẻ thành viên",
                    "Số CMND/CCCD",
                    "Ngày sinh",
                    "Số Fax",
                    "Email",
                    "Mã số thuế",
                    "Nhóm khách hàng",
                    "Loại khách hàng",
                    "Công nợ phải thu",
                    "Điểm hiện tại",
                    "Điểm tích lũy: Được tính từ trước đến nay",
                    "Chu kỳ thanh toán",
                    "Số tài khoản ngân hàng",
                    "Tên ngân hàng",
                    "Người tạo",
                    "Người sửa cuối",
                };
                for (var sheetNum = 1; sheetNum < numSheets; sheetNum++)
                {
                    var sheet = (Worksheet)workBook.Sheets[sheetNum];
                    var excelRange = sheet.UsedRange;
                    var valueArray = (object[,])excelRange.Value[XlRangeValueDataType.xlRangeValueDefault];

                    #region Init convert to model
                    var length = valueArray.GetLength(0); //all rows
                    var maxColumn = valueArray.GetLength(1); //all columns

                    for (var row = 2; row <= length; row++)
                    {
                        var Name = valueArray[row, ColumnsDefined.IndexOf("Tên khách hàng") + 1].ToString();
                        var PartnerCode = valueArray[row, ColumnsDefined.IndexOf("Mã đối tác của Phoenix") + 1].ToString();
                        var LocationCode = valueArray[row, ColumnsDefined.IndexOf("Thuộc chi nhánh") + 1].ToString();
                        var Address = valueArray[row, ColumnsDefined.IndexOf("Địa chỉ") + 1].ToString();
                        var PhoneNo = valueArray[row, ColumnsDefined.IndexOf("Điện thoại") + 1].ToString();
                        var CardNumber = valueArray[row, ColumnsDefined.IndexOf("Mã thẻ thành viên") + 1].ToString();
                        var IDCard = valueArray[row, ColumnsDefined.IndexOf("Số CMND/CCCD") + 1].ToString();
                        var BirthDay = valueArray[row, ColumnsDefined.IndexOf("Ngày sinh") + 1].ToString();
                        var FaxNo = valueArray[row, ColumnsDefined.IndexOf("Số Fax") + 1].ToString();
                        var Email = valueArray[row, ColumnsDefined.IndexOf("Email") + 1].ToString();
                        var TaxCode = valueArray[row, ColumnsDefined.IndexOf("Mã số thuế") + 1].ToString();
                        var CustomerGroup_Code = valueArray[row, ColumnsDefined.IndexOf("Nhóm khách hàng") + 1].ToString();
                        var CustomerType_Code = valueArray[row, ColumnsDefined.IndexOf("Loại khách hàng") + 1].ToString();
                        var CongNoPhaiThu = valueArray[row, ColumnsDefined.IndexOf("Công nợ phải thu") + 1].ToString();
                        var DiemHienTai = valueArray[row, ColumnsDefined.IndexOf("Điểm hiện tại") + 1].ToString();
                        var DiemTichLuy = valueArray[row, ColumnsDefined.IndexOf("Điểm tích lũy: Được tính từ trước đến nay") + 1].ToString();
                        var ChuKyThanhToan = valueArray[row, ColumnsDefined.IndexOf("Chu kỳ thanh toán") + 1].ToString();
                        var NumberBankAccount = valueArray[row, ColumnsDefined.IndexOf("Số tài khoản ngân hàng") + 1].ToString();
                        var BankName = valueArray[row, ColumnsDefined.IndexOf("Tên ngân hàng") + 1].ToString();
                        var CreatedBy = valueArray[row, ColumnsDefined.IndexOf("Người tạo") + 1].ToString();
                        var LastUpdatedBy = valueArray[row, ColumnsDefined.IndexOf("Người sửa cuối") + 1].ToString();

                        var rowValue = new CustomerModel
                        {
                            Name = Name,
                            PartnerCode = PartnerCode,
                            LocationCode = LocationCode,
                            Address = Address,
                            PhoneNo = PhoneNo,
                            CardNumber = CardNumber,
                            IDCard = IDCard,
                            //BirthDay = BirthDay,
                            FaxNo = FaxNo,
                            Email = Email,
                            TaxCode = TaxCode,
                            CustomerGroup_Code = CustomerGroup_Code,
                            CustomerType_Code = CustomerType_Code,
                            //ConNoPhaiThu = CongNoPhaiThu,
                            //DiemHienTai = DiemHienTai,
                            //DiemTichLuu = DiemTichLuy,
                            //ChuKyThanhToan = ChuKyThanhToan,
                            NumberBankAccount = NumberBankAccount,
                            BankName = BankName,
                            CreatedBy = CreatedBy,
                            LastUpdatedBy = LastUpdatedBy,
                        };
                        values.Add(rowValue);
                    }
                    #endregion
                }
                workBook.Close(false, pathFile);
                Marshal.ReleaseComObject(workBook);
                return values;
            }
            catch (Exception e)
            {
            }
            finally
            {
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            return new List<CustomerModel>();
        }
        #endregion
    }
}
