using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UniversalWindows___CreateHTMLPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            WebView.ScriptNotify += WebView_ScriptNotify;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            float[] a = {1, 2, 3, 4,5};
            float[] b = { 1, 2, 3, 4 ,6.5F};
            
            GeneratejSFile(a, b);
            
            string str = "ms-appx-web:///Assets/homepage.html";
            WebView.Source = new Uri(str);
        }

        private async void GeneratejSFile(float[] x, float[] y)
        {
            /*
             * LINK FOR REFERENCE 
             * https://stackoverflow.com/questions/35936725/write-into-file-from-assets-folder-uwp/35937233
             * 
             */
            try
            {
                //0. Generate String for x and y 
                string X = FloatArrayToString(x);
                string Y = FloatArrayToString(y);

                //1. Create String
                StringBuilder JsBuilder = new StringBuilder();
                JsBuilder.Append("var trace1 = { x: [" + X + "],y: [" + Y + "],type: 'scatter'};");
                JsBuilder.Append("var data = [trace1];");
                JsBuilder.Append("var layout = {title: 'Graph Title'};");
                JsBuilder.Append("Plotly.newPlot(graphDiv, data, layout);");

                //2. Create JS File in Local Folder
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await storageFolder.CreateFileAsync("plotlyScript.js", CreationCollisionOption.ReplaceExisting);

                //3. Write JS File in Local Folder
                await FileIO.WriteTextAsync(storageFile, JsBuilder.ToString());

                //4. Retrieve Asset Folder
                StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFolder assetsFolder = await appInstalledFolder.GetFolderAsync("Assets");

                //5. Move File from Local Folder to asset folder
                await storageFile.MoveAsync(assetsFolder, "plotlyScript.js", NameCollisionOption.ReplaceExisting);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private string FloatArrayToString(float[] a)
        {
            string output=null;
            foreach(float temp in a)
            {
                output += temp.ToString();
                output += ",";
            }
            return output;
        }

        private void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ScriptNotifyValue: " + e.Value);
            //I want to do the magic here, but this will never be called
        }
    }
}
