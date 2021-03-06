﻿
namespace OSAE.UI.Controls
{
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic; 

    /// <summary>
    /// Interaction logic for AddControlUserControl.xaml
    /// </summary>
    public partial class AddControlUserControl : UserControl
    {
        string currentScreen;
        private string currentUser;
        public AddControlUserControl(string screen, string user, string controlName = "")
        {
            InitializeComponent();
            currentScreen = screen;
            currentUser = user;
            LoadUserControls();
        }

        private void LoadUserControls()
        {
            //Call the find User Controls routine, to search in our User Controls Folder
            foreach (Types.AvailablePlugin pluginOn in GlobalUserControls.OSAEUserControls.AvailablePlugins)
            {
                typesComboBox.Items.Add(pluginOn.Instance.Name);
            }
        }

        private void typesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selPlug = (ComboBoxItem)e.AddedItems[0];
            string plname = selPlug.Content.ToString();
            PluginName.Content = plname;
        }


        private void typesComboBox_DropDownClosed(object sender, System.EventArgs e)
        {
            if (typesComboBox.Text != "")
            {
                if (CntrlPnl.Children.Count > 0)
                {
                    CntrlPnl.Children.RemoveAt(0);
                }
                Types.AvailablePlugin selectedPlugin = GlobalUserControls.OSAEUserControls.AvailablePlugins.Find(typesComboBox.Text.ToString());
                PluginName.Content = selectedPlugin.Instance.Name;
                PluginDesc.Content = selectedPlugin.Instance.Description;
                PluginVersion.Content = selectedPlugin.Instance.Version;
                PluginAuthor.Content = selectedPlugin.Instance.Author;
                int lastslash = selectedPlugin.AssemblyPath.ToString().LastIndexOf(@"\");
                string imgURI = selectedPlugin.AssemblyPath.ToString().Substring(0, lastslash + 1);
                imgURI = imgURI + "ScreenShot.jpg";
                System.Windows.Media.Imaging.BitmapImage pluginImg = new System.Windows.Media.Imaging.BitmapImage();
                pluginImg.BeginInit();
                pluginImg.UriSource = new System.Uri(imgURI);
                pluginImg.EndInit();
                PluginImg.Source = pluginImg;
                selectedPlugin.Instance.InitializeAddCtrl(currentScreen,selectedPlugin.Instance.Name,currentUser);
                UserControl addcontrol = selectedPlugin.Instance.CtrlInterface;
                if (addcontrol.Height > CntrlPnl.Height)
                {
                    double cH = addcontrol.Height - CntrlPnl.Height;
                    this.Height = this.Height + cH + 80;
                    CntrlPnl.Height = addcontrol.Height + 80;
                }
                CntrlPnl.Children.Add(addcontrol);
            }
        }

        /// <summary>
        /// Let the hosting contol know that we are done
        /// </summary>
        /// <remarks>At present it tells the parent to close, this could later be altered to have a event that fires to
        /// the parent allowing them to decide what to do when the control is finished. If the control is being hosted in
        /// an element host this will have no affect as the parent is the element host and not the form.</remarks>
        private void NotifyParentFinished()
        {
            // Get the window hosting us so we can ask it to close
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }
}
