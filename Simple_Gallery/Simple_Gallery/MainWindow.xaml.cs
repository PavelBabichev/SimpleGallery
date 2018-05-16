using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Threading;

namespace Simple_Gallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> files=new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            list.SelectionChanged += list_SelectionChanged;
        }

        void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(files[(sender as System.Windows.Controls.ListBox).SelectedIndex], UriKind.Relative);
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.EndInit();
            mainImg.Source = bi;
            slider.Value = slider.Maximum / 2;
            expander.Content = System.IO.Path.GetFileName(files[(sender as System.Windows.Controls.ListBox).SelectedIndex]);
        }

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                string[] pathes = Directory.GetFiles(folderBrowser.SelectedPath);
                files = pathes.ToList<string>();
            }
            Duration duration = new Duration(TimeSpan.FromSeconds(3));
            DoubleAnimation doubleanimation = new DoubleAnimation(100.0, duration);
            progressBar.BeginAnimation(System.Windows.Controls.ProgressBar.ValueProperty, doubleanimation);
            for (int i = 0; i < files.Count;i++ )
            {
                FileInfo fileInf = new FileInfo(files[i]);
                if (fileInf.Extension != ".png" && fileInf.Extension != ".jpg")
                {
                    files.Remove(files[i]);
                }
            }   
        }

        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (progressBar.Value == progressBar.Maximum)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(files[0], UriKind.Relative);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                mainImg.Source = bi;
                slider.Value =slider.Maximum/2;
                expander.Content =System.IO.Path.GetFileName(files[0]);
                foreach (string item in files)
                {
                    list.Items.Add(item);
                    System.Windows.Controls.Button btn = new System.Windows.Controls.Button();
                    Image img=new Image();
                    BitmapImage bit = new BitmapImage();
                    bit.BeginInit();
                    bit.UriSource = new Uri(item, UriKind.Relative);
                    bit.CacheOption = BitmapCacheOption.OnLoad;
                    bit.EndInit();
                    img.Source = bit;
                    img.Width = 200;
                    img.Height = 200;
                    btn.Content = img;
                    btn.Height = 200;
                    btn.Width = 200;
                    img.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    img.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    stackPanel.Children.Add(btn);
                    btn.Click+=btn_Click;
                }

            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            mainImg.Source = ((sender as System.Windows.Controls.Button).Content as Image).Source;
            slider.Value = slider.Maximum/2;
            expander.Content = System.IO.Path.GetFileName(mainImg.Source.ToString());
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mainImg.Width = e.NewValue;
            mainImg.Height = e.NewValue;
        }
    }
}
