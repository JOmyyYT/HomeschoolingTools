
using System.Net;
using System.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Documents;
using System.ComponentModel;

namespace schulbistum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            
            InitializeComponent();
            Title = "Homeschooling Tools";
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/schulbistum");
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/schulbistum" + "/notes.rtf"))
            {
                FileStream file = File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/schulbistum" + "/notes.rtf");
                file.Close();
            }
            else
            {
                FileStream fi = File.OpenRead(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/schulbistum" + "/notes.rtf");
                StreamReader reader = new StreamReader(fi);
                
                
                RtB.Document.Blocks.Clear();
                RtB.Document.Blocks.Add(new Paragraph(new Run(reader.ReadToEnd())));
            }
                
            
            
            var cli = new WebClient();

            cli.DownloadFile("https://raw.githubusercontent.com/JOmyyYT/Klasse-9.1-Tools/main/Table.html", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/schulbistum" + "/Table.html");

            TableBrowser.Source = new UriBuilder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/schulbistum" + "/Table.html").Uri;
            
            TableBrowser.Navigating += new System.Windows.Navigation.NavigatingCancelEventHandler(CheckNavigating);
            SizeChanged += new SizeChangedEventHandler(ChangedSize);
            Closing += new CancelEventHandler(SafeNotes);
        }
        void SafeNotes(object sender, CancelEventArgs e)
        {
            FileStream fif = File.OpenWrite(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/schulbistum" + "/notes.rtf");
            StreamWriter writer = new StreamWriter(fif);
            writer.Write(new TextRange(RtB.Document.ContentStart, RtB.Document.ContentEnd).Text);
            writer.Close();

        }
        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RtB.FontSize = e.NewValue;
        }

        void ChangedSize(object sender,SizeChangedEventArgs e)
        {
            RtB.Width = e.NewSize.Width;
            RtB.Height = e.NewSize.Height -95;
            Slider.HorizontalAlignment = HorizontalAlignment.Left;
            
            
                            

        }

        void CheckNavigating(object sender,System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
           
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Cancel = true;
        }
    }
}
