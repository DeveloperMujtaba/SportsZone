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
    
    public partial class player_role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public player_role()
        {
            this.player_associations = new HashSet<player_associations>();
            this.player_associations_request = new HashSet<player_associations_request>();
        }
    
        public int roleid { get; set; }
        public int gameid { get; set; }
        public string rolename { get; set; }
    
        public virtual games games { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<player_associations> player_associations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<player_associations_request> player_associations_request { get; set; }
    }
}
