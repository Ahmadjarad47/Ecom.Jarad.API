using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Jarad.Core.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public virtual AppUsers AppUser { get; set; }
    }
}