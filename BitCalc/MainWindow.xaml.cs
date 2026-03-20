// ※ コメントは必ず日本語で記述すること

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BitCalc
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 表示しているウインドウ数の数
        /// </summary>
        public static int WindowCount = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            /* ウインドウ数カウントアップ */
            MainWindow.WindowCount++;

            /* 表示初期化 */
            InitializeComponent();
            this.Icon = Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Application.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()) ;
            MouseLeftButtonDown += ( _, __ ) => { DragMove(); };

            /* 設定読み込み */
            var vm = new MainVM();
            vm.Load();
            this.DataContext = vm;
        }

        /// <summary>
        /// ウインドウ起動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            /* ウインドウの角を丸くして影を付ける */
            var hwnd = new WindowInteropHelper(this).Handle;
            int corner = (int)DwmWindowCornerPreference.Round;
            DwmSetWindowAttribute(
                hwnd,
                DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
                ref corner,
                sizeof( int ) );

            /* 透過表示設定 */
            this.SetTransparency( Properties.Settings.Default.Transparent );

            /* キーの表示設定 */
            this.KeyDisp( Properties.Settings.Default.KeyDisp );
        }

        /// <summary>
        /// ウインドウ終了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
        {

            if( 0 >= --MainWindow.WindowCount )
            {
                /* 最後のウインドウが閉じられた時 */

                /* 設定保存 */
                if( this.DataContext is MainVM vm )
                {
                    vm.Save();
                }

                /* アプリケーション終了 */
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 最小化ボタンクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeClick( object sender, RoutedEventArgs e )
        {
            if( WindowState == WindowState.Maximized )
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// 最大化ボタンクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseClick( object sender, RoutedEventArgs e )
        {
            this.Close();
        }

        /// <summary>
        /// 閉じるボタンクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeClick( object sender, RoutedEventArgs e )
        {
            WindowState = WindowState.Minimized;
        }
        private void CloseWindow_Click( object sender, RoutedEventArgs e )
        {
            this.Close();
        }

        /// <summary>
        /// 終了メニュークリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shutdown_Click( object sender, RoutedEventArgs e )
        {
            Application.Current.Shutdown();
        }


        /// <summary>
        /// 新規ウィンドウメニュークリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewWindow_Click( object sender, RoutedEventArgs e )
        {
            // 現在のウィンドウの設定を保存
            if( this.DataContext is MainVM vm )
            {
                // 設定保存
                vm.Save();
            }

            // 新しいウィンドウを開く
            var newWindow = new MainWindow
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = this.Left + 30,
                Top  = this.Top  + 30
            };

            newWindow.Show();
        }

        /// <summary>
        /// Aboutウインドウ表示メニュークリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAboutWindow_Click( object sender, RoutedEventArgs e )
        {
            //AboutWindowを表示する
            var aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }

        /// <summary>
        /// 透過表示メニュークリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Transparency_Click( object sender, RoutedEventArgs e )
        {
            if( sender is MenuItem menuItem )
            {
                if( menuItem.IsChecked )
                {
                    this.SetTransparency( false );
                }
                else
                {
                    this.SetTransparency( true );
                }
            }
        }

        /// <summary>
        /// ウインドウの透過表示を設定する
        /// </summary>
        /// <param name="transparent">透過表示にする場合はtrue、通常表示にする場合はfalse</param>
        private void SetTransparency( bool transparent )
        {
            Properties.Settings.Default.Transparent = transparent;
            if( transparent )
            {
                this.Background = new SolidColorBrush( (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString( "#CFFFFFFF" ) );
                this.MinimizeButton.Foreground = System.Windows.Media.Brushes.Gray;
                this.MaximizeButton.Foreground = System.Windows.Media.Brushes.Gray;
                this.CloseButton.Foreground = System.Windows.Media.Brushes.Gray;
                this.TransparencyMenu.IsChecked = true;
            }
            else
            {
                this.Background = new SolidColorBrush( (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString( "White" ) );
                this.MinimizeButton.Foreground = System.Windows.Media.Brushes.Black;
                this.MaximizeButton.Foreground = System.Windows.Media.Brushes.Black;
                this.CloseButton.Foreground = System.Windows.Media.Brushes.Black;
                this.TransparencyMenu.IsChecked = false;
            }
        }


        [DllImport( "dwmapi.dll" )]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE attribute,
            ref int pvAttribute,
            int cbAttribute );

        private enum DWMWINDOWATTRIBUTE
        {
            DWMWA_SYSTEMBACKDROP_TYPE = 38,
            DWMWA_WINDOW_CORNER_PREFERENCE = 33
        }

        private enum DwmSystemBackdropType
        {
            Auto = 0,
            None = 1,
            Mica = 2,
            Acrylic = 3,
            MicaAlt = 4
        }

        private enum DwmWindowCornerPreference
        {
            Default = 0,
            DoNotRound = 1,
            Round = 2,
            RoundSmall = 3
        }

        private void KeyClick( object sender, RoutedEventArgs e )
        {
            if( sender is Button button )
            {
                if( button.Content is string str )
                {
                    this.KeyDisp( str );

                    if( this.DataContext is MainVM vm )
                    {
                        vm.UpdateStatusMessage( $"Button '{str}' clicked." );
                    }
                }
            }
        }

        private void KeyDisp( string str )
        {
            if( this.DataContext is MainVM vm )
            {
                switch( str )
                {
                    case "16進数":
                        this.DispHex.Foreground = System.Windows.Media.Brushes.Red;
                        this.DispDec.Foreground = System.Windows.Media.Brushes.Black;
                        this.DispBin.Foreground = System.Windows.Media.Brushes.Black;
                        this.BtnHex.Foreground = System.Windows.Media.Brushes.Red;
                        this.BtnDec.Foreground = System.Windows.Media.Brushes.Black;
                        this.BtnBin.Foreground = System.Windows.Media.Brushes.Black;
                        this.NumA.IsEnabled = true;
                        this.NumB.IsEnabled = true;
                        this.NumC.IsEnabled = true;
                        this.NumD.IsEnabled = true;
                        this.NumE.IsEnabled = true;
                        this.NumF.IsEnabled = true;
                        this.Num9.IsEnabled = true;
                        this.Num8.IsEnabled = true;
                        this.Num7.IsEnabled = true;
                        this.Num6.IsEnabled = true;
                        this.Num5.IsEnabled = true;
                        this.Num4.IsEnabled = true;
                        this.Num3.IsEnabled = true;
                        this.Num2.IsEnabled = true;
                        this.Num1.IsEnabled = true;
                        this.Num0.IsEnabled = true;
                        Properties.Settings.Default.KeyDisp = str;
                        break;
                    case "10進数":
                        this.DispHex.Foreground = System.Windows.Media.Brushes.Black;
                        this.DispDec.Foreground = System.Windows.Media.Brushes.Red;
                        this.DispBin.Foreground = System.Windows.Media.Brushes.Black;
                        this.BtnHex.Foreground = System.Windows.Media.Brushes.Black;
                        this.BtnDec.Foreground = System.Windows.Media.Brushes.Red;
                        this.BtnBin.Foreground = System.Windows.Media.Brushes.Black;
                        this.NumA.IsEnabled = false;
                        this.NumB.IsEnabled = false;
                        this.NumC.IsEnabled = false;
                        this.NumD.IsEnabled = false;
                        this.NumE.IsEnabled = false;
                        this.NumF.IsEnabled = false;
                        this.Num9.IsEnabled = true;
                        this.Num8.IsEnabled = true;
                        this.Num7.IsEnabled = true;
                        this.Num6.IsEnabled = true;
                        this.Num5.IsEnabled = true;
                        this.Num4.IsEnabled = true;
                        this.Num3.IsEnabled = true;
                        this.Num2.IsEnabled = true;
                        this.Num1.IsEnabled = true;
                        this.Num0.IsEnabled = true;
                        Properties.Settings.Default.KeyDisp = str;
                        break;
                    case "2進数":
                        this.DispHex.Foreground = System.Windows.Media.Brushes.Black;
                        this.DispDec.Foreground = System.Windows.Media.Brushes.Black;
                        this.DispBin.Foreground = System.Windows.Media.Brushes.Red;
                        this.BtnHex.Foreground = System.Windows.Media.Brushes.Black;
                        this.BtnDec.Foreground = System.Windows.Media.Brushes.Black;
                        this.BtnBin.Foreground = System.Windows.Media.Brushes.Red;
                        this.NumA.IsEnabled = false;
                        this.NumB.IsEnabled = false;
                        this.NumC.IsEnabled = false;
                        this.NumD.IsEnabled = false;
                        this.NumE.IsEnabled = false;
                        this.NumF.IsEnabled = false;
                        this.Num9.IsEnabled = false;
                        this.Num8.IsEnabled = false;
                        this.Num7.IsEnabled = false;
                        this.Num6.IsEnabled = false;
                        this.Num5.IsEnabled = false;
                        this.Num4.IsEnabled = false;
                        this.Num3.IsEnabled = false;
                        this.Num2.IsEnabled = false;
                        this.Num1.IsEnabled = true;
                        this.Num0.IsEnabled = true;
                        Properties.Settings.Default.KeyDisp = str;
                        break;
                    case "A":
                    case "B":
                    case "C":
                    case "D":
                    case "E":
                    case "F":
                    case "9":
                    case "8":
                    case "7":
                    case "6":
                    case "5":
                    case "4":
                    case "3":
                    case "2":
                    case "1":
                    case "0":
                        if( Properties.Settings.Default.NextValue )
                        {
                            Properties.Settings.Default.NextValue = false;
                            vm.DecValue = "0";
                        }

                        switch( Properties.Settings.Default.KeyDisp )
                        {
                            case "16進数":
                                if( vm.HexValue.Equals( "0" ) )
                                {
                                    vm.HexValue = str;
                                }
                                else
                                {
                                    vm.HexValue = vm.HexValue + str;
                                }
                                break;
                            case "10進数":
                                vm.DecValue = vm.DecValue + str;
                                break;
                            case "2進数":
                                if( vm.BinValue.Equals( "0" ) )
                                {
                                    vm.BinValue = str; 
                                }
                                else
                                {
                                    vm.BinValue = vm.BinValue + str;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case "AC":
                        vm.DecValue = "0";
                        Properties.Settings.Default.Operator = string.Empty;
                        break;
                    case "←":
                        switch( Properties.Settings.Default.KeyDisp )
                        {
                            case "16進数":
                                if( 1 < vm.HexValue.Length )
                                {
                                    vm.HexValue = vm.HexValue.Remove( vm.HexValue.Length - 1);
                                }
                                else
                                {
                                    if( vm.HexValue.Equals( "1" ) )
                                    {
                                        vm.HexValue = "0";
                                    }
                                }
                                break;
                            case "10進数":
                                if( 1 < vm.DecValue.Length )
                                {
                                    vm.DecValue = vm.DecValue.Remove( vm.DecValue.Length - 1 );
                                }
                                else
                                {
                                    if( vm.DecValue.Equals( "1" ) )
                                    {
                                        vm.DecValue = "0";
                                    }
                                }
                                break;
                            case "2進数":
                                if( 1 < vm.BinValue.Length )
                                {
                                    vm.BinValue = vm.BinValue.Remove( vm.BinValue.Length - 1 );
                                }
                                else
                                {
                                    if( vm.BinValue.Equals( "1" ) )
                                    {
                                        vm.BinValue = "0";
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case "％":
                    case "÷":
                    case "×":
                    case "－":
                    case "＋":
                        Properties.Settings.Default.PreviousValue = Properties.Settings.Default.CurrentValue;
                        Properties.Settings.Default.Operator = str;
                        Properties.Settings.Default.NextValue = true;
                        break;
                    case "＝":
                        if( !string.IsNullOrEmpty( Properties.Settings.Default.Operator ) )
                        {
                            vm.Calc();
                        }
                        Properties.Settings.Default.Operator = string.Empty;
                        Properties.Settings.Default.NextValue = true;
                        break;
                    default:
                        break;
                }

            }
        }

    }
}
