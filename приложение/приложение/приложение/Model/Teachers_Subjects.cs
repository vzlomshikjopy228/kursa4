//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace приложение.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Teachers_Subjects
    {
        public int ID { get; set; }
        public Nullable<int> TeacherID { get; set; }
        public Nullable<int> SubjectID { get; set; }
    
        public virtual Subjects Subjects { get; set; }
        public virtual Teachers Teachers { get; set; }
    }
}
