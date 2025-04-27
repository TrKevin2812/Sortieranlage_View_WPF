using S7.Net;
using Sortieranlage_View_WPF.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Sortieranlage_View_WPF.View
{
    /// <summary>
    /// Interaktionslogik für StartseiteView.xaml
    /// </summary>
    public partial class StartseiteView : UserControl
    {
        Plc _plc;

        Ellipse _workpiece;
        Canvas _lineContainer;
        bool _compressor = false;
        bool _conveyorBelt = false;
        bool _ejectorRed = false;
        bool _ejectorBlue = false;
        bool _ejectorWhite = false;
        bool _automaticMode = true;
        bool _workpieceOnConvoyerBelt = true;
        bool _workpieceIsMoving = false;
        int _scannedColor;


        public StartseiteView()
        {
            InitializeComponent();

            DrawLinesInFoerderband();

            DrawStorage(400, 200);
            DrawStorage(490, 200);
            DrawStorage(580, 200);

            DrawWorkPiece(40, 160);

            try
            {
                _plc = PlcZugriffModel.ConnectionToPlc();
                StartLoop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Verbinden: {ex.Message}...Das Programm wird nicht funktionieren");
            }



        }








        private void DrawStorage(int x, int y)
        {

            int rectangleWith = 60;
            int rectangleHeight = 90;


            // Rechtech für Lagerplatz
            Rectangle storage = new Rectangle
            {
                Width = rectangleWith,
                Height = rectangleHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 3,
                Fill = Brushes.White
            };

            // Position des Rechtecks auf dem Canva
            Canvas.SetLeft(storage, x);
            Canvas.SetTop(storage, y);
            MyCanvaLayer1.Children.Add(storage);

            Canvas lineContainerForStorage = new Canvas();
            MyCanvaLayer1.Children.Add(lineContainerForStorage);

            // Horizontale Linien im Rechteck
            for (int i = 15; i < rectangleHeight; i += 15)
            {
                Line line = new Line
                {
                    X1 = x,
                    Y1 = y + i,
                    X2 = x + rectangleWith,
                    Y2 = y + i,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                lineContainerForStorage.Children.Add(line);
            }

        }
        private void DrawLinesInFoerderband()
        {
            _lineContainer = new Canvas();

            _lineContainer = new Canvas();
            MyCanvaLayer1.Children.Add(_lineContainer);

            // Vertikale Linien im Rechteck
            for (int i = 10; i < 700; i += 15)
            {
                Line line = new Line
                {
                    X1 = 30 + i,
                    Y1 = 150,
                    X2 = 30 + i,
                    Y2 = 200,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                _lineContainer.Children.Add(line);
            }
        }

        private void ConvoyerBeltAnimation()
        {
            if (_conveyorBelt)
            {
                return;
            }

            _conveyorBelt = true;
            ConveyorBelt.Fill = Brushes.LightGreen;

            
            // Linienbewegung
            DoubleAnimation moveAnimation = new DoubleAnimation
            {
                From = 0,
                To = 15,
                Duration = TimeSpan.FromMilliseconds(200),
                RepeatBehavior = RepeatBehavior.Forever // Endlosschleife
            };

            _lineContainer.BeginAnimation(Canvas.LeftProperty, moveAnimation);

           

        }

        private void StopConvoyerBeltAnimation()
        {
            _lineContainer.BeginAnimation(Canvas.LeftProperty, null);
            ConveyorBelt.Fill = Brushes.White;

        }

        private void EjectorAnimation(double startPositionStick, double startPositionPlate, UIElement EjectorStick, UIElement EjectorPlate, double moveDistance)
        {

            // Erstelle eine Animation für die Bewegung nach unten

            DoubleAnimation moveStickDown = new DoubleAnimation
            {
                From = startPositionStick,
                To = startPositionStick + moveDistance,
                Duration = TimeSpan.FromMilliseconds(250),
                AutoReverse = true, // Automatisches Zurückfahren nach oben
            };

            DoubleAnimation movePlateDown = new DoubleAnimation
            {
                From = startPositionPlate,
                To = startPositionPlate + moveDistance,
                Duration = TimeSpan.FromMilliseconds(250),
                AutoReverse = true, // Automatisches Zurückfahren nach oben

            };

            // Animation starten
            EjectorPlate.BeginAnimation(Canvas.TopProperty, movePlateDown);
            Thread.Sleep(10);
            EjectorStick.BeginAnimation(Canvas.TopProperty, moveStickDown);
            Thread.Sleep(10);



        }

        private async void EjectorWhite_Animation()
        {
            if (_ejectorWhite)
            {
                return;
            }

            _ejectorWhite = true;

            if (_compressor && (Canvas.GetLeft(_workpiece) > 590 && Canvas.GetLeft(_workpiece) < 650))
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickWhite), Canvas.GetTop(EjectorPlateWhite), EjectorStickWhite, EjectorPlateWhite, 50);

                _workpieceOnConvoyerBelt = false;
                await Task.Delay(300);
                MoveWorkPieceDown();

            }
            else if (!_compressor && (Canvas.GetLeft(_workpiece) > 590 && Canvas.GetLeft(_workpiece) < 650))
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickWhite), Canvas.GetTop(EjectorPlateWhite), EjectorStickWhite, EjectorPlateWhite, 5);
            }
        }

        private async void EjectorBlue_Animation()
        {
            if (_ejectorBlue)
            {
                return;
            }

            _ejectorBlue = true;

            if (_compressor && (Canvas.GetLeft(_workpiece) > 500 && Canvas.GetLeft(_workpiece) < 560))
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickBlue), Canvas.GetTop(EjectorPlateBlue), EjectorStickBlue, EjectorPlateBlue, 50);

                _workpieceOnConvoyerBelt = false;
                await Task.Delay(300);
                MoveWorkPieceDown();

            }
            else if (_compressor)
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickBlue), Canvas.GetTop(EjectorPlateBlue), EjectorStickBlue, EjectorPlateBlue, 50);

            }
            else if (!_compressor && (Canvas.GetLeft(_workpiece) > 500 && Canvas.GetLeft(_workpiece) < 560))
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickBlue), Canvas.GetTop(EjectorPlateBlue), EjectorStickBlue, EjectorPlateBlue, 5);

            }
            else
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickBlue), Canvas.GetTop(EjectorPlateBlue), EjectorStickBlue, EjectorPlateBlue, 5);

            }

        }

        private async void EjectorRed_Animation()
        {
            if (_ejectorRed)
            {
                return;
            }

            _ejectorRed = true;

            if (_compressor && (Canvas.GetLeft(_workpiece) > 405 && Canvas.GetLeft(_workpiece) < 465))
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickRed), Canvas.GetTop(EjectorPlateRed), EjectorStickRed, EjectorPlateRed, 50);

                _workpieceOnConvoyerBelt = false;
                await Task.Delay(300);
                MoveWorkPieceDown();

            }
            else if (_compressor)
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickRed), Canvas.GetTop(EjectorPlateRed), EjectorStickRed, EjectorPlateRed, 50);
            }
            else if (!_compressor && (Canvas.GetLeft(_workpiece) > 405 && Canvas.GetLeft(_workpiece) < 465))
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickRed), Canvas.GetTop(EjectorPlateRed), EjectorStickRed, EjectorPlateRed, 5);
            }
            else
            {
                EjectorAnimation(Canvas.GetTop(EjectorStickRed), Canvas.GetTop(EjectorPlateRed), EjectorStickRed, EjectorPlateRed, 5);
            }

        }

        private async void MoveWorkPieceDown()
        {
            while (Canvas.GetTop(_workpiece) < 250)
            {
                double y = Canvas.GetTop(_workpiece);

                Canvas.SetTop(_workpiece, y + 0.5);

                await Task.Delay(1);

            }

            if (Canvas.GetTop(_workpiece) == 250)
            {
                await Task.Delay(1500);
                MyCanvaLayer1.Children.Remove(_workpiece);
                DrawWorkPiece(40, 160);
                _compressor = false;
                _conveyorBelt = false;
                _ejectorRed = false;
                _ejectorBlue = false;
                _ejectorWhite = false;
                _automaticMode = true;
                _workpieceOnConvoyerBelt = true;
                _workpieceIsMoving = false;
            }

        }

        private void DrawWorkPiece(double x, double y)
        {
            _workpiece = new Ellipse
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.Magenta,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            Canvas.SetLeft(_workpiece, x);
            Canvas.SetTop(_workpiece, y);
            MyCanvaLayer1.Children.Add(_workpiece);
            _workpieceOnConvoyerBelt = true;
        }
        private async void CompressorAnimation()
        {
            if (_compressor)
            {
                return;
            }

            _compressor = true;

            double startX = Canvas.GetLeft(Compressor);

            DoubleAnimation vibration = new DoubleAnimation
            {
                From = startX - 2,
                To = startX + 2,
                Duration = TimeSpan.FromMilliseconds(10),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            Compressor.BeginAnimation(Canvas.LeftProperty, vibration);
        }

        private void StopCompressorAnimation()
        {
            Compressor.BeginAnimation(Canvas.LeftProperty, null);
            _compressor = false;
        }

        private async void MoveWorkPieceToRight()
        {
            if (_workpieceIsMoving)
            {
                return;
            }

            _workpieceIsMoving = true;

            while (_workpieceOnConvoyerBelt && (ConveyorBelt.Fill == Brushes.LightGreen))
            {
                double x = Canvas.GetLeft(_workpiece);

                Canvas.SetLeft(_workpiece, x + 0.5);

                await Task.Delay(5);

                if (Canvas.GetLeft(_workpiece) == 250 && _automaticMode)
                {
                    //StopConvoyerBeltAnimation();
                    ////ScannedColor();
                    ColorScanned();
                    //await Task.Delay(500);
                    //ConvoyerBeltAnimation();
                }
                else if (Canvas.GetLeft(_workpiece) == 600 && _automaticMode && _workpiece.Fill == Brushes.White && _compressor)
                {
                    EjectorAnimation(Canvas.GetTop(EjectorStickWhite), Canvas.GetTop(EjectorPlateWhite), EjectorStickWhite, EjectorPlateWhite, 50);
                    MoveWorkPieceDown();
                    return;
                }
                else if (Canvas.GetLeft(_workpiece) == 510 && _automaticMode && _workpiece.Fill == Brushes.Blue && _compressor)
                {
                    EjectorAnimation(Canvas.GetTop(EjectorStickBlue), Canvas.GetTop(EjectorPlateBlue), EjectorStickBlue, EjectorPlateBlue, 50);
                    MoveWorkPieceDown();
                    return;
                }
                else if (Canvas.GetLeft(_workpiece) == 415 && _automaticMode && _workpiece.Fill == Brushes.Red && _compressor)
                {
                    EjectorAnimation(Canvas.GetTop(EjectorStickRed), Canvas.GetTop(EjectorPlateRed), EjectorStickRed, EjectorPlateRed, 50);
                    MoveWorkPieceDown();
                    return;
                }
                else if (Canvas.GetLeft(_workpiece) == 175 && !_automaticMode)
                {
                    //ScannedColor();
                    ColorScanned();

                }
                else if (Canvas.GetLeft(_workpiece) > 730)
                {
                    await Task.Delay(1500);
                    MyCanvaLayer1.Children.Remove(_workpiece);
                    DrawWorkPiece(40, 160);
                    _compressor = false;
                    _conveyorBelt = false;
                    _ejectorRed = false;
                    _ejectorBlue = false;
                    _ejectorWhite = false;
                    _automaticMode = true;
                    _workpieceOnConvoyerBelt = true;
                    _workpieceIsMoving = false;
                    return;
                }
                else if (Canvas.GetLeft(_workpiece) > 240)
                {
                    CompressorAnimation();
                }

            }

            _workpieceIsMoving = false; // das koennte ein fehler verursachen!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        private void ColorScanned()
        {
            if (_scannedColor == 0)
            {
                _workpiece.Fill = Brushes.Magenta;
            }
            else if (_scannedColor == 1)
            {
                _workpiece.Fill = Brushes.White;
            }
            else if (_scannedColor == 2)
            {
                _workpiece.Fill = Brushes.Blue;
            }
            else if (_scannedColor == 3)
            {
                _workpiece.Fill = Brushes.Red;
            }
        }



















        private async void StartLoop()
        {
            await Task.Run(() => BackgroundLoop());
        }

        private void BackgroundLoop()
        {
            while (_plc.IsConnected)
            {

                try
                {
                    Object[] werte = PlcZugriffModel.ReadDB(1, 1, 6, _plc);

                    if ((bool)werte[0])
                    {
                        Dispatcher.Invoke(async () => { lmp_Foerderband.Background = Brushes.YellowGreen; ConvoyerBeltAnimation(); MoveWorkPieceToRight(); });
                    }
                    else
                    {
                        Dispatcher.Invoke(() => { lmp_Foerderband.Background = Brushes.IndianRed; _conveyorBelt = false; StopConvoyerBeltAnimation(); }); // mach den schalter in die methode!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    }

                    if ((bool)werte[1])
                    {
                        Dispatcher.Invoke((Delegate)(() => { lmp_Kompressor.Background = Brushes.YellowGreen; CompressorAnimation(); }));
                    }
                    else
                    {
                        Dispatcher.Invoke((Delegate)(() => { lmp_Kompressor.Background = Brushes.IndianRed; _compressor = false; StopCompressorAnimation(); }));
                    }

                    if ((bool)werte[2])
                    {
                        Dispatcher.Invoke(() => { lmp_VentilRot.Background = Brushes.YellowGreen; EjectorRed_Animation(); });
                    }
                    else
                    {
                        Dispatcher.Invoke(() => { lmp_VentilRot.Background = Brushes.IndianRed; _ejectorRed = false; });
                    }

                    if ((bool)werte[3])
                    {
                        Dispatcher.Invoke(() => { lmp_VentilBlau.Background = Brushes.YellowGreen; EjectorBlue_Animation(); });
                    }
                    else
                    {
                        Dispatcher.Invoke(() => { lmp_VentilBlau.Background = Brushes.IndianRed; _ejectorBlue = false; });
                    }

                    if ((bool)werte[4])
                    {
                        Dispatcher.Invoke(() => { lmp_VentilWeiss.Background = Brushes.YellowGreen; EjectorWhite_Animation(); });
                    }
                    else
                    {
                        Dispatcher.Invoke(() => { lmp_VentilWeiss.Background = Brushes.IndianRed; _ejectorWhite = false; });
                    }

                    if ((bool)werte[5])
                    {
                        Dispatcher.Invoke(() => { lmp_Fehler.Background = Brushes.YellowGreen; });
                    }
                    else
                    {
                        Dispatcher.Invoke(() => { lmp_Fehler.Background = Brushes.IndianRed; });
                    }

                    Object[] Farbwerte = PlcZugriffModel.ReadDB(1, 2, 3, _plc);

                    if ((bool)Farbwerte[0])
                    {
                        Dispatcher.Invoke(() => { txt_Farbsensor.Text = "Weiß"; txt_Farbsensor.Foreground = Brushes.White; _scannedColor = 1; });
                    }
                    else if ((bool)Farbwerte[1])
                    {
                        Dispatcher.Invoke(() => { txt_Farbsensor.Text = "Blau"; txt_Farbsensor.Foreground = Brushes.Blue; _scannedColor = 2; });
                    }
                    else if ((bool)Farbwerte[2])
                    {
                        Dispatcher.Invoke(() => { txt_Farbsensor.Text = "Rot"; txt_Farbsensor.Foreground = Brushes.Red; _scannedColor = 3; });
                    }
                    else
                    {
                        Dispatcher.Invoke(() => { txt_Farbsensor.Text = "Nix"; txt_Farbsensor.Foreground = Brushes.Gray; _scannedColor = 0; });
                    }

                    //if (ConveyorBelt.Fill == Brushes.YellowGreen)
                    //{
                    //    Dispatcher.Invoke( async () => { MoveWorkPieceToRight(); });  //Das ist neu!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    //}

                    Dispatcher.Invoke(() => { txt_LagerplatzWeiss.Text = $"{PlcZugriffModel.ReadUInt16DB(1, 4, _plc)}"; });
                    Dispatcher.Invoke(() => { txt_LagerplatzBlau.Text = $"{PlcZugriffModel.ReadUInt16DB(1, 6, _plc)}"; });
                    Dispatcher.Invoke(() => { txt_LagerplatzRot.Text = $"{PlcZugriffModel.ReadUInt16DB(1, 8, _plc)}"; });


                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Auslesen: {ex.Message}");
                }



            }
        }
  

    }
}
