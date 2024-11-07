using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Collections
{
    public class UniqueObservableCollection<T> : ObservableCollection<T> where T : IIdentifiable
    {
        protected override void InsertItem(int index, T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }

            foreach (T existingItem in this)
            {
                if (existingItem.ID == item.ID)
                {
                    Debug.WriteLine($"Item({nameof(T)}) with the same ID ({item.ID}) already exists.");
                    return;
                }
            }

            base.InsertItem(index, item);
        }

        public new void Add(T item)
        {
            InsertItem(Count, item);
        }

        public new T this[int index]
        {
            get => base[index];
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Item cannot be null.");
                }

                foreach (T existingItem in this)
                {
                    if (existingItem.ID == value.ID && !EqualityComparer<T>.Default.Equals(existingItem, base[index]))
                    {
                        Debug.WriteLine($"Item({nameof(T)}) with the same ID ({value.ID}) already exists.");
                        return;
                    }
                }

                base[index] = value;
            }
        }
    }
}
