using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("App_Setting")]
    public class AppSetting
    {
        [Key]
        [MaxLength(100)]
        [Column("key")]
        public string Key { get; set; } = string.Empty;

        [Required]
        [Column("value")]
        public string Value { get; set; } = string.Empty;
    }
}
