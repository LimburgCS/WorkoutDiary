using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Views
{
    public partial class PickerPopup : Popup
    {
        private List<string> _all;

        public PickerPopup(IEnumerable<string> items)
        {
            InitializeComponent();

            _all = items.ToList();
            ListView.ItemsSource = _all;
        }

        void OnSearch(object sender, TextChangedEventArgs e)
        {
            ListView.ItemsSource = _all
                .Where(x => x.Contains(e.NewTextValue ?? "",
                    StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        void OnSelected(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as string;

            if (item != null)
            {
                Close(item); 
            }
        }
    }
}