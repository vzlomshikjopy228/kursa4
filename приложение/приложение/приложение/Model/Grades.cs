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
    
    public partial class Grades
    {
        public int ID { get; set; }
        public byte GradeValue { get; set; }
        public Nullable<int> SubjectID { get; set; }
        public Nullable<int> TeacherID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Students Students { get; set; }
        public virtual Subjects Subjects { get; set; }
        public virtual Teachers Teachers { get; set; }
    }
}
