//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Material
    {
        public Material()
        {
          this.MaterialParameters = new ObservableCollection<IMaterialProperty>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<IMaterialProperty> MaterialParameters { get; set; }
    }
}
