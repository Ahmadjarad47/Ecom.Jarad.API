

using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Jarad.Core.Entities
{
    public class SubCategory : BaseEntity<int>
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public int CountItems { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Categories { get; set; }
    }
}
