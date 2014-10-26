﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ShrinkageExplorer.Wpf.Utilities
{
  public class Caretaker<T> : IEditableObject
  {
    private Memento<T> memento;
    private T target;

    public T Target
    {
      get
      {
        return this.target;
      }
      protected set
      {
        if (value == null)
        {
          throw new ArgumentNullException("Target", "Target cannot be null");
        }

        if (Object.ReferenceEquals(this.Target, value))
          return;

        this.target = value;
      }
    }

    public Caretaker(T target)
    {
      this.Target = target;
    }

    public void BeginEdit()
    {
      if (this.memento == null)
        this.memento = new Memento<T>(this.Target);
    }

    public void CancelEdit()
    {
      if (this.memento == null)
        throw new ArgumentNullException("Memento", "BeginEdit() is not invoked");

      this.memento.Restore(Target);
      this.memento = null;
    }

    public void EndEdit()
    {
      if (this.memento == null)
        throw new ArgumentNullException("Memento", "BeginEdit() is not invoked");

      this.memento = null;
    }
  }
}
