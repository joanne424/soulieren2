using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM2.Ent.Client.ViewModels.User
{
    using System.Collections.ObjectModel;
    using System.IO;

    using DM2.Ent.Client.ViewModels.Common;

    using Microsoft.Practices.ObjectBuilder2;

    public class PermissionTree : Screen
    {


        private bool? isChecked;

        private ObservableCollection<PermissionTree> permissions;


        public PermissionTree()
        {
            this.isChecked = false;
        }

        public PermissionTree(PermissionTree parent, KeyValuePair<int, string> kvp)
            : this(parent, kvp, false)
        {
        }

        public PermissionTree(PermissionTree parent, KeyValuePair<int, string> kvp, bool isChecked)
        {
            this.Parent = parent;
            this.ItemInfo = kvp;
            this.isChecked = isChecked;
        }

        public KeyValuePair<int, string> ItemInfo { get; set; }

        public PermissionTree Parent { get; set; }

        public ObservableCollection<PermissionTree> Permissions
        {
            get
            {
                if (this.permissions == null)
                {
                    
                }

                return this.permissions;
            }
        }

        public bool? IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.NotifyOfPropertyChange(() => this.IsChecked);
                }
            }
        }


        public static ObservableCollection<PermissionTree> InitRoot()
        {
            var pts = new ObservableCollection<PermissionTree>();

            DriveInfo.GetDrives().Where(driver => driver.DriveType == DriveType.Fixed).ForEach(
                driver =>
                    {
                        var info = new KeyValuePair<int, string>(driver.Name.GetHashCode(), driver.Name);
                        pts.Add(new PermissionTree(null, info));
                    });

            return pts;
        }
    }
}
