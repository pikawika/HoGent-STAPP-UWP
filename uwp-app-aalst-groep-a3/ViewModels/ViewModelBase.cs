using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private DataTemplate _template;

        public DataTemplate Template
        {
            get { return _template; }
            set { _template = value; RaisePropertyChanged("Template"); }
        }

        public ViewModelBase()
        {
            Template = GetTemplate();
        }

        private DataTemplate GetTemplate()
        {
            string s = GetType().Name; //bvb. MainPageViewModel
            return (DataTemplate)App.Current.Resources[s]; //bvb. View:MainPage
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
