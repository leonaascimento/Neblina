using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Binder.Api.Core.Models
{
    public class Bank
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BankId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [Url]
        public string ReceiveUrl { get; set; }

        [Url]
        public string StatusUrl { get; set; }
    }
}
