//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TAW_Server.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rating
    {
        public int RatingID { get; set; }
        public int JokeID { get; set; }
        public int UserID { get; set; }
        public int Rating1 { get; set; }
    
        public virtual Joke Joke { get; set; }
        public virtual User User { get; set; }
    }
}
