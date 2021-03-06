﻿using System;
using System.Data;
using System.Windows.Forms;

namespace ComLib
{
    public class clsCom
    {
        public static clsCom Com = new clsCom();
        public static clsCom GetInstance()
        {
            return Com;
        }
        // Grid Common Property

        //Login 정보
        static string userID;       //사용자ID
        static string userNm;       //사용자명
        static string userGrp;      //사용자그룹
        static string defMenuID;    //초기화면
        static string userCk;       //아이디 저장 여부 0 아님 1 저장

        static string line_gp = "#1";
        public string dllFileNm = Application.StartupPath + "\\" + "BGsystemLibrary" + ".dll";

        //임시사용
        static string strKey1;
        static string strKey2;
        static string strKey3;
        static DateTime strKeyDT1;

        //임시
        static string factory;       //공장

        static DataTable temptable;

        //공장
        public string Factory { get { return factory; } set { factory = value; } }

        //사용자ID
        public string UserID { get { return userID; } set { userID = value; } }


        //사용자명
        public string UserNm { get { return userNm; } set { userNm = value; } }

        //사용자그룹
        public string UserGrp { get { return userGrp; } set { userGrp = value; } }

        //초기화면
        public string DefMenuID { get { return defMenuID; } set { defMenuID = value; } }

        //사용자ID
        public string UserCk { get { return userCk; } set { userCk = value; } }


        public string StrKey1 { get { return strKey1; } set { strKey1 = value; } }
        public DateTime StrKeyDT1 { get { return strKeyDT1; } set { strKeyDT1 = value; } }



        public string StrKey2 { get { return strKey2; } set { strKey2 = value; } }

        public string StrKey3 { get { return strKey3; } set { strKey3 = value; } }

        public DataTable Temptable { get { return temptable; } set { temptable = value; } }

        //line_Selected_Index
        public int line_Selected_Index { get { return line_Selected_Index; } set { line_Selected_Index = value; } }

        //날짜
        public string setDateTime { get { return setDateTime; } set { setDateTime = value; } }

        public string Line_gp { get { return line_gp; } set { line_gp = value; } }



    }
}
