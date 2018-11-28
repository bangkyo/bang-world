using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ComLib.clsMgr;
using System.Data.Common;
using System.Text;

namespace ComLib
{
    public class ConnectDB
    {
        //clsStyle cs = new clsStyle();
        //public string ConnString = "Server=10.216.80.130;uid=sa;pwd=level2;DataBase= MDMS";
        //public string ConnString = "Server=10.216.122.110;uid=sa;pwd=level2@@;DataBase= BLOOM120";
        public string ConnString = "Server=192.168.250.165;uid=sa;pwd=level2;DataBase=BLOOM120";
        private bool IsDBAvailable = false;

        //계발계
        //string ConnString = "Data Source=(DESCRIPTION ="
        //                  + "(ADDRESS_LIST=(ADDRESS = (PROTOCOL = TCP)(HOST = 10.216.80.248)(PORT = 1521)))"
        //                  + "(CONNECT_DATA=(SERVER = 10.216.80.248)(SERVICE_NAME = L2PRB)));"
        //                  + "User Id=l2user_dev;Password=l2user_dev;";
        //실서버
        //string ConnString = "Data Source=(DESCRIPTION ="
        //                  + "(ADDRESS_LIST=(ADDRESS = (PROTOCOL = TCP)(HOST = 10.216.80.248)(PORT = 1521)))"
        //                  + "(CONNECT_DATA=(SERVER = 10.216.80.248)(SERVICE_NAME = L2PRB)));"
        //                 + "User Id=l2user;Password=l2user;";

        public bool CheckDbConnection()
        {
            try
            {
                using (var connection = new SqlConnection(ConnString))
                {
                    connection.Open();
                    IsDBAvailable = true;
                    return IsDBAvailable;
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(LogTopicEnum.Agent, "Error in DB connection test on CheckDBConnection", ex);
                IsDBAvailable = false;
                return IsDBAvailable; // any error is considered as db connection error for now
            }
        }
        public SqlConnection OConnect()
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConnString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("■OConnect Error■\n" + ex.Message);
            }
            finally
            {
                if (conn != null) { conn.Close(); }
            }

            return conn;
        }

        //20170628 OCJ 추가--
        //비동기로 db에 연결하기 위해 필요한 함수
        public SqlConnection OConnectAsync()
        {
            SqlConnection conn = null;
            string strConnString = ConnString + "; Asynchronous Processing = true;";

            try
            {
                conn = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("■OConnect Error■\n" + ex.Message);
            }
            finally
            {
                if (conn != null) { conn.Close(); }
            }

            return conn;
        }
        //----
        public DataTable FindDataTable(string Sql)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            try
            {
                conn.Open();
                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }


        /// <summary>
        /// 쿼리 변수 처리 변수앞에는 ' : ' 사용한다.( :userId ) 
        /// 파라메타는 string[] 으로 보낸다 중간 구분자는 ' | ' 이용한다. ( ":userId|AAAA") 
        /// </summary>
        /// <param name="Sql">쿼리문</param>
        /// <param name="parm">파라메타 배열</param>
        /// <returns></returns>
        public DataTable FindDataTable(string Sql, string[] parm)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();
            SqlParameter op = null;

            try
            {
                string[] split;

                conn.Open();
                ocmd = new SqlCommand(Sql, conn);

                for (int ii = 0; ii < parm.Length; ii++)
                {
                    if (parm[ii] != null && parm[ii] != "")
                    {
                        split = parm[ii].Split('|');

                        op = new SqlParameter(split[0], split[1]);

                        ocmd.Parameters.Add(op);
                    }
                }

                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        /// <summary>
        /// 쿼리 실행 (입력, 수정, 삭제)
        /// </summary>
        /// <param name="Sql">쿼리문</param>
        /// <returns>int (0:실패, 1이상:성공)</returns>
        public int ExecuteQuery(string Sql)
        {
            SqlConnection conn = OConnect();
            SqlCommand ocmd = null;
            int ridx = 0;

            try
            {
                conn.Open();
                ocmd = new SqlCommand(Sql, conn);
                ridx = ocmd.ExecuteNonQuery();
                //AutoCommit
                //ocmd = new SqlCommand("commit", conn);
                //ocmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("■ExecuteQuery Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (ocmd != null) { ocmd.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return ridx;
        }

        /// <summary>
        /// 다중 쿼리 실행
        /// </summary>
        /// <param name="Sql">쿼리문 배열</param>
        /// <returns>에러시 값있음</returns>
        public bool ExecuteQuery(string[] Sql)
        {
            SqlConnection conn = OConnect();
            SqlCommand ocmd = new SqlCommand();
            SqlTransaction tran = null;
            bool boolRet = false;

            try
            {
                conn.Open();
                ocmd.Connection = conn;
                tran = conn.BeginTransaction();
                ocmd.Transaction = tran;

                for (int ii = 0; ii < Sql.Length; ii++)
                {
                    if (Sql[ii] != null && Sql[ii] != "")
                    {
                        ocmd.CommandText = Sql[ii];
                        ocmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();

                boolRet = true;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }

                string msg = "■ExecuteQuery Error■\n" + ex.Message + "\n■Sql■\n";

                for (int ii = 0; ii < Sql.Length; ii++)
                {
                    msg += ii + "=[" + Sql[ii] + "]\n";
                }

                MessageBox.Show(msg);
            }
            finally
            {
                if (ocmd != null) { ocmd.Dispose(); }
                if (tran != null) { tran.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return boolRet;
        }

        /// <summary>
        /// 값이 있으면 true 이다
        /// </summary>
        /// <param name="Sql">쿼리</param>
        /// <returns>bool</returns>
        public bool IsInsUpChk(string Sql)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();
            bool boolRet = false;

            try
            {
                conn.Open();
                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];

                boolRet = (dt.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return boolRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colNm">컬럼명</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, string colNm)
        {
            string strRet = "";

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0][colNm].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Column Name : " + colNm);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colNm">컬럼명</param>
        /// <param name="nullDefault">null 이거나 없을시</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, string colNm, string nullDefault)
        {
            string strRet = nullDefault;

            try
            {
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][colNm].ToString() != "")
                {
                    strRet = dt.Rows[0][colNm].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Column Name : " + colNm);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colIndex">컬럼인덱스</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int colIndex)
        {
            string strRet = "";

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0][colIndex].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Column Index : " + colIndex);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">로우인덱스</param>
        /// <param name="colNm">컬럼명</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int rowIndex, string colNm)
        {
            string strRet = "";

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[rowIndex][colNm].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Row Index : " + rowIndex + ", Column Name : " + colNm);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">로우인덱스</param>
        /// <param name="colIndex">컬럼인덱스</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int rowIndex, int colIndex)
        {
            string strRet = "";

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[rowIndex][colIndex].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Row Index : " + rowIndex + ", Column Index : " + colIndex);
            }

            return strRet;
        }



        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">로우인덱스</param>
        /// <param name="colNm">컬럼명</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int rowIndex, string colNm, string nullDefault)
        {
            string strRet = nullDefault;

            try
            {
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[rowIndex][colNm].ToString() != "")
                {
                    strRet = dt.Rows[rowIndex][colNm].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Row Index : " + rowIndex + ", Column Name : " + colNm);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">로우인덱스</param>
        /// <param name="colIndex">컬럼인덱스</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int rowIndex, int colIndex, string nullDefault)
        {
            string strRet = nullDefault;

            try
            {
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[rowIndex][colIndex].ToString() != "")
                {
                    strRet = dt.Rows[rowIndex][colIndex].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Row Index : " + rowIndex + ", Column Index : " + colIndex);
            }

            return strRet;
        }

        public string Find_CD_NM(string category, string cd_id)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;
            string rtn = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT  CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' and CD_ID = '{1}' AND USE_YN = 'Y' ORDER BY SORT_SEQ, CD_NM", category, cd_id);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                }
                else
                {
                    rtn = "";
                }

                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return rtn;
        }

        public string Find_CD_ID(string category, string cd_nm)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;
            string rtn = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT CD_ID  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' and CD_NM = '{1}' AND USE_YN = 'Y' ORDER BY SORT_SEQ, CD_NM, CD_ID", category, cd_nm);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                }
                else
                {
                    rtn = "";
                }

                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return rtn;
        }

        public DataTable Find_CD(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT CD_ID, CD_NM  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' AND USE_YN = 'Y'  ORDER BY SORT_SEQ, CD_ID", category);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public DataTable Find_ROUTING_CD(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                string Sql1 = string.Format("SELECT CD_ID,   ");
                Sql1 += string.Format("  CD_NM    ");
                Sql1 += string.Format("FROM TB_CM_COM_CD  ");
                Sql1 += string.Format("WHERE CATEGORY = 'ROUTING_CD' ");
                Sql1 += string.Format("   AND CD_ID = 'A1'           ");
                Sql1 += string.Format("   OR CD_ID = 'B1'   ");
                Sql1 += string.Format("   OR CD_ID = 'L3'  ");
                Sql1 += string.Format("ORDER BY CD_ID   ");

                ocmd = new SqlCommand(Sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public DataTable Find_TC_CD(string category, string line_gp)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                string Sql1 = string.Format("SELECT CD_ID, ");
                Sql1 += string.Format("CD_NM    ");
                Sql1 += string.Format("FROM TB_CM_COM_CD  ");
                Sql1 += string.Format("WHERE CATEGORY = 'TC_CD' AND USE_YN = 'Y'");
                if (line_gp != "%")
                {
                    Sql1 += string.Format("AND COLUMNA = '{0}' ", line_gp);
                }
                Sql1 += string.Format("ORDER BY SORT_SEQ  ");

                ocmd = new SqlCommand(Sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable Find_CD_GOOD_NG(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT CD_ID, CD_NM  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}'  ORDER BY SORT_SEQ, CD_ID", category);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable GetScreeAcl(string UserGrp)
        {
            DataTable dt = null;

            string Sql1 = string.Empty;

            Sql1 += string.Format("SELECT ");
            Sql1 += string.Format("    B.PAGE_ID ");
            Sql1 += string.Format("   ,NVL(A.INQ_ACL,'N') AS INQ_ACL ");
            Sql1 += string.Format("   ,NVL(A.REG_ACL,'N') AS REG_ACL ");
            Sql1 += string.Format("   ,NVL(A.MOD_ACL,'N') AS MOD_ACL ");
            Sql1 += string.Format("   ,NVL(A.DEL_ACL,'N') AS DEL_ACL ");
            Sql1 += string.Format("FROM TB_CM_ACL A, TB_CM_SCR B ");
            Sql1 += string.Format("WHERE ");
            Sql1 += string.Format("    A.ACL_GRP_ID = '{0}'", UserGrp);
            Sql1 += string.Format("AND A.SCR_ID = B.SCR_ID ");
            Sql1 += string.Format("AND B.USE_YN = 'Y' ");

            dt = FindDataTable(Sql1);

            return dt;
        }

        public string GetDBDate(string gb)
        {
            DataTable dt = null;
            string rda = "";
            string Sql1 = string.Empty;

            //Sql1 = string.Format("select SYSDATETIME() as da");
            Sql1 = string.Format("select convert(varchar, getdate(), 120) AS da");

            dt = FindDataTable(Sql1);
            rda = dt.Rows[0]["da"].ToString();
            //rda = string.Format("yyyy/MM/dd HH:mm:ss.fff", rda);

            return rda;
        }

        public bool SetCombo(ComboBox cb, string categoryNm)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                List<string> list = new List<string>();
                //list.Add("전체");

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    list.Add(dt.Rows[row].ItemArray[1].ToString());
                }
                foreach (var item in list)
                {
                    cb.Items.Add(item);
                }
                if (dt.Rows.Count > 0)
                {
                    //첫번째 아이템 선택
                    cb.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// combobox setup
        /// </summary>
        /// <param name="cb"> combobox object</param>
        /// <param name="categoryNm"> category key string</param>
        /// <param name="ExtractItemNM"> 제거해야할 항목</param>
        /// <param name="AddTotal">전체를 추가할것인지</param>
        /// <returns></returns>
        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        //ConnectDB cd = new ConnectDB();
        public void SetCombo(ComboBox cb, string _sql, string selected_id, bool isTotalAdd, int cd_id, int cd_nm)
        {

            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = FindDataTable(_sql);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row[cd_nm].ToString(), row[cd_id].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.SelectedIndex = 0;

                    if (!string.IsNullOrEmpty(selected_id))
                    {
                        foreach (var item in cb.Items)
                        {
                            if (((DictionaryList)item).fnValue == selected_id)
                            {
                                cb.SelectedIndex = cb.Items.IndexOf(item);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }

        public void SetCombo(ComboBox cb, string _sql, string selected_id, bool isTotalAdd, int cd_id, int cd_nm, int cd_value1)
        {

            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = FindDataTable(_sql);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row[cd_nm].ToString(), row[cd_id].ToString(), row[cd_value1].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.SelectedIndex = 0;

                    if (!string.IsNullOrEmpty(selected_id))
                    {
                        foreach (var item in cb.Items)
                        {
                            if (((DictionaryList)item).fnValue == selected_id)
                            {
                                cb.SelectedIndex = cb.Items.IndexOf(item);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }

        public void SetCombo(ComboBox cb, string _sql, string selected_id, bool isTotalAdd, int cd_id, int cd_nm, int cd_value1, int cd_value2)
        {

            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = FindDataTable(_sql);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row[cd_nm].ToString(), row[cd_id].ToString(), row[cd_value1].ToString(), row[cd_value2].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.SelectedIndex = 0;

                    if (!string.IsNullOrEmpty(selected_id))
                    {
                        foreach (var item in cb.Items)
                        {
                            if (((DictionaryList)item).fnValue == selected_id)
                            {
                                cb.SelectedIndex = cb.Items.IndexOf(item);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }

        public void SetCombo(ComboBox cb, string _sql, string selected_id, bool isTotalAdd, int cd_id, int cd_nm, int cd_value1, int cd_value2, int cd_value3)
        {

            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = FindDataTable(_sql);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row[cd_nm].ToString(), row[cd_id].ToString(), row[cd_value1].ToString(), row[cd_value2].ToString(), row[cd_value3].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.SelectedIndex = 0;

                    if (!string.IsNullOrEmpty(selected_id))
                    {
                        foreach (var item in cb.Items)
                        {
                            if (((DictionaryList)item).fnValue == selected_id)
                            {
                                cb.SelectedIndex = cb.Items.IndexOf(item);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }
        public void SetCombo(ComboBox cb, string _sql, string selected_id, bool isTotalAdd, int cd_id, int cd_nm, int cd_value1, int cd_value2, int cd_value3, int cd_value4)
        {

            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = FindDataTable(_sql);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row[cd_nm].ToString(), row[cd_id].ToString(), row[cd_value1].ToString(), row[cd_value2].ToString(), row[cd_value3].ToString(), row[cd_value4].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.SelectedIndex = 0;

                    if (!string.IsNullOrEmpty(selected_id))
                    {
                        foreach (var item in cb.Items)
                        {
                            if (((DictionaryList)item).fnValue == selected_id)
                            {
                                cb.SelectedIndex = cb.Items.IndexOf(item);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }

        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SetCombo_3(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 2;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //특정 항목들을 제거하기위한 설정
        public bool SetCombo(ComboBox cb, string categoryNm, List<string> Extractlist, bool AddTotal)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                bool exists = false;
                foreach (DataRow row in dt.Rows)
                {

                    //리스트에서 해당 항목의 이름이 같은 경우가 있는지 체크

                    //string result = Extractlist.Find(s => s.StartsWith(row["CD_NM"].ToString()));
                    exists = Extractlist.Exists(element => element == row["CD_NM"].ToString());

                    // 해당 항목이 제외할 아이템리스트에 없는경우  ==> 추가해야할경우
                    if (!exists)
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }
                    // 해당 항목이  제외할 아이템리스트에 없는경우 ==> 추가해야하지 않아야할경우

                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }


        //교정, 면취의 정보만 가진 공정 cb
        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, List<string> list)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());


                        }
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            if (row["CD_NM"].ToString() == item)
                            {
                                arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());

                            }

                        }
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public bool SetCombo(ComboBox cb, string categoryNm, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                DataTable dt = Find_CD(categoryNm);


                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public void SetComboYN(ComboBox cb)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                ArrayList arrType1 = new ArrayList();

                arrType1.Add(new DictionaryList("Y", "Y"));
                arrType1.Add(new DictionaryList("N", "N"));


                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                return;
            }

            return;
        }

        public void SetComboHEATPOC(ComboBox cb)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                ArrayList arrType1 = new ArrayList();

                arrType1.Add(new DictionaryList("POC", "A"));
                arrType1.Add(new DictionaryList("HEAT", "B"));


                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                return;
            }

            return;
        }

        public bool SetCombo_TC_CD(ComboBox cb, string categoryNm, string line_gp, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                cb.DataSource = null;
                cb.Items.Clear();

                DataTable dt = Find_TC_CD(categoryNm, line_gp);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_ID"].ToString() + "  " + row["CD_NM"].ToString(), row["CD_ID"].ToString()));
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public bool SetComboTC_CD(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["COLUMNA"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public DataTable Find_STEEL_GRP(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT DISTINCT  COLUMNA FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' AND USE_YN = 'Y' ORDER BY COLUMNA ", category);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public string Find_STEEL_GRP(string category, string grp_nm)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;
            string rtn = string.Empty;

            try
            {
                conn.Open();

                Sql = string.Format("SELECT DISTINCT COLUMNA  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' and COLUMNA = '{1}' AND USE_YN = 'Y' ORDER BY COLUMNA", category, grp_nm);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                }
                else
                {
                    rtn = "";
                }

                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return rtn;
        }

        public void SetComboIDNM(ComboBox cb, string categoryNm)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();
                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                //첫번째 아이템 선택
                //cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            return;
        }

        public void SetComboZoneInfo(ComboBox cb, string _line_gp)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();
                DataTable dt = FindReWorkZoneInfo(_line_gp);

                ArrayList arrType1 = new ArrayList();
                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            return;
        }

        public void SetComboPOC(ComboBox cb, string _line_gp)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();
                DataTable dt = FindPOCInfo(_line_gp);

                ArrayList arrType1 = new ArrayList();
                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["POC_NO"].ToString(), row["POC_NO"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                if (dt.Rows.Count > 0)
                {
                    //첫번째 아이템 선택
                    cb.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
            return;
        }

        public DataTable Find_BIZ_GP()
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT DISTINCT BIZ_GP  FROM TB_CM_SCR  ORDER BY BIZ_GP");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public DataTable Find_ACL_GRP_ID()
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format(" SELECT ACL_GRP_ID,ACL_GRP_NM  FROM TB_CM_ACL_GRP ");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public string Find_Steel_NM_By_ID(string steel_id)
        {
            string rtn = "";

            string strQry = string.Empty;

            strQry += string.Format("select  ");
            strQry += string.Format("    ROWNUM AS L_NO");
            strQry += string.Format("   ,CD_NM ");
            strQry += string.Format("FROM  ");
            strQry += string.Format("    TB_CM_COM_CD  ");
            strQry += string.Format("WHERE  ");
            strQry += string.Format("    category = 'STEEL' ");
            strQry += string.Format("AND  ");
            strQry += string.Format("        CD_ID LIKE '{0}'  ", steel_id);
            strQry += string.Format("AND ROWNUM = 1      ");

            DataTable olddt = FindDataTable(strQry);

            if (olddt.Rows.Count > 0)
            {
                rtn = olddt.Rows[0]["CD_NM"].ToString();
            }

            return rtn;
        }

        public string Find_Scr_ID_By_PAGE_ID(string page_id)
        {
            string rtn = "";

            try
            {
                string strQry = string.Empty;

                //strQry += string.Format("select  ");
                //strQry += string.Format("    ROWNUM AS L_NO");
                //strQry += string.Format("   ,SCR_ID ");
                //strQry += string.Format("FROM  ");
                //strQry += string.Format("    TB_CM_SCR  ");
                //strQry += string.Format("WHERE  ");
                //strQry += string.Format("    PAGE_ID = '{0}' ", page_id);
                //strQry += string.Format("AND ROWNUM = 1      ");

                strQry += string.Format("select  ");
                strQry += string.Format("top 1 SCR_ID ");
                strQry += string.Format("FROM  ");
                strQry += string.Format("TB_CM_SCR  ");
                strQry += string.Format("WHERE  ");
                strQry += string.Format("PAGE_ID = '{0}' ", page_id);
                //strQry += string.Format("AND ROWNUM = 1      ");

                DataTable olddt = FindDataTable(strQry);

                if (olddt.Rows.Count > 0)
                {
                    rtn = olddt.Rows[0]["SCR_ID"].ToString();
                }
            }
            catch (Exception ex)
            {
                
            }

            

            return rtn;
        }

        public DataTable FindReWorkZoneInfo(string _line_gp)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql1 = string.Empty;

            try
            {
                conn.Open();
                Sql1 += string.Format("SELECT ZONE_CD ");
                Sql1 += string.Format("FROM TB_CR_ZONEINFO ");
                Sql1 += string.Format("WHERE REENT_PSB_YN = 'Y' ");
                Sql1 += string.Format("and   LINE_GP  = '{0}' ", _line_gp);

                ocmd = new SqlCommand(Sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql1);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }


        public string GetPOCSql()
        {
            // --실시간트레킹중간_POC정보
            string Sql1 = string.Empty;
            Sql1 += string.Format("SELECT NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL),STEEL) AS STEEL_NM ");
            Sql1 += string.Format("      ,A.HEAT ");
            Sql1 += string.Format("      ,ITEM_SIZE ");
            Sql1 += string.Format("      ,LENGTH ");
            Sql1 += string.Format("      ,SURFACE_LEVEL ");
            Sql1 += string.Format("      ,POC_NO ");
            Sql1 += string.Format("      ,(SELECT SUM(PCS) FROM TB_CR_ORD_BUNDLEINFO WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ) AS MILL_PCS   "); //--압연본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO     = A.POC_NO AND POC_SEQ = A.POC_SEQ ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'A1' "); // 교정
            Sql1 += string.Format("        AND   LINE_GP    = :P_LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        ) AS STR_PCS   "); //--투입본수
            Sql1 += string.Format("      ,(SELECT NVL(SUM(PCS), 0) FROM TB_BND_WR ");
            Sql1 += string.Format("        WHERE POC_NO           = A.POC_NO AND POC_SEQ = A.POC_SEQ ");
            Sql1 += string.Format("        AND   NVL(DEL_YN,'N') <> 'Y' ");
            Sql1 += string.Format("        AND   LINE_GP          = A.LINE_GP) AS OK_PCS   ");  //--합격본수, 바인딩본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO     = A.POC_NO AND POC_SEQ = A.POC_SEQ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'W2' ");
            Sql1 += string.Format("        AND   LINE_GP    = A.LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        ) AS NG_PCS    ");  //--불합격본수
            Sql1 += string.Format("FROM    ( ");
            Sql1 += string.Format("         SELECT POC_NO ");
            Sql1 += string.Format("               ,POC_SEQ ");
            Sql1 += string.Format("               ,STEEL ");
            Sql1 += string.Format("               ,HEAT ");
            Sql1 += string.Format("               ,ITEM_SIZE ");
            Sql1 += string.Format("               ,LENGTH ");
            Sql1 += string.Format("               ,SURFACE_LEVEL ");
            Sql1 += string.Format("               ,LINE_GP ");
            Sql1 += string.Format("         FROM   TB_CR_INPUT_ORD ");
            Sql1 += string.Format("         WHERE  (POC_NO, POC_SEQ) IN ( ");
            Sql1 += string.Format("                                       SELECT /*+RULE */ DISTINCT POC_NO, POC_SEQ ");
            Sql1 += string.Format("                                       FROM   TB_RL_TM_TRACKING ");
            Sql1 += string.Format("                                       WHERE  PROG_STAT   IN ('RUN','WAT', 'FIN') ");
            Sql1 += string.Format("                                       AND    LINE_GP     = :P_LINE_GP   ");
            Sql1 += string.Format("                                       AND    ZONE_CD  not in ( '3Z99','3Z39')   ");
            Sql1 += string.Format("                                       AND    ROWNUM      = 1   ");
            Sql1 += string.Format("                                      ) ");
            Sql1 += string.Format("        ) A ");

            return Sql1;
        }

        public string GetPOCSql_12()
        {
            // --실시간트레킹중간_POC정보
            string Sql1 = string.Empty;
            Sql1 += string.Format("SELECT NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL),STEEL) AS STEEL_NM ");
            Sql1 += string.Format("      ,A.HEAT ");
            Sql1 += string.Format("      ,ITEM_SIZE ");
            Sql1 += string.Format("      ,LENGTH ");
            Sql1 += string.Format("      ,SURFACE_LEVEL ");
            Sql1 += string.Format("      ,POC_NO ");
            Sql1 += string.Format("      ,(SELECT SUM(PCS) FROM TB_CR_ORD_BUNDLEINFO WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ) AS MILL_PCS   "); //--압연본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO     = A.POC_NO AND POC_SEQ = A.POC_SEQ ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'A1' "); // 교정
            Sql1 += string.Format("        AND   LINE_GP    = :P_LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        ) AS STR_PCS   "); //--투입본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO           = A.POC_NO AND POC_SEQ = A.POC_SEQ ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'F2'");
            Sql1 += string.Format("        AND   LINE_GP          = A.LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        AND   GOOD_YN    = 'OK'");
            Sql1 += string.Format("        ) AS OK_PCS   "); //--합격본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO     = A.POC_NO AND POC_SEQ = A.POC_SEQ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'F2' ");
            Sql1 += string.Format("        AND   LINE_GP    = A.LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        AND   GOOD_YN = 'NG' ");
            Sql1 += string.Format("        ) AS NG_PCS    ");  //--불합격본수
            Sql1 += string.Format("FROM    ( ");
            Sql1 += string.Format("         SELECT POC_NO ");
            Sql1 += string.Format("               ,POC_SEQ ");
            Sql1 += string.Format("               ,STEEL ");
            Sql1 += string.Format("               ,HEAT ");
            Sql1 += string.Format("               ,ITEM_SIZE ");
            Sql1 += string.Format("               ,LENGTH ");
            Sql1 += string.Format("               ,SURFACE_LEVEL ");
            Sql1 += string.Format("               ,LINE_GP ");
            Sql1 += string.Format("         FROM   TB_CR_INPUT_ORD ");
            Sql1 += string.Format("         WHERE  (POC_NO, POC_SEQ) IN ( ");
            Sql1 += string.Format("                                       SELECT /*+RULE */ DISTINCT POC_NO, POC_SEQ ");
            Sql1 += string.Format("                                       FROM   TB_RL_TM_TRACKING ");
            Sql1 += string.Format("                                       WHERE  PROG_STAT   IN ('RUN','WAT', 'FIN') ");
            Sql1 += string.Format("                                       AND    LINE_GP     = :P_LINE_GP   ");
            Sql1 += string.Format("                                       AND    ZONE_CD  not in ( '1Z41','2Z43')   ");
            Sql1 += string.Format("                                       AND    ROWNUM      = 1   ");
            Sql1 += string.Format("                                      ) ");
            Sql1 += string.Format("        ) A ");

            return Sql1;
        }
        public string GetReadyBundleSql()
        {
            string Sql1 = string.Empty;
            Sql1 += string.Format("SELECT B.MILL_NO ");
            Sql1 += string.Format("      ,A.PCS    AS MILL_PCS ");
            Sql1 += string.Format("      ,C.WR_PCS AS STR_PCS ");
            Sql1 += string.Format("FROM   TB_CR_INPUT_WR A ");
            Sql1 += string.Format("      ,(SELECT DISTINCT MILL_NO AS MILL_NO ");
            Sql1 += string.Format("        FROM   TB_RL_TM_TRACKING ");
            Sql1 += string.Format("        WHERE  PROG_STAT   IN ('RUN','WAT') ");
            Sql1 += string.Format("        AND    LINE_GP     =  :P_LINE_GP ");
            Sql1 += string.Format("        AND    ZONE_CD     LIKE '_Z01') B ");
            Sql1 += string.Format("      ,(SELECT  MILL_NO, COUNT(*) AS WR_PCS ");
            Sql1 += string.Format("        FROM   TB_CR_PIECE_WR P ");
            Sql1 += string.Format("        WHERE  LINE_GP     = :P_LINE_GP ");
            Sql1 += string.Format("        AND    ROUTING_CD  = 'A1' ");
            Sql1 += string.Format("        AND    REWORK_SEQ  = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                              WHERE  MILL_NO    = P.MILL_NO ");
            Sql1 += string.Format("                              AND    PIECE_NO   = P.PIECE_NO ");
            Sql1 += string.Format("                              AND    LINE_GP    = P.LINE_GP ");
            Sql1 += string.Format("                              AND    ROUTING_CD = P.ROUTING_CD) ");
            Sql1 += string.Format("        GROUP BY MILL_NO ) C ");
            Sql1 += string.Format("WHERE  B.MILL_NO  = A.MILL_NO ");
            Sql1 += string.Format("AND    B.MILL_NO  = C.MILL_NO(+) ");
            Sql1 += string.Format("ORDER BY A.REG_DDTT, MILL_NO ");

            return Sql1;
        }

        public string GetZoneDataSql()
        {
            string Sql1 = string.Empty;
            //Sql1 += string.Format("SELECT ZONE_CD ");
            //Sql1 += string.Format("      ,MIN(MILL_NO) AS MILL_NO ");
            //Sql1 += string.Format("      ,COUNT(*)     AS PCS ");
            //Sql1 += string.Format("      ,MAX(BUNDLE_NO) AS BUNDLE_NO ");
            //Sql1 += string.Format("FROM   TB_RL_TM_TRACKING A ");
            //Sql1 += string.Format("WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            //Sql1 += string.Format("AND    NOT EXISTS (SELECT POC_NO FROM TB_CR_ORD WHERE POC_NO = A.POC_NO AND POC_PROG_STAT = 'D' )   ");
            //Sql1 += string.Format("AND    LINE_GP     =  :P_LINE_GP ");
            //Sql1 += string.Format("GROUP BY ZONE_CD ");

            Sql1 += string.Format("SELECT /*+RULE */ ZONE_CD ");
            Sql1 += string.Format("      ,MIN(MILL_NO) AS MILL_NO ");
            Sql1 += string.Format("      ,COUNT(*)     AS PCS ");
            Sql1 += string.Format("      ,MAX(BUNDLE_NO) AS BUNDLE_NO ");
            Sql1 += string.Format("      ,MAX(BUNDLE_PCS) AS BUNDLE_PCS ");
            Sql1 += string.Format("FROM   TB_RL_TM_TRACKING A ");
            Sql1 += string.Format("      ,( SELECT POC_NO, COUNT(*) AS BUNDLE_PCS FROM TB_BND_WR ");
            Sql1 += string.Format("         WHERE  POC_NO  = ( SELECT /*+RULE */ MAX(POC_NO) FROM TB_RL_TM_TRACKING ");
            Sql1 += string.Format("                            WHERE  PROG_STAT   IN ('RUN','FIN') ");
            Sql1 += string.Format("                            AND    ZONE_CD   <>  '3Z99' ");
            Sql1 += string.Format("                            AND    LINE_GP     =  :P_LINE_GP ) ");
            Sql1 += string.Format("         AND   NVL(DEL_YN,'N') <> 'Y' ");
            Sql1 += string.Format("          GROUP BY POC_NO ) B ");
            Sql1 += string.Format("WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            Sql1 += string.Format("AND    A.POC_NO    = B.POC_NO(+) ");
            Sql1 += string.Format("AND    LINE_GP     =  :P_LINE_GP ");
            Sql1 += string.Format("GROUP BY ZONE_CD ");

            return Sql1;
        }

        /// <summary>
        /// 해당 실시간 트레킹라인에서 현재 진행중인 데이터를 테이블로 가져옴
        /// </summary>
        /// <param name="line_gp"></param>
        /// <returns></returns>
        public DataTable GetRTDATA(string line_gp)
        {
            DataTable dt = new DataTable();

            try
            {
                string Sql1 = string.Empty;
                Sql1 += string.Format("SELECT MILL_NO ");
                Sql1 += string.Format("      ,PIECE_NO ");
                Sql1 += string.Format("      ,POC_NO ");
                Sql1 += string.Format("      ,PROG_STAT ");
                Sql1 += string.Format("FROM TB_RL_TM_TRACKING ");
                Sql1 += string.Format("WHERE LINE_GP = '{0}' ", line_gp);
                Sql1 += string.Format("AND   PROG_STAT = 'RUN' ");


                dt = FindDataTable(Sql1);

            }
            catch (Exception)
            {

            }

            return dt;
        }

        public DataTable FindPOCInfo(string _line_gp)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql1 = string.Empty;

            try
            {
                conn.Open();
                Sql1 = GetDistinctPOCSql(_line_gp);

                ocmd = new SqlCommand(Sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql1);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public string GetDistinctPOCSql(string _line_gp)
        {
            string Sql1 = string.Empty;
            Sql1 += string.Format("SELECT DISTINCT POC_NO ");
            Sql1 += string.Format("FROM TB_PROG_POC_MGMT ");
            Sql1 += string.Format("WHERE    ");
            Sql1 += string.Format("    LINE_GP     =  '{0}'    ", _line_gp);
            //Sql1 += string.Format("ORDER BY EXIT_DDTT DESC ");

            return Sql1;
        }

        public void InsertLogForSearchForOracle(string userid, Button btn, string gp)
        {
            string user_id = userid;
            string btn_gp = "1";
            string scr_id;

            string pageid = string.IsNullOrEmpty(btn.Parent.Parent.Parent.Name) ? btn.Parent.Parent.Name : btn.Parent.Parent.Parent.Name;

            if (string.IsNullOrEmpty(Find_Scr_ID_By_PAGE_ID(pageid)))
            {
                return;
            }
            else
            {
                scr_id = Find_Scr_ID_By_PAGE_ID(pageid);
            }

            //디비선언
            SqlConnection conn = OConnect();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                //TB_CM_SCR_USEHIS -> MDMS.dbo.TB_CM_SCR_USEHIS
                string Sql1 = string.Empty;
                Sql1 += string.Format(" INSERT INTO TB_CM_SCR_USEHIS ");
                Sql1 += string.Format("      ( ");
                Sql1 += string.Format("         USER_ID ");
                Sql1 += string.Format("        ,SCR_ID ");
                Sql1 += string.Format("        ,USE_DATE_SEQ ");
                Sql1 += string.Format("        ,USE_DDTT ");
                Sql1 += string.Format("        ,BTN_GP ");
                Sql1 += string.Format("        ,REGISTER ");
                Sql1 += string.Format("        ,REG_DDTT ");
                Sql1 += string.Format("       ) ");
                Sql1 += string.Format(" VALUES ( ");
                Sql1 += string.Format("           '{0}' ", user_id);
                Sql1 += string.Format("          ,'{0}' ", scr_id);
                Sql1 += string.Format("          ,( ");
                Sql1 += string.Format("             SELECT NVL(MAX(USE_DATE_SEQ),0) + 1 FROM  TB_CM_SCR_USEHIS ");
                Sql1 += string.Format("             WHERE  USER_ID = '{0}' ", user_id);
                Sql1 += string.Format("             AND    SCR_ID  = '{0}' ", scr_id);
                Sql1 += string.Format("             AND    TO_CHAR(USE_DDTT,'YYYYMMDD') = TO_CHAR(SYSDATE,'YYYYMMDD') ");
                Sql1 += string.Format("            ) ");
                Sql1 += string.Format("          , sysdate ");
                Sql1 += string.Format("          , '{0}' ", btn_gp);
                Sql1 += string.Format("          , '{0}' ", user_id);
                Sql1 += string.Format("          , sysdate ");
                Sql1 += string.Format("         ) ");

                cmd.CommandText = Sql1;
                cmd.ExecuteNonQuery();

                //실행후 성공
                transaction.Commit();

            }
            catch (System.Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        

        //2017.06.13 버튼 -> 픽처박스 수정으로 인한 추가 by 정호준
        public void InsertLogForSearch(string userid, Button  btn, string strSYSTEM_GP)
        {
            string user_id = userid;
            string btn_gp = "1";
            string scr_id;

            string pageid = string.IsNullOrEmpty(btn.Parent.Parent.Parent.Name) ? btn.Parent.Parent.Name : btn.Parent.Parent.Parent.Name;

            if (string.IsNullOrEmpty(Find_Scr_ID_By_PAGE_ID(pageid)))
            {
                return;
            }
            else
            {
                scr_id = Find_Scr_ID_By_PAGE_ID(pageid);
            }

            //디비선언
            SqlConnection conn = OConnect();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                //TB_CM_SCR_USEHIS -> MDMS.dbo.TB_CM_SCR_USEHIS
                string Sql1 = string.Empty;
                Sql1 += string.Format(" INSERT INTO TB_CM_SCR_USEHIS ");
                Sql1 += string.Format("      (                       ");
                Sql1 += string.Format("         USER_ID              ");
                Sql1 += string.Format("        ,SCR_ID               ");
                Sql1 += string.Format("        ,USE_DATE_SEQ         ");
                Sql1 += string.Format("        ,USE_DDTT             ");
                Sql1 += string.Format("        ,BTN_GP               ");
                Sql1 += string.Format("        ,REGISTER             ");
                Sql1 += string.Format("        ,REG_DDTT             ");
                Sql1 += string.Format("       )                      ");
                Sql1 += string.Format(" VALUES (                     ");
                Sql1 += string.Format("           '{0}'              ", user_id);
                Sql1 += string.Format("          ,'{0}'              ", scr_id);
                Sql1 += string.Format("          ,(                  ");
                Sql1 += string.Format("             SELECT ISNULL(MAX(USE_DATE_SEQ),0) + 1 FROM  TB_CM_SCR_USEHIS       ");
                Sql1 += string.Format("             WHERE  USER_ID = '{0}'                                              ", user_id);
                Sql1 += string.Format("             AND    SCR_ID  = '{0}'                                              ", scr_id);
                Sql1 += string.Format("             AND    CONVERT(varchar,USE_DDTT,120) = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                Sql1 += string.Format("            )                                                                    ");
                Sql1 += string.Format("          , (SELECT CONVERT(VARCHAR, GETDATE(), 120))                            ");
                Sql1 += string.Format("          , '{0}'                                                                ", btn_gp);
                Sql1 += string.Format("          , '{0}'                                                                ", user_id);
                Sql1 += string.Format("          , (SELECT CONVERT(VARCHAR, GETDATE(), 120))                            ");
                Sql1 += string.Format("         )                                                                       ");
                //Console.WriteLine(Sql1);
                cmd.CommandText = Sql1;
                cmd.ExecuteNonQuery();

                //실행후 성공
                transaction.Commit();

            }
            catch (System.Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        public void InsertLogForSearch(string userid, PictureBox btn, string strSYSTEM_GP)
        {
            string user_id = userid;
            string btn_gp = "1";
            string scr_id;

            string pageid = string.IsNullOrEmpty(btn.Parent.Parent.Parent.Name) ? btn.Parent.Parent.Name : btn.Parent.Parent.Parent.Name;

            if (string.IsNullOrEmpty(Find_Scr_ID_By_PAGE_ID(pageid)))
            {
                return;
            }
            else
            {
                scr_id = Find_Scr_ID_By_PAGE_ID(pageid);
            }

            //디비선언
            SqlConnection conn = OConnect();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                //TB_CM_SCR_USEHIS -> MDMS.dbo.TB_CM_SCR_USEHIS
                string Sql1 = string.Empty;
                Sql1 += string.Format(" INSERT INTO TB_CM_SCR_USEHIS ");
                Sql1 += string.Format("      (                       ");
                Sql1 += string.Format("         USER_ID              ");
                Sql1 += string.Format("        ,SCR_ID               ");
                Sql1 += string.Format("        ,USE_DATE_SEQ         ");
                Sql1 += string.Format("        ,USE_DDTT             ");
                Sql1 += string.Format("        ,BTN_GP               ");
                Sql1 += string.Format("        ,REGISTER             ");
                Sql1 += string.Format("        ,REG_DDTT             ");
                Sql1 += string.Format("       )                      ");
                Sql1 += string.Format(" VALUES (                     ");
                Sql1 += string.Format("           '{0}'              ", user_id);
                Sql1 += string.Format("          ,'{0}'              ", scr_id);
                Sql1 += string.Format("          ,(                  ");
                Sql1 += string.Format("             SELECT ISNULL(MAX(USE_DATE_SEQ),0) + 1 FROM  TB_CM_SCR_USEHIS       ");
                Sql1 += string.Format("             WHERE  USER_ID = '{0}'                                              ", user_id);
                Sql1 += string.Format("             AND    SCR_ID  = '{0}'                                              ", scr_id);
                Sql1 += string.Format("             AND    CONVERT(varchar,USE_DDTT,120) = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                Sql1 += string.Format("            )                                                                    ");
                Sql1 += string.Format("          , (SELECT CONVERT(VARCHAR, GETDATE(), 120))                            ");
                Sql1 += string.Format("          , '{0}'                                                                ", btn_gp);
                Sql1 += string.Format("          , '{0}'                                                                ", user_id);
                Sql1 += string.Format("          , (SELECT CONVERT(VARCHAR, GETDATE(), 120))                            ");
                Sql1 += string.Format("         )                                                                       ");
                //Console.WriteLine(Sql1);
                cmd.CommandText = Sql1;
                cmd.ExecuteNonQuery();

                //실행후 성공
                transaction.Commit();

            }
            catch (System.Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
        }
        public void InsertLogForSearch(string userid, string pageid, string strSYSTEM_GP = "BISYS")
        {
            string user_id = userid;
            string btn_gp = "1";
            string scr_id;

            if (string.IsNullOrEmpty(Find_Scr_ID_By_PAGE_ID(pageid)))
            {
                return;
            }
            else
            {
                scr_id = Find_Scr_ID_By_PAGE_ID(pageid);
            }

            //디비선언
            SqlConnection conn = OConnect();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                //TB_CM_SCR_USEHIS -> MDMS.dbo.TB_CM_SCR_USEHIS
                string Sql1 = string.Empty;
                Sql1 += string.Format(" INSERT INTO TB_CM_SCR_USEHIS ");
                Sql1 += string.Format("      ( ");
                Sql1 += string.Format("         SYSTEM_GP ");
                Sql1 += string.Format("         USER_ID ");
                Sql1 += string.Format("        ,SCR_ID ");
                Sql1 += string.Format("        ,USE_DATE_SEQ ");
                Sql1 += string.Format("        ,USE_DDTT ");
                Sql1 += string.Format("        ,BTN_GP ");
                Sql1 += string.Format("        ,REGISTER ");
                Sql1 += string.Format("        ,REG_DDTT ");
                Sql1 += string.Format("       ) ");
                Sql1 += string.Format(" VALUES ( ");
                Sql1 += string.Format("           '{0}' ", strSYSTEM_GP);
                Sql1 += string.Format("           '{0}' ", user_id);
                Sql1 += string.Format("          ,'{0}' ", scr_id);
                Sql1 += string.Format("          ,( ");
                Sql1 += string.Format("             SELECT NVL(MAX(USE_DATE_SEQ),0) + 1 FROM  TB_CM_SCR_USEHIS ");
                Sql1 += string.Format("             WHERE  USER_ID = '{0}' ", user_id);
                Sql1 += string.Format("             AND    SCR_ID  = '{0}' ", scr_id);
                Sql1 += string.Format("             AND    TO_CHAR(USE_DDTT,'YYYYMMDD') = TO_CHAR(SYSDATE,'YYYYMMDD') ");
                Sql1 += string.Format("            ) ");
                Sql1 += string.Format("          , sysdate ");
                Sql1 += string.Format("          , '{0}' ", btn_gp);
                Sql1 += string.Format("          , '{0}' ", user_id);
                Sql1 += string.Format("          , sysdate ");
                Sql1 += string.Format("         ) ");

                cmd.CommandText = Sql1;
                cmd.ExecuteNonQuery();

                //실행후 성공
                transaction.Commit();

            }
            catch (System.Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        private void SetItemLoad(DataTable dt)
        {
        }
        public DataTable Menu_Call_List(string r_SYSTEM_GP, string r_USER_ID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                ocmd.CommandText = "dbo.SP_GetMENUList";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();
                ocmd.Parameters.Add("@SYSTEM_GP", SqlDbType.VarChar, 10);
                ocmd.Parameters.Add("@USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@SYSTEM_GP"].Value = r_SYSTEM_GP;
                ocmd.Parameters["@USER_ID"].Value = r_USER_ID;

                //SqlParameter myParameter = cmd.Parameters.Add("@SCR_ID", SqlDbType.Int);
                //myParameter.Direction = ParameterDirection.Output;

                //ocmd.ExecuteReader();

                //string result_value = "";
                //result_value = (cmd.Parameters["@SCR_ID"].Value).ToString();
                //MessageBox.Show(result_value);

                olda.SelectCommand = ocmd;
                olda.Fill(dt);

                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        /// <summary>
        /// 마이크로데이터 tag정보
        /// </summary>
        /// <param name="r_USER_ID"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public DataTable GetTagInfo(string r_USER_ID, string factory)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                ocmd.CommandText = "dbo.SP_GetTagList";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();

                ocmd.Parameters.Add("@USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@USER_ID"].Value = r_USER_ID;

                ocmd.Parameters.Add("@FACTORY", SqlDbType.VarChar, 10);
                ocmd.Parameters["@FACTORY"].Value = factory;

                //SqlParameter myParameter = cmd.Parameters.Add("@SCR_ID", SqlDbType.Int);
                //myParameter.Direction = ParameterDirection.Output;

                //ocmd.ExecuteReader();

                //string result_value = "";
                //result_value = (cmd.Parameters["@SCR_ID"].Value).ToString();
                //MessageBox.Show(result_value);

                olda.SelectCommand = ocmd;
                olda.Fill(dt);

                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        /// <summary>
        /// 분단위데이터 태그 정보
        /// </summary>
        /// <param name="r_USER_ID"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public DataTable GetTagMinuteInfo(string r_USER_ID, string factory)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                ocmd.CommandText = "dbo.SP_GetTagList_Minute";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();

                ocmd.Parameters.Add("@USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@USER_ID"].Value = r_USER_ID;

                ocmd.Parameters.Add("@FACTORY", SqlDbType.VarChar, 10);
                ocmd.Parameters["@FACTORY"].Value = factory;

                //SqlParameter myParameter = cmd.Parameters.Add("@SCR_ID", SqlDbType.Int);
                //myParameter.Direction = ParameterDirection.Output;

                //ocmd.ExecuteReader();

                //string result_value = "";
                //result_value = (cmd.Parameters["@SCR_ID"].Value).ToString();
                //MessageBox.Show(result_value);

                olda.SelectCommand = ocmd;
                olda.Fill(dt);

                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }


        public DataTable Find_Item(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                //Sql = string.Format("SELECT *  FROM MDMS.dbo.TB_TAG_INFO WHERE TAGNAME= '{0}' ", category);
                Sql = string.Format("SELECT TAGNAME ");
                Sql += string.Format(", (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'FACTORY' AND CD_ID = A.FACTORY) AS FACTORY ");
                //Sql += string.Format(", (SELECT ROUTING_NM FROM TB_ROUTING_CD WHERE FACTORY = A.FACTORY AND ROUTING_CD = A.ROUTING_CD) AS ROUTING_CD ");
                Sql += string.Format(", (SELECT ROUTING_CD FROM TB_ROUTING_CD WHERE FACTORY = A.FACTORY AND ROUTING_CD = A.ROUTING_CD) AS ROUTING_CD ");
                Sql += string.Format(", EQP_NM ");
                Sql += string.Format(", ITEM_NM ");
                Sql += string.Format(", ITEM_COMMENT ");
                Sql += string.Format(", PLC_ADDRESS ");
                Sql += string.Format(", MIN_EU ");
                Sql += string.Format(", MAX_EU ");
                Sql += string.Format(", MIN_RAW ");
                Sql += string.Format(", MAX_RAW ");
                Sql += string.Format(", ALLOW_MIN ");
                Sql += string.Format(", ALLOW_MAX ");
                Sql += string.Format(", DISP_UOM ");
                //Sql += string.Format(", CALCULATION_YN ");
                //Sql += string.Format(", CALCULATION_FORMULA ");
                Sql += string.Format(", CALCULATION_SYMBOL ");
                Sql += string.Format(", CALCULATION_VAL ");
                Sql += string.Format(", OCC_CYCLE ");
                Sql += string.Format(", GATHERING_CYCLE ");
                Sql += string.Format(", SUM_CYCLE ");
                Sql += string.Format(", SUM_GP ");
                Sql += string.Format(", ACTIVE_YN ");
                Sql += string.Format(", USE_YN ");
                Sql += string.Format(", REGISTER ");
                Sql += string.Format(", REG_DDTT ");
                Sql += string.Format(", MODIFIER ");
                Sql += string.Format(", MOD_DDTT ");
                Sql += string.Format(", MOD_DDTT ");
                Sql += string.Format(", DATA_TYPE ");
                Sql += string.Format(", TOPIC ");
                Sql += string.Format(", FILTERING_YN ");
                Sql += string.Format(", FILTERING_MIN ");
                Sql += string.Format(", FILTERING_MAX ");
                Sql += string.Format("FROM MDMS.dbo.TB_TAG_INFO A ");
                Sql += string.Format("WHERE TAGNAME = '{0}' ", category);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        //마이크로 데이터
        public DataTable GetTagMicroInfo(string TagName_Info,string Start_time,string End_time,string HeatNo, string UserID, string mode, double second, string Filter)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                string format = "yyyyMMdd HH:mm:ss";
                DateTime cStart_time = DateTime.ParseExact(Start_time, format, null);
                DateTime cEnd_time = DateTime.ParseExact(End_time, format, null);

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                //ocmd.CommandText = "dbo.SP_GetMicroDataList";
                ocmd.CommandText = "dbo.SP_GetMicroDataList7";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();
                ocmd.Parameters.Add("@P_TAGNAME", SqlDbType.VarChar, 4000);
                ocmd.Parameters["@P_TAGNAME"].Value = TagName_Info.ToString();

                ocmd.Parameters.Add("@P_StartDate", SqlDbType.DateTime, 4);
                ocmd.Parameters["@P_StartDate"].Value = cStart_time;

                ocmd.Parameters.Add("@P_EndDate", SqlDbType.DateTime, 4);
                ocmd.Parameters["@P_EndDate"].Value = cEnd_time;

                ocmd.Parameters.Add("@P_HEAT_NO", SqlDbType.VarChar, 8);
                ocmd.Parameters["@P_HEAT_NO"].Value = HeatNo;

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                ocmd.Parameters.Add("@P_Mode", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_Mode"].Value = mode;

                ocmd.Parameters.Add("@P_Second", SqlDbType.Float);
                ocmd.Parameters["@P_Second"].Value = second;

                ocmd.Parameters.Add("@P_FILTER_YN", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_FILTER_YN"].Value = Filter;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        //분단위 데이터
        public DataTable GetTagMinuteInfo(string TagName_Info,
                                          string Start_time,
                                          string End_time,
                                          string HeatNo,
                                          string Steel_CD,
                                          string Item,
                                          string Item_Size,
                                          string Po_No,
                                          string Series,
                                          string Steel_GP,
                                          string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                string format = "yyyyMMdd HH:mm:ss";
                DateTime cStart_time = DateTime.ParseExact(Start_time, format, null);
                DateTime cEnd_time = DateTime.ParseExact(End_time, format, null);

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                //ocmd.CommandText = "dbo.SP_GetMicroDataList";
                ocmd.CommandText = "dbo.SP_GetMinuteDataList";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();
                ocmd.Parameters.Add("@P_TAGNAME", SqlDbType.VarChar, 4000);
                ocmd.Parameters["@P_TAGNAME"].Value = TagName_Info.ToString();

                ocmd.Parameters.Add("@P_StartDate", SqlDbType.DateTime, 4);
                ocmd.Parameters["@P_StartDate"].Value = cStart_time;

                ocmd.Parameters.Add("@P_EndDate", SqlDbType.DateTime, 4);
                ocmd.Parameters["@P_EndDate"].Value = cEnd_time;

                ocmd.Parameters.Add("@P_HEAT_NO", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_HEAT_NO"].Value = HeatNo;

                ocmd.Parameters.Add("@P_STEEL_CD", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_STEEL_CD"].Value = Steel_CD;

                ocmd.Parameters.Add("@P_ITEM", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_ITEM"].Value = Item;

                ocmd.Parameters.Add("@P_ITEM_SIZE", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_ITEM_SIZE"].Value = Item_Size;

                ocmd.Parameters.Add("@P_PO_NO", SqlDbType.VarChar, 20);
                ocmd.Parameters["@P_PO_NO"].Value = Po_No;

                ocmd.Parameters.Add("@P_SERIES", SqlDbType.VarChar, 20);
                ocmd.Parameters["@P_SERIES"].Value = Series;

                ocmd.Parameters.Add("@P_STEEL_GP", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_STEEL_GP"].Value = Steel_GP;

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }


        //X,Y 상관분석 데이터(XYAnalyInq)
        public DataTable GetTagXYAnalyInfo(string TagName_Info, string Start_time, string End_time, string HeatNo, string UserID, string Filter)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                string format = "yyyyMMdd HH:mm:ss";
                DateTime cStart_time = DateTime.ParseExact(Start_time, format, null);
                DateTime cEnd_time = DateTime.ParseExact(End_time, format, null);

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                ocmd.CommandText = "dbo.SP_GetXYAnalyDataList2";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();
                ocmd.Parameters.Add("@P_TAGNAME", SqlDbType.VarChar, 4000);
                ocmd.Parameters["@P_TAGNAME"].Value = TagName_Info.ToString();

                ocmd.Parameters.Add("@P_StartDate", SqlDbType.DateTime, 4);
                ocmd.Parameters["@P_StartDate"].Value = cStart_time;

                ocmd.Parameters.Add("@P_EndDate", SqlDbType.DateTime, 4);
                ocmd.Parameters["@P_EndDate"].Value = cEnd_time;

                ocmd.Parameters.Add("@P_HEAT_NO", SqlDbType.VarChar, 8);
                ocmd.Parameters["@P_HEAT_NO"].Value = HeatNo;

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                ocmd.Parameters.Add("@P_FILTER_YN", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_FILTER_YN"].Value = Filter;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        //startup page SP11
        public DataTable GetStartUpSP(int SPIndex, string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                //if (SPIndex == 1)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList11";
                //if (SPIndex == 2)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList12";
                //if (SPIndex == 3)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList21";
                //if (SPIndex == 4)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList22";
                //if (SPIndex == 5)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList31";
                //if (SPIndex == 6)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList32";
                //if (SPIndex == 7)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList41";
                //if (SPIndex == 8)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList42";
                //if (SPIndex == 9)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList51";
                //if (SPIndex == 10)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList52";
                //if (SPIndex == 11)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList61";
                //if (SPIndex == 12)
                //    ocmd.CommandText = "dbo.SP_GetMainDataList62";

                if (SPIndex == 100)
                    ocmd.CommandText = "dbo.SP_GetMainDataList_STEEL";

                if (SPIndex == 1 || SPIndex == 2 || SPIndex == 3)
                    ocmd.CommandText = "dbo.SP_GetMainDataList_PA080";

                if (SPIndex == 4 || SPIndex == 5 || SPIndex == 6)
                    ocmd.CommandText = "dbo.SP_GetMainDataList_PA120";

                if (SPIndex == 7 || SPIndex == 8 || SPIndex == 9)
                    ocmd.CommandText = "dbo.SP_GetMainDataList_PA100";

                if (SPIndex == 10 || SPIndex == 11 || SPIndex == 12)
                    ocmd.CommandText = "dbo.SP_GetMainDataList_PB080";

                if (SPIndex == 13 || SPIndex == 14 || SPIndex == 15)
                    ocmd.CommandText = "dbo.SP_GetMainDataList_PB120";

                if (SPIndex == 16 || SPIndex == 17 || SPIndex == 18)
                    ocmd.CommandText = "dbo.SP_GetMainDataList_PB100";

                ocmd.CommandType = CommandType.StoredProcedure;

                if (SPIndex != 100)
                {
                    //where 이후 입력 값
                    ocmd.Parameters.Clear();
                    ocmd.Parameters.Add("@P_ROUTING_CD", SqlDbType.VarChar, 10);

                    if (SPIndex == 1 || SPIndex == 4 || SPIndex == 7 || SPIndex == 10 || SPIndex == 13 || SPIndex == 16)
                        ocmd.Parameters["@P_ROUTING_CD"].Value = "A";

                    if (SPIndex == 2 || SPIndex == 5 || SPIndex == 8 || SPIndex == 11 || SPIndex == 14 || SPIndex == 17)
                        ocmd.Parameters["@P_ROUTING_CD"].Value = "B";

                    if (SPIndex == 3 || SPIndex == 6 || SPIndex == 9 || SPIndex == 12 || SPIndex == 15 || SPIndex == 18)
                        ocmd.Parameters["@P_ROUTING_CD"].Value = "C";
                }
                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }


        //공장별_설비별이상조업집계(Chart용 sp)
        public DataTable GetEqpALMSP_Chart(string fromDate, string toDate, string factory, string routing, string eqp, string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                //string format = "yyyyMMdd HH:mm:ss";
                //string format = "yyyyMM";
                //DateTime cStart_time = DateTime.ParseExact(fromDate, format, null);
                //DateTime cEnd_time = DateTime.ParseExact(toDate, format, null);

                fromDate = fromDate.Replace("-", "");
                toDate = toDate.Replace("-", "");

                //string format = "yyyy-MM";
                //DateTime cStart_time = DateTime.ParseExact(fromDate/*.Replace("-", "")*/, format, null);
                //DateTime cEnd_time = DateTime.ParseExact(toDate/*.Replace("-", "")*/, format, null);

                ocmd.CommandText = "dbo.SP_GetEqpALMHInqListC";
                ocmd.CommandType = CommandType.StoredProcedure;

                ocmd.Parameters.Clear();

                //ocmd.Parameters.Add("@P_FR_YYYYMM", SqlDbType.DateTime, 4);
                //ocmd.Parameters.Add("@P_TO_YYYYMM", SqlDbType.DateTime, 4);

                ocmd.Parameters.Add("@P_FR_DATE", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_FR_DATE"].Value = fromDate;// "2017-05-01";

                ocmd.Parameters.Add("@P_TO_DATE", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_TO_DATE"].Value = toDate;// "2017-06-30";

                ocmd.Parameters.Add("@P_FACTORY", SqlDbType.VarChar, 5);  //공장
                ocmd.Parameters["@P_FACTORY"].Value = factory;// "PA2";

                ocmd.Parameters.Add("@P_ROUTING_CD", SqlDbType.VarChar, 10);  //공정
                ocmd.Parameters["@P_ROUTING_CD"].Value = routing;// "E";

                ocmd.Parameters.Add("@P_EQP_NM", SqlDbType.VarChar, 100); //설비
                ocmd.Parameters["@P_EQP_NM"].Value = eqp;// "로벽버너";

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID; //"admin";// UserID;

                //where 이후 입력 값
                //ocmd.Parameters.Clear();
                //ocmd.Parameters.Add("@P_TAGNAME", SqlDbType.VarChar, 4000);
                //ocmd.Parameters["@P_TAGNAME"].Value = TagName_Info.ToString();

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        //공장별_설비별이상조업집계(grid용 sp)
        public DataTable GetEqpALMSP_Grid(string fromDate, string toDate, string factory, string routing, string eqp, string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                //string format = "yyyyMMdd HH:mm:ss";
                //string format = "yyyyMM";
                //DateTime cStart_time = DateTime.ParseExact(fromDate, format, null);
                //DateTime cEnd_time = DateTime.ParseExact(toDate, format, null);

                fromDate = fromDate.Replace("-", "");
                toDate = toDate.Replace("-", "");

                //string format = "yyyy-MM";
                //DateTime cStart_time = DateTime.ParseExact(fromDate/*.Replace("-", "")*/, format, null);
                //DateTime cEnd_time = DateTime.ParseExact(toDate/*.Replace("-", "")*/, format, null);

                ocmd.CommandText = "dbo.SP_GetEqpALMHInqListG";
                ocmd.CommandType = CommandType.StoredProcedure;

                ocmd.Parameters.Clear();

                //ocmd.Parameters.Add("@P_FR_YYYYMM", SqlDbType.DateTime, 4);
                //ocmd.Parameters.Add("@P_TO_YYYYMM", SqlDbType.DateTime, 4);

                ocmd.Parameters.Add("@P_FR_DATE", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_FR_DATE"].Value = fromDate;// "2017-05-01";

                ocmd.Parameters.Add("@P_TO_DATE", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_TO_DATE"].Value = toDate;// "2017-06-30";

                ocmd.Parameters.Add("@P_FACTORY", SqlDbType.VarChar, 5);  //공장
                ocmd.Parameters["@P_FACTORY"].Value = factory;// "PA2";

                ocmd.Parameters.Add("@P_ROUTING_CD", SqlDbType.VarChar, 10);  //공정
                ocmd.Parameters["@P_ROUTING_CD"].Value = routing;// "E";

                ocmd.Parameters.Add("@P_EQP_NM", SqlDbType.VarChar, 100); //설비
                ocmd.Parameters["@P_EQP_NM"].Value = eqp;// "로벽버너";

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID; //"admin";// UserID;

                //where 이후 입력 값
                //ocmd.Parameters.Clear();
                //ocmd.Parameters.Add("@P_TAGNAME", SqlDbType.VarChar, 4000);
                //ocmd.Parameters["@P_TAGNAME"].Value = TagName_Info.ToString();

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        //공장별_이상조업현황
        public DataTable GetALMHisSP_Grid(string fromDate, string toDate, string factory, string routing, string eqp, string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                string format = "yyyyMMdd HH:mm:ss";
                DateTime cStart_time = DateTime.ParseExact(fromDate, format, null);
                DateTime cEnd_time = DateTime.ParseExact(toDate, format, null);

                //fromDate = fromDate.Replace("-", "");
                //toDate = toDate.Replace("-", "");

                //string format = "yyyy-MM";
                //DateTime cStart_time = DateTime.ParseExact(fromDate/*.Replace("-", "")*/, format, null);
                //DateTime cEnd_time = DateTime.ParseExact(toDate/*.Replace("-", "")*/, format, null);

                ocmd.CommandText = "dbo.SP_GetALMHisInqList";
                ocmd.CommandType = CommandType.StoredProcedure;

                ocmd.Parameters.Clear();

                //ocmd.Parameters.Add("@P_FR_YYYYMM", SqlDbType.DateTime, 4);
                //ocmd.Parameters.Add("@P_TO_YYYYMM", SqlDbType.DateTime, 4);

                ocmd.Parameters.Add("@P_StartDate", SqlDbType.DateTime, 4);
                ocmd.Parameters["@P_StartDate"].Value = cStart_time;

                ocmd.Parameters.Add("@P_EndDate", SqlDbType.DateTime, 4);
                ocmd.Parameters["@P_EndDate"].Value = cEnd_time;

                ocmd.Parameters.Add("@P_FACTORY", SqlDbType.VarChar, 5);  //공장
                ocmd.Parameters["@P_FACTORY"].Value = factory;

                ocmd.Parameters.Add("@P_ROUTING_CD", SqlDbType.VarChar, 10);  //공정
                ocmd.Parameters["@P_ROUTING_CD"].Value = routing;

                ocmd.Parameters.Add("@P_EQP_NM", SqlDbType.VarChar, 100); //설비
                ocmd.Parameters["@P_EQP_NM"].Value = eqp;

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                //where 이후 입력 값
                //ocmd.Parameters.Clear();
                //ocmd.Parameters.Add("@P_TAGNAME", SqlDbType.VarChar, 4000);
                //ocmd.Parameters["@P_TAGNAME"].Value = TagName_Info.ToString();

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        //전공장 이상조업현황
        public DataTable GetAllALMSP(string fromDate, string toDate, string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                //string format = "yyyyMMdd HH:mm:ss";
                //                string format = "yyyyMM";
                //              DateTime cStart_time = DateTime.ParseExact(fromDate, format, null);
                //            DateTime cEnd_time = DateTime.ParseExact(toDate, format, null);

                fromDate = fromDate.Replace("-", "");
                toDate = toDate.Replace("-", "");

                //string format = "yyyy-MM";
                //DateTime cStart_time = DateTime.ParseExact(fromDate/*.Replace("-", "")*/, format, null);
                //DateTime cEnd_time = DateTime.ParseExact(toDate/*.Replace("-", "")*/, format, null);


                ocmd.CommandText = "dbo.SP_GetAllALMHInqList";
                ocmd.CommandType = CommandType.StoredProcedure;

                ocmd.Parameters.Clear();

                //ocmd.Parameters.Add("@P_FR_YYYYMM", SqlDbType.DateTime, 4);
                ocmd.Parameters.Add("@P_FR_YYYYMM", SqlDbType.VarChar, 6);
                ocmd.Parameters["@P_FR_YYYYMM"].Value = fromDate;

                //ocmd.Parameters.Add("@P_TO_YYYYMM", SqlDbType.DateTime, 4);
                ocmd.Parameters.Add("@P_TO_YYYYMM", SqlDbType.VarChar, 6);
                ocmd.Parameters["@P_TO_YYYYMM"].Value = toDate;

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                //where 이후 입력 값
                //ocmd.Parameters.Clear();
                //ocmd.Parameters.Add("@P_TAGNAME", SqlDbType.VarChar, 4000);
                //ocmd.Parameters["@P_TAGNAME"].Value = TagName_Info.ToString();

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        public DataTable GetSumDataInfo(string TagName_Info,
                                  string Start_time,
                                  string End_time,
                                  string Steel_CD,
                                  string Item,
                                  string Item_Size,
                                  string Po_No,
                                  string Series,
                                  string Steel_GP,
                                  string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                //string format = "yyyyMMdd HH:mm:ss";
                //DateTime cStart_time = DateTime.ParseExact(Start_time, format, null);
                //DateTime cEnd_time = DateTime.ParseExact(End_time, format, null);

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                //ocmd.CommandText = "dbo.SP_GetMicroDataList";
                ocmd.CommandText = "dbo.SP_GetSumDataList";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();
                ocmd.Parameters.Add("@P_TAGNAME", SqlDbType.NVarChar, 4000);
                ocmd.Parameters["@P_TAGNAME"].Value = TagName_Info.ToString();

                ocmd.Parameters.Add("@P_StartDate", SqlDbType.VarChar, 8);
                ocmd.Parameters["@P_StartDate"].Value = Start_time;

                ocmd.Parameters.Add("@P_EndDate", SqlDbType.VarChar, 8);
                ocmd.Parameters["@P_EndDate"].Value = End_time;

                ocmd.Parameters.Add("@P_STEEL_CD", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_STEEL_CD"].Value = Steel_CD;

                ocmd.Parameters.Add("@P_ITEM", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_ITEM"].Value = Item;

                ocmd.Parameters.Add("@P_ITEM_SIZE", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_ITEM_SIZE"].Value = Item_Size;

                ocmd.Parameters.Add("@P_PO_NO", SqlDbType.VarChar, 20);
                ocmd.Parameters["@P_PO_NO"].Value = Po_No;

                ocmd.Parameters.Add("@P_SERIES", SqlDbType.VarChar, 20);
                ocmd.Parameters["@P_SERIES"].Value = Series;

                ocmd.Parameters.Add("@P_STEEL_GP", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_STEEL_GP"].Value = Steel_GP;

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }


        /// <summary>
        /// My Data 관리
        /// </summary>
        /// <param name="TagName_Info"></param>
        /// <param name="Start_time"></param>
        /// <param name="End_time"></param>
        /// <param name="HeatNo"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable GetMyDataInfo(string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                //ocmd.CommandText = "dbo.SP_GetMicroDataList";
                ocmd.CommandText = "dbo.SP_GetMyDataList";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        public DataTable SetMyDataInfo(string UserID, int Reg_Seq, int Disp_Seq, string Title)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                //ocmd.CommandText = "dbo.SP_GetMicroDataList";
                ocmd.CommandText = "dbo.SP_SetMyData_UPD";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                ocmd.Parameters.Add("@P_REG_SEQ", SqlDbType.Int, 4);
                ocmd.Parameters["@P_REG_SEQ"].Value = Reg_Seq;

                ocmd.Parameters.Add("@P_DISP_SEQ", SqlDbType.Int, 4);
                ocmd.Parameters["@P_DISP_SEQ"].Value = Disp_Seq;

                ocmd.Parameters.Add("@P_TITLE", SqlDbType.VarChar, 60);
                ocmd.Parameters["@P_TITLE"].Value = Title;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                //MessageBox.Show("저장되었습니다");

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        public DataTable DelMyDataInfo(string UserID, int Reg_Seq)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                //ocmd.CommandText = "dbo.SP_GetMicroDataList";
                ocmd.CommandText = "dbo.SP_SetMyData_DEL";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                ocmd.Parameters.Add("@P_REG_SEQ", SqlDbType.Int, 4);
                ocmd.Parameters["@P_REG_SEQ"].Value = Reg_Seq;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                //MessageBox.Show("삭제되었습니다");

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }


        public DataTable Find_Item_Get_Distinct(string Sdt, string Edt)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT DISTINCT(TagName) FROM TEST.dbo.TagValueData WHERE DateTime >= '{0}' and DateTime <= '{1}' ORDER BY TagName", Sdt, Edt);
                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }



        public DataTable Find_Item_Test(string Sdt, string Edt)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                //Sql = string.Format("SELECT *  FROM MDMS.dbo.TB_TAG_INFO WHERE TAGNAME= '{0}' ", category);
                Sql = string.Format("SELECT  * FROM MDMS.dbo.TB_CM_COM_CD ");              
                Sql = string.Format("SELECT TagName, DateTime, Value01, Value02, Value03, Value04, Heat, PO FROM TEST.dbo.TagValueData WHERE DateTime >= '{0}' and DateTime <= '{1}' ORDER BY TagName", Sdt, Edt);
                Sql = string.Format("SELECT TagName, DateTime, Value01, Value02, Value03, Value04, Heat, PO FROM TEST.dbo.TagValueData ORDER BY TagName", Sdt, Edt);
                //Sql = string.Format("SELECT TagName, DateTime, Value, Heat, PO FROM TEST.dbo.TEST where TagName = 'CCM_LOOP_SPRAY_1_PSS_ST5'");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable Find_Item_acce()
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                //Sql = string.Format("SELECT *  FROM MDMS.dbo.TB_TAG_INFO WHERE TAGNAME= '{0}' ", category);
                Sql = "SELECT *  FROM [gettaginfo]";
                //Sql = string.Format("SELECT TagName, DateTime, Value, Heat, PO FROM TEST.dbo.TEST where TagName = 'CCM_LOOP_SPRAY_1_PSS_ST5'");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        //정호준 작업 START
        public DataTable getSysGp()
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();

                Sql += string.Format("/*2017.06.02 시스템구분 조회 by 정호준*/");
                Sql += String.Format("SELECT CD_ID                            ");
                Sql += String.Format("      ,CD_NM                            ");
                Sql += String.Format("  FROM TB_CM_COM_CD                     ");
                Sql += String.Format(" WHERE CATEGORY = 'SYSTEM_GP'           ");
                Sql += String.Format("   AND USE_YN = 'Y'                     ");
                Sql += String.Format(" ORDER BY SORT_SEQ                      ");


                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable getFactoryGp()
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();

                Sql += String.Format("SELECT CD_ID                            ");
                Sql += String.Format("      ,CD_NM                            ");
                Sql += String.Format("  FROM TB_CM_COM_CD                     ");
                Sql += String.Format(" WHERE CATEGORY = 'FACTORY_GP'           ");
                //Sql += String.Format("   AND USE_YN = 'Y'                     ");
                //Sql += String.Format(" ORDER BY SORT_SEQ                      ");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;
            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable getRouting(string FactoryGP)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();

                Sql += string.Format(" SELECT DISTINCT ROUTING_CD, ROUTING_NM, SORT_SEQ");
                Sql += string.Format(" FROM dbo.TB_ROUTING_CD                          ");
                Sql += string.Format(" WHERE FACTORY LIKE '{0}'                        ", FactoryGP + "%");
                Sql += String.Format(" ORDER BY SORT_SEQ                               ");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;
            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable GetComItemRegList(string FactoryGP, string RoutingCD, string UserID)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                ocmd.CommandText = "dbo.SP_GetComItemRegList";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();

                ocmd.Parameters.Add("@P_FACTORY_GP", SqlDbType.VarChar, 5);
                ocmd.Parameters["@P_FACTORY_GP"].Value = FactoryGP;

                ocmd.Parameters.Add("@P_ROUTING_CD", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_ROUTING_CD"].Value = RoutingCD;

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = UserID;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        public DataTable SetComItemReg_CRE(string com_item_nm, string b_tagName, string c_tagName, string d_tagName, string user_id)
        {
            SqlConnection conn = OConnect();
            SqlTransaction transaction = null;
            SqlCommand ocmd = new SqlCommand();
            SqlDataAdapter olda = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                ocmd.Connection = conn;
                transaction = conn.BeginTransaction();
                ocmd.Transaction = transaction;

                /*
                //MessageBox.Show(TagName_Info.ToString());* 
                string[] TagName_Infos = TagName_Info.Split(',');
                for (int i = 0; i < TagName_Infos.Length; i++)
                {
                    //MessageBox.Show(TagName_Infos[i]);
                }
                string Search_TG_No = TagName_Infos[0].ToString();
                */

                //ocmd.CommandText = "dbo.SP_GetMicroDataList";
                ocmd.CommandText = "dbo.SP_SetComItemReg_CRE";
                ocmd.CommandType = CommandType.StoredProcedure;
                //where 이후 입력 값
                ocmd.Parameters.Clear();

                ocmd.Parameters.Add("@P_COM_ITEM_NM", SqlDbType.NVarChar, 512);
                ocmd.Parameters["@P_COM_ITEM_NM"].Value = com_item_nm;

                ocmd.Parameters.Add("@P_B_TAGNAME", SqlDbType.NVarChar, 256);
                ocmd.Parameters["@P_B_TAGNAME"].Value = b_tagName;

                ocmd.Parameters.Add("@P_C_TAGNAME", SqlDbType.NVarChar, 256);
                ocmd.Parameters["@P_C_TAGNAME"].Value = c_tagName;

                ocmd.Parameters.Add("@P_D_TAGNAME", SqlDbType.NVarChar, 256);
                ocmd.Parameters["@P_D_TAGNAME"].Value = d_tagName;

                ocmd.Parameters.Add("@P_USER_ID", SqlDbType.VarChar, 10);
                ocmd.Parameters["@P_USER_ID"].Value = user_id;

                olda.SelectCommand = ocmd;
                olda.Fill(dt);
                //int rcnt = dt.Rows.Count;
                //실행후 성공
                transaction.Commit();

                //MessageBox.Show("저장되었습니다");

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB쿼리 실패");
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (ocmd != null)
                    ocmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            return dt;

        }

        public DataTable getAclGrp(string sysgp)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();



                Sql += string.Format("/* 2017.06.07 권한리스트 조회 by 정호준*/");
                Sql += string.Format("SELECT ACL_GRP_ID                        ");
                Sql += string.Format("      ,ACL_GRP_NM                        ");
                Sql += string.Format("  FROM TB_CM_ACL_GRP                     ");
                Sql += string.Format(" WHERE SYSTEM_GP = '" + sysgp + "'        ");
                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        /* 2017.07.03 공장 데이터 가져오기 by 정호준*/
        public DataTable getFactory()
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();

                Sql += string.Format("/* 2017.07.03 공장 가져오기 by 정호준*/");
                Sql += string.Format("SELECT CD_ID                           ");
                Sql += string.Format("      ,CD_NM                           ");
                Sql += string.Format("  FROM TB_CM_COM_CD                    ");
                Sql += string.Format(" WHERE CATEGORY  = 'FACTORY'           ");
                Sql += string.Format("   AND USE_YN = 'Y'                    ");
                Sql += string.Format(" ORDER BY SORT_SEQ                     ");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        //정호준 작업 END
        //
        //


        public bool isExecuting = false;
        //SqlCommand SqlGCommand = new SqlCommand();
        //OCJ 작업 
        //-----------------------------------------------------------------------
        //Name    : ExecuteDataSet
        //설명    : Sql문 실행 후에 DataTable로 반환하는 함수
        //파라메터 : strSql (Sql문 이름)
        //        : ParameterValues (StoreProcedure 실행하는데 필요한 파라메터 정보들)  
        //-----------------------------------------------------------------------

        public DataSet ExecuteDataSet(StringBuilder strSql)
        {
            return ExecuteDataSet(strSql, (clsParameterMember)null);
        }
        public DataSet ExecuteDataSet(StringBuilder strSql, clsParameterMember param)
        {
            return ExecuteDataSet((SqlTransaction)null, strSql, param);
        }
        public DataSet ExecuteDataSet(SqlTransaction transaction, StringBuilder strSql)
        {
            return ExecuteDataSet(transaction, strSql, (clsParameterMember)null);
        }
        public DataSet ExecuteDataSet(SqlTransaction transaction, StringBuilder strSql, clsParameterMember param)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, transaction, strSql, param);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);

            cmd.Parameters.Clear();
            return ds;
        }

        private void PrepareCommand(SqlCommand command, SqlTransaction transaction, StringBuilder strSql,
            clsParameterMember commandParameters)
        {
            SqlConnection conn = OConnect();

            conn.Open();
            command.Connection = conn;
            command.CommandText = strSql.ToString();
            command.CommandType = CommandType.Text;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        private void AttachParameters(SqlCommand command, clsParameterMember param)
        {
            if (param == null) return;

            for (int i = 0; i < param.Count; i++)
            {
                SqlParameter cmdPara = new SqlParameter(param.GetName(i), param.GetType(i));
                cmdPara.Value = param.GetValue(i);

                command.Parameters.Add(cmdPara);
            }
        }


        //-----------------------------------------------------------------------
        //Name    : ExecuteStoreProcedureDataTable
        //설명    : Stored Procedure 실행 후에 DataTable로 반환하는 함수
        //파라메터 : spName (StoreProcedure 이름)
        //        : ParameterValues (StoreProcedure 실행하는데 필요한 파라메터 정보들)  
        //-----------------------------------------------------------------------
        public DataTable ExecuteStoreProcedureDataTable(string spName, clsParameterMember parameterValues)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;

            SqlCommand command = new SqlCommand();
            string outParamterName = "";

            conn.Open();
            command.Connection = conn;
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;
            //무제한 으로 설정
            command.CommandTimeout = 0;
            try
            {
                if ((parameterValues != null) && (parameterValues.Count > 0))
                {
                    SqlParameter[] commandParamters = new SqlParameter[parameterValues.Count];

                    for (int i = 0; i < parameterValues.Count; i++)
                    {
                        commandParamters[i] = new SqlParameter(parameterValues.GetName(i), parameterValues.GetValue(i));
                        commandParamters[i].SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameterValues.GetType(i).ToString());
                        commandParamters[i].Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), parameterValues.GetDirection(i).ToString());

                        if (commandParamters[i].Direction == ParameterDirection.Output)
                            outParamterName = commandParamters[i].ParameterName;

                        if (commandParamters[i].Direction == ParameterDirection.ReturnValue)
                            outParamterName = commandParamters[i].ParameterName;

                        command.Parameters.Add(commandParamters[i]);
                    }

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    dt = new DataTable();
                    da.Fill(dt);

                    da.Dispose();

                    command.Parameters.Clear();
                    return dt;
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    dt = new DataTable();
                    da.Fill(dt);

                    da.Dispose();

                    command.Parameters.Clear();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteStoreProcedure Exception Message : " + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
            }
        }

        //-----------------------------------------------------------------------
        //Name    : ExecuteStoreProcedureDataTable
        //설명    : Stored Procedure 실행 후에 그 결과를 ReturnValue의 Count로 반환하는 함수
        //파라메터 : spName (StoreProcedure 이름)
        //        : ParameterValues (StoreProcedure 실행하는데 필요한 파라메터 정보들)  
        //-----------------------------------------------------------------------
        public int ExecuteStoreProcedure(string spName, clsParameterMember parameterValues)
        {
            SqlConnection conn = OConnect();

            SqlCommand command = new SqlCommand();
            string outParamterName = "";
            int retVal = 0;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = spName;
                command.CommandType = CommandType.StoredProcedure;

                //무제한 으로 설정
                command.CommandTimeout = 0;
                if ((parameterValues != null) && (parameterValues.Count > 0))
                {
                    SqlParameter[] commandParamters = new SqlParameter[parameterValues.Count];

                    for (int i = 0; i < parameterValues.Count; i++)
                    {
                        commandParamters[i] = new SqlParameter(parameterValues.GetName(i), parameterValues.GetValue(i));
                        commandParamters[i].SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameterValues.GetType(i).ToString());
                        commandParamters[i].Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), parameterValues.GetDirection(i).ToString());

                        if (commandParamters[i].Direction == ParameterDirection.Output)
                            outParamterName = commandParamters[i].ParameterName;

                        if (commandParamters[i].Direction == ParameterDirection.ReturnValue)
                            outParamterName = commandParamters[i].ParameterName;

                        command.Parameters.Add(commandParamters[i]);
                    }
                    command.ExecuteNonQuery();
                    retVal = Convert.ToInt32(command.Parameters[outParamterName].Value);

                    command.Parameters.Clear();
                    return retVal;
                }
                else
                {
                    retVal = command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    return retVal;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteStoreProcedure Exception Message : " + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
            }
        }

        public SqlCommand ExecuteStoreProcedureRecCmd(string spName, clsParameterMember parameterValues)
        {
            SqlConnection conn = OConnect();

            object rtnparameterValues = new clsParameterMember();

            SqlCommand command = new SqlCommand();
            string outParamterName = "";
            int retVal = 0;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = spName;
                command.CommandType = CommandType.StoredProcedure;

                //무제한 으로 설정
                command.CommandTimeout = 0;
                if ((parameterValues != null) && (parameterValues.Count > 0))
                {
                    SqlParameter[] commandParamters = new SqlParameter[parameterValues.Count];

                    for (int i = 0; i < parameterValues.Count; i++)
                    {
                        commandParamters[i] = new SqlParameter(parameterValues.GetName(i), parameterValues.GetValue(i));
                        commandParamters[i].SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameterValues.GetType(i).ToString());
                        commandParamters[i].Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), parameterValues.GetDirection(i).ToString());
                        

                        if (commandParamters[i].Direction == ParameterDirection.Output)
                        {
                            commandParamters[i].Size = (int)parameterValues.GetSize(i);
                            outParamterName = commandParamters[i].ParameterName;
                        }
                            

                        if (commandParamters[i].Direction == ParameterDirection.ReturnValue)
                            outParamterName = commandParamters[i].ParameterName;

                        command.Parameters.Add(commandParamters[i]);
                    }
                    command.ExecuteNonQuery();
                    //retVal = Convert.ToInt32(command.Parameters[outParamterName].Value);

                    //command.Parameters.Clear();
                    return command;
                }
                else
                {
                    retVal = command.ExecuteNonQuery();

                    //command.Parameters.Clear();
                    return command;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteStoreProcedure Exception Message : " + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
            }
        }

        public int ExecuteStoreProcedure(SqlTransaction transaction, string spName, clsParameterMember parameterValues)
        {
            SqlConnection conn = OConnect();

            SqlCommand command = new SqlCommand();
            string outParamterName = "";
            int retVal = 0;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = spName;
                command.CommandType = CommandType.StoredProcedure;

                //무제한 으로 설정
                command.CommandTimeout = 0;
                if (transaction != null) command.Transaction = transaction;

                if ((parameterValues != null) && (parameterValues.Count > 0))
                {
                    SqlParameter[] commandParamters = new SqlParameter[parameterValues.Count];

                    for (int i = 0; i < parameterValues.Count; i++)
                    {
                        commandParamters[i] = new SqlParameter(parameterValues.GetName(i), parameterValues.GetValue(i));
                        commandParamters[i].SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameterValues.GetType(i).ToString());
                        commandParamters[i].Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), parameterValues.GetDirection(i).ToString());

                        if (commandParamters[i].Direction == ParameterDirection.Output)
                            outParamterName = commandParamters[i].ParameterName;

                        if (commandParamters[i].Direction == ParameterDirection.ReturnValue)
                            outParamterName = commandParamters[i].ParameterName;

                        command.Parameters.Add(commandParamters[i]);
                    }
                    command.ExecuteNonQuery();
                    retVal = Convert.ToInt32(command.Parameters[outParamterName].Value);

                    command.Parameters.Clear();
                    return retVal;
                }
                else
                {
                    retVal = command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    return retVal;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteStoreProcedure Exception Message : " + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
            }
        }

        public bool IsCancelled = false;

        public async Task<DataTable> ExecuteStoreProcedureDataTableAsync(string spName, clsParameterMember parameterValues)
        {
            SqlConnection conn = OConnectAsync();
            DataTable dt = null;

            SqlCommand command = new SqlCommand();
            string outParamterName = "";

            conn.Open();
            command.Connection = conn;
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            //무제한 으로 설정
            command.CommandTimeout = 0;            
            try
            {
                if ((parameterValues != null) && (parameterValues.Count > 0))
                {
                    SqlParameter[] commandParamters = new SqlParameter[parameterValues.Count];

                    for (int i = 0; i < parameterValues.Count; i++)
                    {
                        commandParamters[i] = new SqlParameter(parameterValues.GetName(i), parameterValues.GetValue(i));
                        commandParamters[i].SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameterValues.GetType(i).ToString());
                        commandParamters[i].Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), parameterValues.GetDirection(i).ToString());

                        if (commandParamters[i].Direction == ParameterDirection.Output)
                            outParamterName = commandParamters[i].ParameterName;

                        if (commandParamters[i].Direction == ParameterDirection.ReturnValue)
                            outParamterName = commandParamters[i].ParameterName;

                        command.Parameters.Add(commandParamters[i]);
                    }

                    isExecuting = true;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    dt = new DataTable();

                    await Task.Run(() =>
                    {
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                            throw new Exception(ex.Message);
                        }
                    });

                    command.Parameters.Clear();

                    return dt;
                }
                else
                {
                    isExecuting = true;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    dt = new DataTable();

                    await Task.Run(() =>
                    {
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                            throw new Exception(ex.Message);
                        }
                    }
                    );

                    command.Parameters.Clear();

                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();

                isExecuting = false;
                if (command != null)
                    command.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
            }
        }
        public async Task<DataTable> ExecuteStoreProcedureDataTableAsync(string spName, clsParameterMember parameterValues, CancellationToken token)
        {
            SqlConnection conn = OConnectAsync();
            DataTable dt = null;

            SqlCommand command = new SqlCommand();
            DbDataReader dbr = null;

            string outParamterName = "";

            conn.Open();
            command.Connection = conn;
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            //무제한 으로 설정
            command.CommandTimeout = 0;

            try
            {
                if ((parameterValues != null) && (parameterValues.Count > 0))
                {
                    SqlParameter[] commandParamters = new SqlParameter[parameterValues.Count];

                    for (int i = 0; i < parameterValues.Count; i++)
                    {
                        commandParamters[i] = new SqlParameter(parameterValues.GetName(i), parameterValues.GetValue(i));
                        commandParamters[i].SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameterValues.GetType(i).ToString());
                        commandParamters[i].Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), parameterValues.GetDirection(i).ToString());

                        if (commandParamters[i].Direction == ParameterDirection.Output)
                            outParamterName = commandParamters[i].ParameterName;

                        if (commandParamters[i].Direction == ParameterDirection.ReturnValue)
                            outParamterName = commandParamters[i].ParameterName;

                        command.Parameters.Add(commandParamters[i]);
                    }

                    dt = new DataTable();

                    Task task = Task.Run(async() =>
                    {
                        dbr = await command.ExecuteReaderAsync (token);
                        if (token.IsCancellationRequested)
                        {
                            if (!dbr.IsClosed)
                            {
                                command.Cancel();
                                dbr.Close();
                            }
                            token.ThrowIfCancellationRequested();
                        }

                    }, token);

                    await task.ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                        {
                            object[] values = new object[dbr.FieldCount];

                            for (int n = 0; n < dbr.FieldCount; n++)
                            {
                                dt.Columns.Add(dbr.GetName(n), dbr.GetFieldType(n));
                            }

                            while (dbr.Read())
                            {
                                if (token.IsCancellationRequested)
                                {
                                    if (!dbr.IsClosed)
                                    {
                                        command.Cancel();
                                        dbr.Close();
                                    }

                                    token.ThrowIfCancellationRequested();
                                    break;
                                }

                                dbr.GetValues(values);
                                dt.Rows.Add(values);
                            }
                        }
                        else if(t.Status == TaskStatus.Faulted)
                        {
                            throw new AggregateException(t.Exception.Message, t.Exception.InnerException);
                        }
                        else if (t.Status == TaskStatus.Canceled)
                        {
                            token.ThrowIfCancellationRequested();
                        }

                    }, token);

                    command.Parameters.Clear();

                    return dt;
                }
                else
                {
                    dt = new DataTable();

                    Task task = Task.Run(async () =>
                    {
                        dbr = await command.ExecuteReaderAsync(token);
                        if (token.IsCancellationRequested)
                        {
                            if (!dbr.IsClosed)
                            {
                                command.Cancel();
                                dbr.Close();
                            }
                            token.ThrowIfCancellationRequested();
                        }

                    }, token);

                    await task.ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                        {
                            object[] values = new object[dbr.FieldCount];

                            for (int n = 0; n < dbr.FieldCount; n++)
                            {
                                dt.Columns.Add(dbr.GetName(n), dbr.GetFieldType(n));
                            }

                            while (dbr.Read())
                            {
                                if (token.IsCancellationRequested)
                                {
                                    if (!dbr.IsClosed)
                                    {
                                        command.Cancel();
                                        dbr.Close();
                                    }

                                    token.ThrowIfCancellationRequested();
                                    break;
                                }

                                dbr.GetValues(values);
                                dt.Rows.Add(values);
                            }
                        }

                    }, token);

                    command.Parameters.Clear();

                    return dt;
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("OperationCanceledException Message : " + ex.Message);
                Console.WriteLine("DataRow.Count : " + dt.Rows.Count);
                throw new OperationCanceledException(ex.Message, token);
            }
            catch (AggregateException ex)
            {
                if (!dbr.IsClosed)
                {
                    dt.Clear();
                    command.Cancel();
                    dbr.Close();
                }
                Console.WriteLine("AggregateException Message : " + ex.Message);
                Console.WriteLine("DataRow.Count : " + dt.Rows.Count);
                throw new AggregateException(ex.Message, ex);
            }
            catch (SqlException ex)
            {
                if (!dbr.IsClosed)
                {
                    dt.Clear();
                    command.Cancel();
                    dbr.Close();
                }

                Console.WriteLine("SqlException Message : " + ex.Message);
                Console.WriteLine("DataRow.Count : " + dt.Rows.Count);
                throw new AggregateException(ex.Message, ex);
            }
            catch(OutOfMemoryException ex)
            {
                if (!dbr.IsClosed)
                {
                    dt.Clear();
                    command.Cancel();
                    dbr.Close();
                }

                Console.WriteLine("OutOfMemoryException Message : " + ex.Message);
                Console.WriteLine("DataRow.Count : " + dt.Rows.Count);
                throw new AggregateException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                if (!dbr.IsClosed)
                {
                    command.Cancel();
                    dbr.Close();
                }

                throw new AggregateException(ex.Message, ex);
            }
            finally
            {
                isExecuting = false;
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                    
                if (conn != null)
                {
                    conn.Close();       //데이터베이스연결해제
                    conn.Dispose();
                    conn = null;
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                }
            }
        }
    }

    //-- OCJ 작업
}
