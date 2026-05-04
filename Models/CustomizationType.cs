using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Customization_type")]
    public class CustomizationType
    {
        [Key]
        [Column("type_id")]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("is_multi_select")]
        public bool IsMultiSelect { get; set; } = false;

        public virtual ICollection<CustomizationOption> CustomizationOptions { get; set; } = new List<CustomizationOption>();
    }
}
