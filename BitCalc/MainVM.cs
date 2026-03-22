using System;
using System.ComponentModel;
using System.Net.Http.Headers;
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

            this.DecValue = Properties.Settings.Default.CurrentValue.ToString();
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

        private string m_BaseType = "uint";
        public string BaseType
        {
            get { return this.m_BaseType; }
            set
            {
                this.m_BaseType = value;
                this.DecValue = this.DecValue;
            }
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
                    // 文字列を10進数に変換
                    ulong decValue = string.IsNullOrEmpty( value ) ? 0 : Convert.ToUInt64( value, 16 );

                    switch( this.BaseType )
                    {
                        case "byte":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked((byte)decValue), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked((byte)decValue);

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked((byte)decValue), 2 );
                            break;
                        case "ushort":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked((ushort)decValue), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked((ushort)decValue);

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked((ushort)decValue), 2 );
                            break;
                        case "uint":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked((uint)decValue), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked((uint)decValue);

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked((uint)decValue), 2 );
                            break;
                        case "uint64":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = decValue.ToString( "X)" );

                            // 10進数を保存
                            this.m_DecValue = decValue;

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = MainVM.ConvertUlongToBinary( decValue );
                            break;
                        default:
                            break;
                    }

                    // 現在の値を記録
                    Properties.Settings.Default.CurrentValue = this.m_DecValue;
                }
                catch( Exception )
                {
                    // 更新キャンセル
                }

                this.UpdateDisp();
            }
        }

        private ulong m_DecValue = 0;
        public string DecValue
        {
            get { return m_DecValue.ToString(); }
            set
            {
                try
                {
                    // 文字列を10進数に変換
                    ulong decValue = string.IsNullOrEmpty( value ) ? 0 : Convert.ToUInt64( value, 10 );

                    switch( this.BaseType )
                    {
                        case "byte":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked( (byte)decValue ), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked( (byte)decValue );

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked( (byte)decValue ), 2 );
                            break;
                        case "ushort":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked( (ushort)decValue ), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked((ushort)decValue);

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked( (ushort)decValue ), 2 );
                            break;
                        case "uint":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked( (uint)decValue ), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked((uint)decValue);

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked( (uint)decValue ), 2 );
                            break;
                        case "uint64":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = decValue.ToString("X)");

                            // 10進数を保存
                            this.m_DecValue = decValue;

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = MainVM.ConvertUlongToBinary( decValue );
                            break;
                        default:
                            break;
                    }

                    // 現在の値を記録
                    Properties.Settings.Default.CurrentValue = this.m_DecValue;
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
                    // 文字列を10進数に変換
                    ulong decValue = string.IsNullOrEmpty( value ) ? 0 : Convert.ToUInt64( value, 2 );

                    switch( this.BaseType )
                    {
                        case "byte":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked((byte)decValue), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked((byte)decValue);

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked((byte)decValue), 2 );
                            break;
                        case "ushort":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked((ushort)decValue), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked((ushort)decValue);

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked((ushort)decValue), 2 );
                            break;
                        case "uint":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = Convert.ToString( unchecked((uint)decValue), 16 ).ToUpper();

                            // 10進数を保存
                            this.m_DecValue = unchecked((uint)decValue);

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = Convert.ToString( unchecked((uint)decValue), 2 );
                            break;
                        case "uint64":
                            // 10進数を16進数に変換して保存
                            this.m_HexValue = decValue.ToString( "X)" );

                            // 10進数を保存
                            this.m_DecValue = decValue;

                            // 10進数を2進数に変換して保存
                            this.m_BinValue = MainVM.ConvertUlongToBinary( decValue );
                            break;
                        default:
                            break;
                    }

                    // 現在の値を記録
                    Properties.Settings.Default.CurrentValue = this.m_DecValue;
                }
                catch( Exception )
                {
                    // 更新キャンセル
                }

                this.UpdateDisp();
            }
        }

        public void Calc()
        {
            ulong result = 0;
            switch( Properties.Settings.Default.Operator )
            {
                case "％":
                    result = Properties.Settings.Default.PreviousValue % Properties.Settings.Default.CurrentValue;
                    break;
                case "÷":
                    result = Properties.Settings.Default.PreviousValue / Properties.Settings.Default.CurrentValue;
                    break;
                case "×":
                    result = Properties.Settings.Default.PreviousValue * Properties.Settings.Default.CurrentValue;
                    break;
                case "－":
                    result = Properties.Settings.Default.PreviousValue - Properties.Settings.Default.CurrentValue;
                    break;
                case "＋":
                    result = Properties.Settings.Default.PreviousValue + Properties.Settings.Default.CurrentValue;
                    break;
                default:
                    break;
            }

            switch( Properties.Settings.Default.KeyDisp )
            {
                case "16進数":
                    this.UpdateStatusMessage( $"{ Convert.ToString( (uint)Properties.Settings.Default.PreviousValue, 16 ).ToUpper() } { Properties.Settings.Default.Operator } { Convert.ToString( (uint)Properties.Settings.Default.CurrentValue, 16 ).ToUpper() } ＝ { Convert.ToString( (uint)result, 16 ).ToUpper() }" );
                    break;
                case "10進数":
                    this.UpdateStatusMessage( $"{ Properties.Settings.Default.PreviousValue } { Properties.Settings.Default.Operator } { Properties.Settings.Default.CurrentValue } ＝ { result }" );
                    break;
                case "2進数":
                    this.UpdateStatusMessage( $"{ Convert.ToString( (uint)Properties.Settings.Default.PreviousValue, 2 ).ToUpper() } { Properties.Settings.Default.Operator } { Convert.ToString( (uint)Properties.Settings.Default.CurrentValue, 2 ) } ＝ { Convert.ToString( (uint)result, 2 ) }" );
                    break;
                default:
                    break;
            }

            this.DecValue = result.ToString();
        }

        private void UpdateDisp()
        {
            this.RaisePropertyChanged( nameof( this.HexValue ) );
            this.RaisePropertyChanged( nameof( this.DecValue ) );
            this.RaisePropertyChanged( nameof( this.BinValue ) );
        }

        private static string ConvertUlongToBinary( ulong value )
        {
            char[] bits = new char[64];

            for( int i = 0; i < 64; i++ )
            {
                bits[63 - i] = ( ( value >> i ) & 1 ) == 1 ? '1' : '0';
            }

            return new string( bits ).TrimStart( '0' );
        }

    }
}
