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
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


namespace EasyCopy
{
    public delegate void ChangeContent();
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public event ChangeContent ChangeContentEvent;


        public Window1()
        {
            
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            List<string> _tmp_lst = new List<string>();
            _tmp_lst.Add(this.name.Text);
            _tmp_lst.Add(this.contentBox.Text);
            _tmp_lst.Add((this.name.Text.Replace(" ", "")).Replace(".", ""));


            StreamReader reader = File.OpenText("content.json");
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObj = (JObject)JToken.ReadFrom(jsonTextReader);

            jsonObj[this.name.Text] = string.Join(",",_tmp_lst.ToArray());
            reader.Close();
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText("content.json", output);
            this.Close();
 
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            StrikeEvent();
        }

        private void StrikeEvent()
        {
            if (ChangeContentEvent != null)
            {
                ChangeContentEvent();
            }
        }
    }
}
