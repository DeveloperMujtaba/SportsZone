//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportsZone
{
    using System;
    using System.Collections.Generic;
    
    public partial class player_associations
    {
        public int paid { get; set; }
        public int playerid { get; set; }
        public int clubid { get; set; }
        public int teamid { get; set; }
        public int roleid { get; set; }
        public System.DateTime C_date { get; set; }
    
        public virtual clubs clubs { get; set; }
        public virtual teams teams { get; set; }
        public virtual player_role player_role { get; set; }
        public virtual players players { get; set; }
    }
}