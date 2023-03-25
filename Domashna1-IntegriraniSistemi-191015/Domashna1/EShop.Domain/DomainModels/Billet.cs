using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.DomainModels
{
    public class Billet:BaseEntity
    {
        [Required]
        public string MovieName { get; set; }
        [Required]
        public string MovieImage { get; set; }
        [Required]
        public string MovieDescription { get; set; }
        [Required]
        public double BilletPrice { get; set; }
        [Required]
        public double MovieRating { get; set; }
        [Required]
        public DateTime Datum { get; set; }
        [Required]
        public string Zanr { get; set; }

        public virtual ICollection<BiletInShoppingCard> BiletInShoppingCards { get; set; }
        public virtual ICollection<BiletInOrder> Orders { get; set; }

    }
}
