using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComLib
{
    
    public class clsScreen
    {
        //Login 정보
        static string userID;       //사용자ID
        static string userNm;       //사용자명
        static string userGrp;      //사용자그룹
        static string defMenuID;    //초기화면

        //사용자ID
        public string UserID { get { return userID; } set { userID = value; } }

        //사용자명
        public string UserNm { get { return userNm; } set { userNm = value; } }

        //사용자그룹
        public string UserGrp { get { return userGrp; } set { userGrp = value; } }

        //초기화면
        public string DefMenuID { get { return defMenuID; } set { defMenuID = value; } }
    }

    
}
