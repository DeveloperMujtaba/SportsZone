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
    
    public partial class games_positions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public games_positions()
        {
            this.coach_associations = new HashSet<coach_associations>();
            this.coach_associations_request = new HashSet<coach_associations_request>();
            this.coachs = new HashSet<coachs>();
        }
    
        public int positionid { get; set; }
        public int gameid { get; set; }
        public string position { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<coach_associations> coach_associations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<coach_associations_request> coach_associations_request { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<coachs> coachs { get; set; }
        public virtual games games { get; set; }
    }
}