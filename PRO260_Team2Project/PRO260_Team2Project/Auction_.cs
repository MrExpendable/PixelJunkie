
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace PRO260_Team2Project
{

using System;
    using System.Collections.Generic;
    
public partial class Auction_
{

    public int ImageID { get; set; }

    public int PosterID { get; set; }

    public int HighestBidderID { get; set; }

    public long CurrentBid { get; set; }

    public Nullable<System.DateTime> ExpirationDate { get; set; }



    public virtual ImageOwner ImageOwner { get; set; }

    public virtual Member Member { get; set; }

}

}
