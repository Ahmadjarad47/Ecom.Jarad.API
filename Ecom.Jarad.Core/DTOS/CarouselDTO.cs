

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Jarad.Core.DTOS
{
    public record CarouselDTO
    ([Required]
     string Title,
        [Required]
     string Description,
        [Required]
     string LinkProduct,
        [Required]
     IFormFile  Image

    );


}
