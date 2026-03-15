using System;
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

        public void UpdateStatusMessage( string message )
        {
            this.StatusMessage = message;
            this.RaisePropertyChanged( nameof( this.StatusMessage ) );
        }

        private string m_HexValue = 0.ToString();
        public string HexValue
        {
            get { return m_HexValue; }
            set
            {
                // 入力が空の場合
                if( string.IsNullOrEmpty( value ) )
                {
                    this.m_HexValue = string.Empty;
                    this.UpdateDisp();
                    return;
                }

                try
                {
                    // 16進数を10進数に変換して保存
                    this.m_DecValue = Convert.ToInt64( value, 16 );

                    // 10進数を2進数に変換して保存
                    this.m_BinValue = Convert.ToString(this.m_DecValue, 2);

                    // 10進数を16進数に変換して保存
                    this.m_HexValue = value.ToUpper();
                }
                catch( Exception )
                {
                    // 更新キャンセル
                }

                this.UpdateDisp();
            }
        }

        private long m_DecValue = 0;
        public long DecValue
        {
            get { return m_DecValue; }
            set
            {
                try
                {
                    // 10進数を16進数に変換して保存
                    this.m_HexValue = Convert.ToString( value, 16 ).ToUpper();

                    // 10進数を2進数に変換して保存
                    this.m_BinValue = Convert.ToString( value, 2 );

                    // 10進数を保存
                    this.m_DecValue = value;
                }
                catch( Exception )
                {
                    // 更新キャンセル
                }

                this.UpdateDisp();
            }
        }

        private string m_BinValue = "0";
        public string BinValue
        {
            get { return m_BinValue; }
            set
            {
                // 入力が空の場合
                if( string.IsNullOrEmpty( value ) )
                {
                    this.m_BinValue = string.Empty;
                    this.UpdateDisp();
                    return;
                }

                try
                {
                    // 2進数を10進数に変換して保存
                    this.m_DecValue = Convert.ToInt64( value, 2 );

                    // 10進数を16進数に変換して保存
                    this.m_HexValue = Convert.ToString( this.m_DecValue, 16 ).ToUpper();

                    // 2進数を保存
                    this.m_BinValue = value;
                }
                catch( Exception )
                {
                    // 更新キャンセル
                }

                this.UpdateDisp();
            }
        }

        private void UpdateDisp()
        {
            this.RaisePropertyChanged( nameof( this.HexValue ) );
            this.RaisePropertyChanged( nameof( this.DecValue ) );
            this.RaisePropertyChanged( nameof( this.BinValue ) );
        }

    }
}
