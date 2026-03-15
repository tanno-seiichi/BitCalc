using System.ComponentModel;
using System.Reflection;

namespace BitCalc
{
    internal class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged( string propertyName )
        {
            this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public string AppTitle { get; set; } = string.Empty;

        public string StatusMessage { get; set; } = string.Empty;

        public void Load()
        {
            this.AppTitle = Assembly.GetExecutingAssembly().GetName().Name + " v" + Assembly.GetExecutingAssembly().GetName().Version;
            this.RaisePropertyChanged( nameof( this.AppTitle ) );
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }

    }
}
