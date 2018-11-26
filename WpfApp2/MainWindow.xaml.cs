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
using System.Xml;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        class Item
        {
            public string title { get; set; }
            public string description { get; set; }
            public Item() { }

            public Item(string title, string description)
            {
                this.title = title;
                this.description = description;
            }
        }

        List<Item> _data = new List<Item>();

        private void btnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XmlDocument docs = new XmlDocument();
                docs.Load("https://habr.com/rss/interesting/");
                XmlNodeList xmlNodeList = docs.SelectNodes("/rss/channel/item");

                foreach (XmlNode item in xmlNodeList)
                {
                    string title = item["title"].InnerText;
                    string desc = item["description"].InnerText;
                    _data.Add(new Item(title, desc));
                }
                string source = "";
                foreach (Item item in _data)
                {
                    source += item.title + "\n";
                    source += item.description + "\n\n";
                }
                TextFromXML.Text = source;
                status.Text = "Xml-файл сгенерирован";
                status.Foreground = Brushes.Green;
            }
            catch (Exception ex)
            {
                status.Text = "Xml-файл НЕ сгенерирован";
                status.Foreground = Brushes.Red;
            }
        }
        private void btnCreateXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string xmlFile = @"xmlFile.xml";
                XmlTextWriter xmlTextWriter = new XmlTextWriter(xmlFile, Encoding.UTF8);
                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement("doc");
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Close();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFile);

                foreach (var item in _data)
                {
                    XmlNode element = xmlDocument.CreateElement("title");
                    xmlDocument.DocumentElement.AppendChild(element);
                    element.InnerText = item.title;

                    XmlNode element1 = xmlDocument.CreateElement("description");
                    xmlDocument.DocumentElement.AppendChild(element1);
                    element1.InnerText = item.description;
                }
                xmlDocument.Save(xmlFile);
                status.Text = "Xml-файл записан";
                status.Foreground = Brushes.Green;
            }
            catch (Exception ex)
            {
                status.Text = "Xml-файл НЕ записан";
                status.Foreground = Brushes.Red;
            }
        }
    }
}

