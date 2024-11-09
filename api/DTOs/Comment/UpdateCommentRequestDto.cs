using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Comment
{
    public class UpdateCommentRequestDto
    {
        
        [Required]
        [MinLength(5, ErrorMessage = "Başlık en az 5 karakter olabilir.")]
        [MaxLength(140, ErrorMessage ="Başlık en fazla 140 karakter olabilir.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "İçerik en az 5 karakter olabilir.")]
        [MaxLength(140, ErrorMessage ="İçerik en fazla 140 karakter olabilir.")]
        public string Content { get; set; } = string.Empty;
    }
}