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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyCopy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        Dictionary<string, string> contentDict = new Dictionary<string, string>();

        public MainWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void newPage_Click(object sender, RoutedEventArgs e)
        {
            Window1 w1 = new Window1();
            w1.ChangeContentEvent += new ChangeContent(showContent);
            w1.ShowDialog();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // 读取文件，如果没有则创建
            ReadJsonFile();
            this.showList.Children.Clear();
            showContent();


        }

        public void showContent()
        {

            ReadJsonFile();

            foreach(var item in contentDict)
            {
                Button b = new Button();

                List<string> contenLst = new List<string>(contentDict[item.Key].Split(','));

                b.Content = contenLst[0];
                b.Width = 243;
                b.Height = 33;

                b.Click += (sender,e)=>btnClickToclip(contenLst[0]);
                
                //this.showList.RegisterName(item.Key, b);
                try
                {
                    this.showList.Children.Add(b);

                    this.showList.RegisterName(contenLst[2], b);

                }
                catch 
                {

                    this.showList.Children.Remove(b);

                    Console.WriteLine("注册名重复，忽略！");
                }
                
            }
            
        }

        public void btnClickToclip(string name)
        {
            ReadJsonFile();
            List<string> infoL = new List<string>(contentDict[name].Split(','));

            Clipboard.SetText(infoL[1]);
        }
        public void ReadJsonFile()
        {
            try
            {
                StreamReader reader = File.OpenText("content.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObj = (JObject)JToken.ReadFrom(jsonTextReader);
                string json_str = JsonConvert.SerializeObject(jsonObj);
                contentDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json_str);
                reader.Close();

            }
            catch
            {
                File.CreateText("content.json").Close();
                File.WriteAllText("content.json", "{}");

            }
        }



        
    }
}
