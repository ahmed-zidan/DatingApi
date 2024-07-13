using Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApi.DTOS
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public bool PublicId { get; set; }
        
        
    }
}