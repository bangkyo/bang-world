using System.Data;

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

        static string line_gp = "#1";

        static string biz_gp, err_yn, oper_gp, use_yn, ref_mode, diff_yn, op_biz_gp;   // PHQMS 추가

        //임시사용
        static string strKey1;
        static string strKey2;
        static string strKey3;
        static string strKey4;
        static string strKey5;
        static string strKey6;
        static string strKey7;
        static string strKey8;
        static string strKey9;

        static DataTable temptable;

        //사용자ID
        public string UserID { get { return userID; } set { userID = value; } }


        //사용자명
        public string UserNm { get { return userNm; } set { userNm = value; } }

        //사용자그룹
        public string UserGrp { get { return userGrp; } set { userGrp = value; } }

        //초기화면
        public string DefMenuID { get { return defMenuID; } set { defMenuID = value; } }

        public string StrKey1 { get { return strKey1; } set { strKey1 = value; } }

        public string StrKey2 { get { return strKey2; } set { strKey2 = value; } }

        public DataTable Temptable { get { return temptable; } set { temptable = value; } }

        //line_Selected_Index
        public int line_Selected_Index { get { return line_Selected_Index; } set { line_Selected_Index = value; } }

        //날짜
        public string setDateTime { get { return setDateTime; } set { setDateTime = value; } }

        public string Line_gp { get { return line_gp; } set { line_gp = value; } }

        public string Biz_gp { get { return biz_gp; } set { biz_gp = value; } } // PHQMS 추가
        public string Err_yn { get { return err_yn; } set { err_yn = value; } } // PHQMS 추가
        public string Oper_gp { get { return oper_gp; } set { oper_gp = value; } } // PHQMS 추가
        public string Use_yn { get { return use_yn; } set { use_yn = value; } } // PHQMS 추가
        public string Ref_mode { get { return ref_mode; } set { ref_mode = value; } } // PHQMS 추가
        public string Diff_yn { get { return diff_yn; } set { diff_yn = value; } } // PHQMS 추가
        public string Op_Biz_gp { get { return op_biz_gp; } set { op_biz_gp = value; } } // PHQMS 추가


    }
}
