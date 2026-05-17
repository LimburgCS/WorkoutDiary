using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.Views;

namespace WorkoutDiary.Helper
{
    public static class PickerService
    {
        public static async Task<string> ShowPicker(Page page, IEnumerable<string> items)
        {
            var popup = new PickerPopup(items);
            var result = await page.ShowPopupAsync(popup);
            return result?.ToString();
        }
    }
}
