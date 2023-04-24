using System.ComponentModel;

namespace Diploma.Client.Core.MVVM.ViewModel
{
    /// <summary>
    /// Базовый класс ViewModel
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertiesChanged(params string[] propertiesNames)
        {
            foreach (var propertyName in propertiesNames)
            {
                RaisePropertyChanged(propertyName);
            }
        }
    }
}