//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Team2Project
{
    using System;
    using System.Collections.Generic;

    public partial class Flag
    {
        public int FlagID { get; set; }
        public int FlaggerID { get; set; }
        public int ImageID { get; set; }
        public System.DateTime TimeOfFlag { get; set; }
        public string Description { get; set; }

        public virtual Image Image { get; set; }
    }
}