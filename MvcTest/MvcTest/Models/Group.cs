using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcTest.Models
{
    public class Group
    {
        [Key]
        public int GId { get; set; }
        [Required (ErrorMessage = "Необходимо название группы")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Необходим ID пользователя")]
        public int UId { get; set; }

    }
}