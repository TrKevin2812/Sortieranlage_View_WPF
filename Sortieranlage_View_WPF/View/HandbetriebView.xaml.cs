using S7.Net;
using Sortieranlage_View_WPF.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sortieranlage_View_WPF.View
{
    /// <summary>
    /// Interaktionslogik für HandbetriebView.xaml
    /// </summary>
    public partial class HandbetriebView : UserControl
    {
        Plc _plc;

        BedienschalterModel _bedienschalter = new BedienschalterModel();
        KompressorModel _kompressor = new KompressorModel();
        MotorModel _motor = new MotorModel();
        VentilRotModel _ventilRot = new VentilRotModel();
        VentilBlauModel _ventilBlau = new VentilBlauModel();
        VentilWeissModel _ventilWeiss = new VentilWeissModel();

        public HandbetriebView()
        {
            InitializeComponent();

            try
            {
                _plc = PlcZugriffModel.ConnectionToPlc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Verbinden: {ex}");
            }

        }



        private void btn_bedienschalter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_bedienschalter.Activated)
                {
                    _bedienschalter.SwitchMode(true, _plc);
                    btn_bedienschalter.Content = "Connected";
                    btn_bedienschalter.BorderBrush = Brushes.YellowGreen;
                    txt_Verbindung.Text = "ONLINE: Bereit zum ausführen";
                    txt_Verbindung.Foreground = Brushes.YellowGreen;

                }
                else
                {
                    _bedienschalter.SwitchMode(false, _plc);
                    btn_bedienschalter.Content = "Connect";
                    btn_bedienschalter.BorderBrush = Brushes.Red;
                    txt_Verbindung.Text = "OFFLINE: Bedienschalter muss betätigt werden";
                    txt_Verbindung.Foreground = Brushes.Red;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }


        private void btn_foerderband_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                _motor.SwitchMode(true, _plc);
                btn_foerderband.BorderBrush = Brushes.YellowGreen;
                btn_foerderband.Content = "ON";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void btn_foerderband_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                _motor.SwitchMode(false, _plc);
                btn_foerderband.BorderBrush = Brushes.Red;
                btn_foerderband.Content = "OFF";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }

        }




        private void btn_kompressor_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!_kompressor.Activated)
                {
                    _kompressor.SwitchMode(true, _plc);
                    btn_kompressor.Content = "ON";
                    btn_kompressor.BorderBrush = Brushes.YellowGreen;
                }
                else
                {
                    _kompressor.SwitchMode(false, _plc);
                    btn_kompressor.Content = "OFF";
                    btn_kompressor.BorderBrush = Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }




       
        private void btn_ventil_weiss_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                _ventilWeiss.SwitchMode(true, _plc);
                btn_ventil_weiss.BorderBrush = Brushes.YellowGreen;
                btn_ventil_weiss.Content = "ON";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void btn_ventil_weiss_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                _ventilWeiss.SwitchMode(false, _plc);
                btn_ventil_weiss.BorderBrush = Brushes.Red;
                btn_ventil_weiss.Content = "OFF";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }





        private void btn_ventil_blau_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                _ventilBlau.SwitchMode(true, _plc);
                btn_ventil_blau.BorderBrush = Brushes.YellowGreen;
                btn_ventil_blau.Content = "ON";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void btn_ventil_blau_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                _ventilBlau.SwitchMode(false, _plc);
                btn_ventil_blau.BorderBrush = Brushes.Red;
                btn_ventil_blau.Content = "OFF";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }





        private void btn_ventil_rot_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                _ventilRot.SwitchMode(true, _plc);
                btn_ventil_rot.BorderBrush = Brushes.YellowGreen;
                btn_ventil_rot.Content = "ON";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }

        }

        private void btn_ventil_rot_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                _ventilRot.SwitchMode(false, _plc);
                btn_ventil_rot.BorderBrush = Brushes.Red;
                btn_ventil_rot.Content = "OFF";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }
    }
}
