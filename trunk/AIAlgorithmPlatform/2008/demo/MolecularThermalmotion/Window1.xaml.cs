using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace MolecularThermalmotion
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Game game = null;

        public Window1()
        {
            InitializeComponent();

            //this.Closed += delegate { if (timerPump != null) { timerPump.AbortPump(); } };
        }

        private void StartBotton_Click(object sender, RoutedEventArgs e)
        {
            game = new Game();
            game.StartGame();
        }

        private void StopBotton_Click(object sender, RoutedEventArgs e)
        {
            game.StopGame();
        }
    }
}
