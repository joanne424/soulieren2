using System.Collections;
using System.ComponentModel;

namespace Banclogix.Controls {
    public abstract class CustomSort : IComparer {
        public string PropertyName { get; private set; }
        public ListSortDirection Direction { get; private set; }

        public CustomSort(ListSortDirection direction, string propName) {
            this.Direction = direction;
            this.PropertyName = propName;
        }

        protected abstract int Compare(object x, object y);

        int IComparer.Compare(object x, object y) {
            return Compare(x, y);
        }
    }
}