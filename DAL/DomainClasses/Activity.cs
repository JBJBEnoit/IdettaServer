
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Newtonsoft.Json;
using IdettaTestServer.DAL.Helpers;

namespace IdettaTestServer.DAL.DomainClasses
{
    public class Activity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Func { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Filename { get; set; }
        public ICollection<ActivityDetails>? Details { get; set; }
    }
}
