using System.Windows;
using System.Windows.Controls;

namespace DM2.Ent.Client.ViewModels
{
    public class DataGridText : TextBlock
    {
        #region 字段

        /// <summary>
        /// 显示消息依赖项属性
        /// </summary>
        public static readonly DependencyProperty StationInfoDisplayProperty =
            DependencyProperty.Register("StationInfoDisplay", typeof(object), typeof(DataGridText));

        #endregion

        public object StationInfoDisplay
        {
            get
            {
                return this.GetValue(StationInfoDisplayProperty);
            }

            set
            {
                this.SetValue(StationInfoDisplayProperty, value);
            }
        }
    }
}
