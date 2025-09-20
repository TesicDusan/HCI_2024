using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HCI_2024.Resources.Themes
{
    internal class ThemeManager
    {
        public static void ApplyTheme(string themeName)
        {
            string themePath = $"Resources/Themes/{themeName}.xaml";

            var existingDictionaries = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source != null && d.Source.OriginalString.Contains("Resources/Themes/"))
                .ToList();

            foreach (var dict in existingDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Remove(dict);
            }

            var newDict = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(newDict);
        }
    }
}
