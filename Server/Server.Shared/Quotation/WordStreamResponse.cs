using System;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Quotation
{
    public class WordStreamResponse
    {
        public int Id { get; set; }
        public string DocumentNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string CustomerEmail { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhoneNo { get; set; }
        public string EmployeeEmail { get; set; }
        public double TongTien { get; set; }
        public double PhanTramChiecKhau { get; set; }
        public double TienChietKhau { get; set; }
        public double PhanTramVAT { get; set; }
        public double TienVAT { get; set; }
        public double ConLai { get; set; }
        public string ThanhToan { get; set; }
        public string BaoHanh { get; set; }
        public string GiaoNhan { get; set; }
        public string GhiChu { get; set; }
        public string Position { get; set; }

        public List<Quotation_LineResponse> Quotation_Line { get; set; }
    }
}
