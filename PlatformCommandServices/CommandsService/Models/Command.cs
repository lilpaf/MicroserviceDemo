using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandsService.Models
{
    public class Command
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HowTo { get; set; } = null!;

        [Required]
        public string CommandLine { get; set; } = null!;

        [Required]
        public int PlatformId { get; set; }

        [ForeignKey(nameof(PlatformId))]
        public Platform Platform { get; set; } = null!;
    }
}
