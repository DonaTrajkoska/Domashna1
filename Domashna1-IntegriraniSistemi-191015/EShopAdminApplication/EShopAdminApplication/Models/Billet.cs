using System;

namespace EShopAdminApplication.Models
{
    public class Billet
    {
        public Guid Id { get; set; }
     
        public string MovieName { get; set; }
        
        public string MovieImage { get; set; }
     
        public string MovieDescription { get; set; }
     
        public double BilletPrice { get; set; }
     
        public double MovieRating { get; set; }
   
        public DateTime Datum { get; set; }
     
        public string Zanr { get; set; }

       
    }
}
