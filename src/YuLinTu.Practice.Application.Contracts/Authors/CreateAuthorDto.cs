using System;
using System.ComponentModel.DataAnnotations;

namespace YuLinTu.Practice.Authors
{
    public class CreateAuthorDto
    {
        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }
    }
}