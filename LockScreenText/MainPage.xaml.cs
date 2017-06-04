using System.Net;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace LockScreenText
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private const string TileTemplateXml = @" 
 <tile branding='name'>  
   <visual version='2'>
    <binding template='TileMedium'> 
       <text hint-wrap='true'>{0}</text> 
       <text hint-style='captionsubtle' hint-wrap='true'>{1}</text> 
        <text hint-style='captionsubtle' hint-wrap='true'>{2}</text> 
     </binding> 
     <binding template='TileWide'> 
       <text id='1'>{0}</text> 
       <text id='2' hint-style='captionsubtle' hint-wrap='true'>{1}</text> 
       <text id='3' hint-style='captionsubtle' hint-wrap='true'>{2}</text> 
     </binding> 
   </visual> 
 </tile>";

        private void Button_Click(System.Object sender, RoutedEventArgs e)
        {
            string tilexml = string.Format(TileTemplateXml, textbox1.Text, textbox2.Text, textbox3.Text);
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueueForWide310x150(true);
            updater.EnableNotificationQueueForSquare150x150(true);
            updater.EnableNotificationQueueForSquare310x310(true);
            updater.EnableNotificationQueue(true);
            updater.Clear();
            var doc = new Windows.Data.Xml.Dom.XmlDocument();
            doc.LoadXml(WebUtility.HtmlDecode(tilexml), new XmlLoadSettings { ProhibitDtd = false, ValidateOnParse = false, ElementContentWhiteSpace = false, ResolveExternals = false });
            updater.Update(new TileNotification(doc));
        }
    }
}
