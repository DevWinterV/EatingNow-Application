using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaiPhucVinh.Server.Data.Entity
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int LogLevelId { get; set; }
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        [MaxLength(100)]
        public string IpAddress { get; set; }
        public int? FalconUserId { get; set; }
        public string PageUrl { get; set; }
        public string ReferrerUrl { get; set; }
        [Required]
        public DateTime CreatedOnUtc { get; set; }
        [Required]
        public LogLevel LogLevel { get; set; }
    }
    public enum LogLevel
    {
        Debug = 10,
        Information = 20,
        Warning = 30,
        Error = 40,
        Fatal = 50
    }
}
