using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minifice.FileManagement
{

    public class DataItem<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public DataItem()
        {

        }

        public DataItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

}
