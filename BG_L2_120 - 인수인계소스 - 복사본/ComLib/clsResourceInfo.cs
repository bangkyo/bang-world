using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLib
{
    public class clsResourceInfo
    {
        private string PathName;
        private string ResourceName;
        Assembly MyAssembly;
        ResourceManager MyResourceManager;

        public clsResourceInfo(namespaceEnum namespaceEnumName)
        {
            string ApplicationPath = Application.StartupPath; 
            clsConstants clsConstants = new clsConstants();
            if (namespaceEnumName == namespaceEnum.ComLib)
            {
                PathName = ApplicationPath + "\\" + clsConstants.ComLib;
                ResourceName = clsConstants.ComLibResourceName;
            }
            else if (namespaceEnumName == namespaceEnum.BISYS)
            {
                PathName = ApplicationPath + "\\" + clsConstants.BISYS;
                ResourceName = clsConstants.BISYSResourceName;
            }
            else if (namespaceEnumName == namespaceEnum.SystemControlClassLibrary)
            {
                PathName = ApplicationPath + "\\" + clsConstants.SystemControlClassLibrary;
                ResourceName = clsConstants.SystemControlClassLibraryResourceName;
            }

            MyAssembly = Assembly.LoadFile(PathName);
            MyResourceManager = new ResourceManager(ResourceName, MyAssembly);
        }

        public clsResourceInfo(namespaceEnum namespaceEnumName, string ApplicationPath)
        {
            clsConstants clsConstants = new clsConstants();
            if (namespaceEnumName == namespaceEnum.ComLib)
            {
                PathName = ApplicationPath + "\\" + clsConstants.ComLib;
                ResourceName = clsConstants.ComLibResourceName;
            }
            else if(namespaceEnumName == namespaceEnum.BISYS)
            {
                PathName = ApplicationPath + "\\" + clsConstants.SystemControlClassLibrary;
                ResourceName = clsConstants.SystemControlClassLibraryResourceName;
            }
            else if(namespaceEnumName == namespaceEnum.SystemControlClassLibrary)
            {
                PathName = ApplicationPath + "\\" + clsConstants.SystemControlClassLibrary;
                ResourceName = clsConstants.SystemControlClassLibraryResourceName;
            }

            MyAssembly = Assembly.LoadFile(PathName);
            MyResourceManager = new ResourceManager(ResourceName, MyAssembly);

        }

        public clsResourceInfo(string pResourceName, string pPathFileName)
        {
            PathName = pPathFileName;
            ResourceName = pResourceName;
            MyAssembly = Assembly.LoadFile(PathName);
            MyResourceManager = new ResourceManager(ResourceName, MyAssembly);
        }

        #region [Method] ResManager GetIcon : Icon Select
        /// <summary>
        /// Icon Select
        /// </summary>
        /// <param name="strImage">Image Name</param>
        /// <returns>Image</returns>
        ///
        public Icon GetIcon(string strImage)
        {
            Icon imgIcon = null;
            try
            {
                imgIcon = (Icon)MyResourceManager.GetObject(strImage);
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, MethodBase.GetCurrentMethod().Name, clsMsg.Log.__Line(), "이미지 불러오는 중 에러");
                //imgIcon = (Icon)Properties.Resources.ResourceManager.GetObject("Toolbar_None");
            }
            return imgIcon;
        }
        #endregion

        #region [Method] ResManager GetImage : Image Select
        /// <summary>
        /// Image Select
        /// </summary>
        /// <param name="strImage">Image Name</param>
        /// <returns>Image</returns>
        public Image GetIconToImage(string strImage)
        {
            Icon tempIcon = null;   //비트맵으로 전환 하기 위한 Temp icon
            Image imgIcon = null;

            try
            {
                //Assembly MyAssembly = Assembly.LoadFile(Application.StartupPath + "\\" + "SystemControlClassLibrary.dll");
                //ResourceManager ResourceManager = new ResourceManager("SystemControlClassLibrary.Properties.Resources", MyAssembly);
                tempIcon = (Icon)MyResourceManager.GetObject(strImage);
                imgIcon = (Image)(tempIcon.ToBitmap());
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, MethodBase.GetCurrentMethod().Name, clsMsg.Log.__Line(), "이미지 불러오는 중 에러");
                //tempIcon = (Icon)Properties.Resources.ResourceManager.GetObject("Image_None");
                //imgIcon = (Image)(tempIcon.ToBitmap());
            }

            return imgIcon;
        }
        #endregion

        #region [Method] Date GetImage : Image Select
        /// <summary>
        /// Image Select
        /// </summary>
        /// <param name="strImage">Image Name</param>
        /// <returns>Image</returns>
        public Image GetImage(string strImage)
        {
            Image imgIcon = null;

            try
            {
                //Assembly MyAssembly = Assembly.LoadFile(Application.StartupPath + "\\" + "SystemControlClassLibrary.dll");
                //ResourceManager ResourceManager = new ResourceManager("SystemControlClassLibrary.Properties.Resources", MyAssembly);
                imgIcon = (Image)MyResourceManager.GetObject(strImage);
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, MethodBase.GetCurrentMethod().Name, clsMsg.Log.__Line(), "이미지 불러오는 중 에러");
                //imgIcon = (Image)Properties.Resources.ResourceManager.GetObject("NONE");
                //SqlError.SystemError("ResourceInfo", ex);
            }
            return imgIcon;
        }
        #endregion
    }
}
