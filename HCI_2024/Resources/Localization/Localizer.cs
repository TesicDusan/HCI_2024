using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HCI_2024.Resources.Localization
{
    public class Localizer : INotifyPropertyChanged
    {
        private static Localizer _instance = new Localizer();
        public static Localizer Instance => _instance;

        private static ResourceManager _resourceManager =
            new ResourceManager("HCI_2024.Resources.Localization.Strings", typeof(Localizer).Assembly);

        public string this[string key] => _resourceManager.GetString(key, System.Globalization.CultureInfo.CurrentUICulture);

        public event PropertyChangedEventHandler PropertyChanged;

        public static void ChangeLanguage(string cultureCode)
        {
            var culture = new System.Globalization.CultureInfo(cultureCode);
            System.Globalization.CultureInfo.CurrentUICulture = culture;
            _instance.PropertyChanged?.Invoke(_instance, new PropertyChangedEventArgs(null));
        }
    }
}
