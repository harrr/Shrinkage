//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShrinkageExplorer.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Roll
    {
        public int Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
        public bool Clockwise { get; set; }
        public Nullable<int> DriveId { get; set; }
    
        public virtual Drive Drive { get; set; }
    }
}
