using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;
        private bool isShooting;


        public double SliderSkaliranje
        {
            get;
            set;
        }
        public bool IsShooting
        {
            get;
            set;
        }
        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Ball"), "BallDAE.dae", (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }
        }

        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.WindowWidth = (int)window.ActualWidth;
            m_world.WindowHeight = (int)window.ActualHeight;
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (isShooting)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.F2: this.Close(); break;
                case Key.E : m_world.RotationX -= 5.0f; break;
                case Key.D: m_world.RotationX += 5.0f; break;
                case Key.S: m_world.RotationY -= 5.0f; break;
                case Key.F: m_world.RotationY += 5.0f; break;
                case Key.PageUp: m_world.Zoom += 30.0f; break;
                case Key.PageDown: m_world.Zoom -= 30.0f; break;
                case Key.V:
                    m_world.startShooting(this);
                    textBox.IsEnabled = false;
                    textBox1.IsEnabled = false;
                    textBox2.IsEnabled = false;
                    isShooting = true;
                    break;
                case Key.F4:
                    OpenFileDialog opfModel = new OpenFileDialog();
                    bool result = (bool) opfModel.ShowDialog();
                    if (result)
                    {

                        try
                        {
                            World newWorld = new World(Directory.GetParent(opfModel.FileName).ToString(), Path.GetFileName(opfModel.FileName), (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
                            m_world.Dispose();
                            m_world = newWorld;
                            m_world.Initialize(openGLControl.OpenGL);
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta:\n" + exp.Message, "GRESKA", MessageBoxButton.OK );
                        }
                    }
                    break;
            }
        }
        

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            double fSkaliranja = 1;
            if (IsTextAllowed(textBox.Text))
            {                
                double.TryParse(textBox.Text,out fSkaliranja);
            }
            if(textBox.Text!="" )
                 m_world.skalirajLoptu(fSkaliranja);

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            double bRotacije = 1;
            if (IsTextAllowed(textBox1.Text))
            {
                double.TryParse(textBox1.Text, out bRotacije);
            }
            if (textBox1.Text != "")
                m_world.promeniBRotacije(bRotacije);
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            double visina = 1;
            if (IsTextAllowed(textBox2.Text))
            {
                double.TryParse(textBox2.Text, out visina);
            }
            if (textBox2.Text != "")
                m_world.promeniVisinu(visina);
        }


        internal void animationDone()
        {
            textBox.IsEnabled = true;
            textBox1.IsEnabled = true;
            textBox2.IsEnabled = true;
            isShooting = false;
        }
    }
}
