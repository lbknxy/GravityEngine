﻿using System;
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
using GEditor.Model.Outliner;
using GEditor;
using System.Runtime.InteropServices;

namespace GEditor.View
{
    /// <summary>
    /// Interaction logic for Outliner.xaml
    /// </summary>
    public partial class Outliner : UserControl
    {
        Dictionary<string, BitmapImage> outlinerIcons = new Dictionary<string, BitmapImage>();

        IGCore.VoidFuncPointerType mSetSceneObjectsCallback;

        MainWindow mainWindow;

        public Outliner()
        {
            InitializeComponent();

            LoadImages();

            mSetSceneObjectsCallback = new IGCore.VoidFuncPointerType(SetSceneObjects);

            IGCore.SetSetSceneObjectsCallback(mSetSceneObjectsCallback);
        }

        void LoadImages()
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            img.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\GEditor\\Resource\\Image\\icon_outliner_scene.png", UriKind.Absolute);
            img.EndInit();
            outlinerIcons.Add("Scene", img);
            img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            img.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\GEditor\\Resource\\Image\\icon_outliner_object.png", UriKind.Absolute);
            img.EndInit();
            outlinerIcons.Add("Mesh", img);
        }

        public void SetSceneObjects()
        {
            outlinerTreeView.Items.Clear();

            List<OutlinerItemModel> root = new List<OutlinerItemModel>();

            OutlinerItemModel scene = new OutlinerItemModel();
            scene.Name = "Test_Scene";
            scene.Icon = outlinerIcons["Scene"];
            scene.Children = GetAllSceneObjectsInScene();
            scene.ObjectType = "Scene";

            root.Add(scene);

            outlinerTreeView.ItemsSource = root;
        }

        List<OutlinerItemModel> GetAllSceneObjectsInScene()
        {
            List<OutlinerItemModel> sObjects = new List<OutlinerItemModel>();

            int numObj = IGCore.GetSceneObjectNum();
            for (int i = 0; i < numObj; i++)
            {
                IntPtr pName = IGCore.GetSceneObjectName(i);
                string objName = Marshal.PtrToStringUni(pName);
                //string objName = Marshal.PtrToStringAnsi(pName);
                OutlinerItemModel obj = new OutlinerItemModel();
                obj.Icon = outlinerIcons["Mesh"];
                obj.Name = objName;
                obj.Children = new List<OutlinerItemModel>();
                obj.ObjectType = "Mesh";
                sObjects.Add(obj);
            }

            return sObjects;
        }

        void item_Expanded(object sender, RoutedEventArgs e)
        {
            ;
        }

        void item_Selected(object sender, RoutedEventArgs e)
        {
            OutlinerItemModel selected = (OutlinerItemModel)outlinerTreeView.SelectedItem;
            if (selected.ObjectType == "Mesh")
            {
                mainWindow.GetSceneObjectProperties(selected.Name);
            }
        }

        public void SetMainWindow(MainWindow mwRef)
        {
            mainWindow = mwRef;
        }
    }
}
